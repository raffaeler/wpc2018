using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using ModelLibrary;

namespace AuthorizationLibrary
{
    public class AgeRequirement : IAuthorizationRequirement
    {
        public AgeRequirement()
        {
        }

    }
}
