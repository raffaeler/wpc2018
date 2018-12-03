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
    public class AgeHandler : AuthorizationHandler<AgeRequirement, Article>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            AgeRequirement requirement, Article resource)
        {
            var userMaturity = GetMaturity(context.User);

            if (userMaturity >= resource.Maturity)
            {
                context.Succeed(requirement);
            }

            // not allowed because it requires
            // more seniority
            return Task.CompletedTask;
        }

        private Maturity GetMaturity(ClaimsPrincipal user)
        {
            var birth = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.DateOfBirth);
            if (birth == null)
            {
                return Maturity.Unclassified;
            }

            // obtained with 
            // datetime.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
            var birthDate = DateTimeOffset.Parse(birth.Value);
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;

            // https://ieeexplore.ieee.org/document/6416855/
            if (age < 13) return Maturity.Child;
            if (age < 19) return Maturity.Adolescent;
            if (age < 60) return Maturity.Adult;
            return Maturity.Senior;
        }
    }
}
