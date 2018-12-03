using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ModelLibrary;

namespace AuthorizationLibrary
{
    public class EditingHandler :
        AuthorizationHandler<LcrudRequirement, Article>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            LcrudRequirement requirement,
            Article resource)
        {
            var claims = context.User.Claims
                .Where(c => c.Type.StartsWith("Article"))
                .ToList();

            // claim.Value contains zero or more "LCRUD" chars
            var claim = claims.FirstOrDefault();

            if (claim != null &&
                //resource.State == ArticleState.ListedForSelling &&
                requirement.SplitMatchAny(claim.Value))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
