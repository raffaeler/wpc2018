using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ModelLibrary;

namespace AuthorizationLibrary
{
    public class OwnerHandler : AuthorizationHandler<LcrudRequirement, Article>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LcrudRequirement requirement, Article resource)
        {
            var isOwner = resource.Owner == context.User.Identity.Name;
            if (isOwner) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
