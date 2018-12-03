using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace AuthorizationLibrary
{
    public class ClaimTransformer : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            InjectClaims(principal.Identity as ClaimsIdentity);
            return Task.FromResult(principal);
        }

        private void InjectClaims(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null || !claimsIdentity.IsAuthenticated)
            {
                return;
            }

            switch (claimsIdentity.Name)
            {
                case "adult@email.com":
                    claimsIdentity.AddClaim(new Claim("Article", "LCRUD"));
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.DateOfBirth,
                        new DateTime(1967, 2, 26).ToString("s", System.Globalization.CultureInfo.InvariantCulture)));
                    break;

                case "buyer@email.com":
                    claimsIdentity.AddClaim(new Claim("Article", "LCR"));
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.DateOfBirth,
                        new DateTime(1967, 2, 26).ToString("s", System.Globalization.CultureInfo.InvariantCulture)));
                    break;

                case "child@email.com":
                    claimsIdentity.AddClaim(new Claim("Article", "L"));
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.DateOfBirth,
                        new DateTime(2010, 2, 26).ToString("s", System.Globalization.CultureInfo.InvariantCulture)));
                    break;

                case "boy@email.com":
                    claimsIdentity.AddClaim(new Claim("Article", "LR"));
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.DateOfBirth,
                        new DateTime(2005, 2, 26).ToString("s", System.Globalization.CultureInfo.InvariantCulture)));
                    break;
            }
        }

    }
}
