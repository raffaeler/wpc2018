using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

// oAuth2.0
// oAuth + Bearer Token: https://tools.ietf.org/html/rfc6750

// Jwt specs: https://tools.ietf.org/html/rfc7519
// 4.1.1.  "iss" (Issuer) Claim
// 4.1.2.  "sub" (Subject) Claim
// 4.1.3.  "aud" (Audience) Claim
// 4.1.4.  "exp" (Expiration Time) Claim
// 4.1.5.  "nbf" (Not Before) Claim
// 4.1.6.  "iat" (Issued At) Claim
// 4.1.7.  "jti" (JWT ID) Claim

namespace RafIdentityProviderLibrary
{
    public class ServiceLoginMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly ServiceLoginOptions _options;

        public ServiceLoginMiddleware(RequestDelegate next,
            //IHostingEnvironment hostingEnvironment,
            IConfiguration configuration,
            ILoggerFactory loggerFactory,
            IOptions<ServiceLoginOptions> options)
        {
            _next = next;
            //_hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<ServiceLoginMiddleware>();
            _options = options.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var request = httpContext.Request;
            var response = httpContext.Response;

            if (!request.Path.Equals(_options.Path, StringComparison.InvariantCultureIgnoreCase))
            {
                await _next(httpContext);
                return;
            }

            if (!request.Method.Equals("POST") || !request.HasFormContentType)
            {
                response.StatusCode = 400;
                await response.WriteAsync("Request must have a POST verb and contains a valid form content type");
                return;
            }

            string grant = null;
            string token = null;
            if (request.Form.TryGetValue(OAuth2Constants.GrantType, out StringValues grantValues))
            {
                grant = grantValues.First();
            }

            switch (grant)
            {
                case null:
                case OAuth2Constants.Password:
                    string username = request.Form[OAuth2Constants.Username];
                    string password = request.Form[OAuth2Constants.Password];
                    string clientid = request.Form[OAuth2Constants.ClientId];
                    string cliensecret = request.Form[OAuth2Constants.ClientSecret];

                    token = CreateJwtToken(username, clientid);
                    break;

                case OAuth2Constants.RefreshToken:
                    token = await CreateTokenFromRefreshToken(httpContext);
                    break;

                default:
                    var msg = $"Invalid grant type: {grant}";
                    _logger.LogWarning(msg);
                    await response.WriteAsync(msg);
                    break;
            }

            if (string.IsNullOrEmpty(token))
            {
                response.StatusCode = 400;
                await response.WriteAsync("Invalid token data");
                return;
            }

            await response.WriteAsync(token);
            //await _next(httpContext);
        }

        private string CreateJwtToken(
            string username, string clientId)
        {
            //ClaimsPrincipal principal;
            //ClaimsIdentity identity;

            var secret = _configuration["JWT:Secret"];
            var blobSecret = Encoding.UTF8.GetBytes(secret);
            var encryptedSecret = new SymmetricSecurityKey(blobSecret);
            var signingCredentials = new SigningCredentials(encryptedSecret, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;

            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, username));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixTimeSeconds(now).ToString(), ClaimValueTypes.Integer64));

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: new List<Claim>(),
                notBefore: now,
                expires: now.AddMinutes(10),   // 10 ==> duration
                signingCredentials: signingCredentials
            );

            var serializedToken = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToken);

            return serializedToken;
        }

        private async Task<string> CreateTokenFromRefreshToken(HttpContext httpContext)
        {
            var request = httpContext.Request;
            var response = httpContext.Response;
            var refreshTokenIdString = request.Form["refresh_token"];
            if (string.IsNullOrEmpty(refreshTokenIdString))
            {
                var msg = $"The refresh token is invalid (1)";
                _logger.LogWarning(msg);
                response.StatusCode = 400;
                await response.WriteAsync(msg);
                return string.Empty;
            }

            if (!Guid.TryParse(refreshTokenIdString, out Guid refreshTokenId))
            {
                var msg = $"The refresh token is invalid (2)";
                _logger.LogWarning(msg);
                response.StatusCode = 400;
                await response.WriteAsync(msg);
                return string.Empty;
            }

            var refreshToken = await LoadRefreshToken(refreshTokenId);
            if (refreshToken == null)
            {
                // refresh token not found,
                // we must go on by redirecting to the full auth
                return null;
            }

            _logger.LogInformation($"The refresh token id: {refreshToken.Id} user: {refreshToken.Username} has been validated");

            // TODO
            // deserialize the token using refreshToken.Identity
            // and obtain a ClaimsPrincipal

            // TODO:
            // serialize the ClaimsPrincipal in identity
            string identity = null;

            var newRefreshToken = RefreshToken.Create(
                refreshToken.Username,
                refreshToken.ClientId,
                _options.TokenDuration,
                refreshToken.InitialGrantType,
                refreshToken.IdentityProviderId,
                identity,
                null);

            await SaveRefreshToken(newRefreshToken);
            var fullToken = CreateJwtToken(refreshToken.Username, refreshToken.ClientId);
            var token = CreateJwt(fullToken, newRefreshToken.Id, _options.TokenDuration.TotalSeconds);
            return token;
        }

        private async Task<RefreshToken> LoadRefreshToken(Guid id)
        {
            try
            {
                // 1. retrieve refresh token from persistence
                // 2. delete the old refresh token from persistence
                await Task.Delay(0);
                return null;
            }
            catch (Exception err)
            {
                _logger.LogWarning($"Error in LoadRefreshToken: {err.ToString()}");
                return null;
            }
        }

        private string CreateJwt(string encodedToken, string refreshToken, double expiration)
        {
            var content = new
            {
                access_token = encodedToken,
                refresh_token = refreshToken,
                expires_in = expiration,
            };

            return JsonConvert.SerializeObject(content);
        }

        private Task SaveRefreshToken(RefreshToken refreshToken)
        {
            // TODO: persistence
            return Task.CompletedTask;
        }

        /// <summary>
        /// Seconds elapsed since 1/1/1970 00:00 UTC
        /// </summary>
        private long ToUnixTimeSeconds(DateTime date)
        {
            return new DateTimeOffset(date)
                .ToUniversalTime()
                .ToUnixTimeSeconds();
        }
    }
}
