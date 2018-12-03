using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ModelLibrary;

namespace AuthorizationLibrary
{
    public class BuyerHandler : AuthorizationHandler<AvailableRequirement, Article>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            AvailableRequirement requirement, Article resource)
        {
            if (!context.User.Identity.IsAuthenticated) return Task.CompletedTask;

            if (resource.State == ArticleState.Returned ||
                resource.State == ArticleState.Sold)
            {
                var isBuyer = resource.Buyer == context.User.Identity.Name;
                if (!isBuyer) return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
