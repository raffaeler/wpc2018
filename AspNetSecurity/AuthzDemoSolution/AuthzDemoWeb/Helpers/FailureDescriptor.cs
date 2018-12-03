using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ModelLibrary;

namespace AuthzDemoWeb
{
    public class FailureDescriptor
    {
        public Article Article { get; set; }
        public AuthorizationFailure Failure { get; set; }
    }
}
