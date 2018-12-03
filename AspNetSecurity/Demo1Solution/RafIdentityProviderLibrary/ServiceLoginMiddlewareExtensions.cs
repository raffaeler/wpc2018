using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace RafIdentityProviderLibrary
{
    public static class ServiceLoginMiddlewareExtensions
    {
        public static IApplicationBuilder UseServiceLoginMiddleware(this IApplicationBuilder app, ServiceLoginOptions options)
        {
            return app.UseMiddleware<ServiceLoginMiddleware>(Options.Create(options));
        }
    }
}
