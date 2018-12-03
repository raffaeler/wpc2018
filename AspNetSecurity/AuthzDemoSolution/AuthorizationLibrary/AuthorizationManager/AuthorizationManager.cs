using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AuthorizationLibrary.AuthorizationManager
{
    /// <summary>
    /// ASP.NET Core service to verify the authorizations
    /// It must be registered in the DI a Singleton
    /// </summary>
    public class AuthorizationManager
    {
        private readonly IAuthorizationService _auth;
        private readonly ILogger _logger;

        public AuthorizationManager(
            IAuthorizationService authorizationService,
            ILoggerFactory loggerFactory)
        {
            _auth = authorizationService;
            _logger = loggerFactory.CreateLogger<AuthorizationManager>();
        }

        public async Task<AuthorizationResult> Authorize(
            ClaimsPrincipal claimsPrincipal, string policyName, object resource = null)
        {
            var result = await _auth.AuthorizeAsync(claimsPrincipal, resource, policyName);
            if (!result.Succeeded)
            {
                var messages = result.Failure.FailedRequirements
                    .Select(r => Format(r))
                    .ToList();

                var messageList = string.Join(", ", messages);
                _logger.LogWarning($"Authorization failed: {messageList}");
            }

            return result;
        }

        private static string Format(IAuthorizationRequirement requirement)
        {
            return requirement.GetType().Name;
        }
    }
}
