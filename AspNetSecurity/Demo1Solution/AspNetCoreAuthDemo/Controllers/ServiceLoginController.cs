using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RafIdentityProviderLibrary;

namespace AspNetCoreAuthDemo.Controllers
{
    [Route("auth")]
    [ApiController]
    public class ServiceLoginController : Controller
    {
        public ServiceLoginController(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]Credentials credentials)
        {
            if (credentials == null ||
                string.IsNullOrEmpty(credentials.Username) ||
                string.IsNullOrEmpty(credentials.Password))
            {
                return BadRequest("Username and password must be supplied");
            }


            if (credentials.Username == "raffaeler@vevy.com" && credentials.Password == "P@ssw0rd")
            {
                var secret = Configuration["JWT:Secret"];
                var blobSecret = Encoding.UTF8.GetBytes(secret);
                var encryptedSecret = new SymmetricSecurityKey(blobSecret);
                var signingCredentials = new SigningCredentials(encryptedSecret, SecurityAlgorithms.HmacSha256);

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: signingCredentials
                );

                var serializedToken = new JwtSecurityTokenHandler()
                    .WriteToken(jwtSecurityToken);

                return Ok(new { Token = serializedToken });
            }

            return Unauthorized();
        }
    }
}
