using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AuthorizationLibrary
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterHandlers(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, BuyerHandler>();
            services.AddSingleton<IAuthorizationHandler, EditingHandler>();
            services.AddSingleton<IAuthorizationHandler, AgeHandler>();
            services.AddSingleton<IAuthorizationHandler, OwnerHandler>();

            return services;
        }


        public static void RegisterPolicies(this AuthorizationOptions authorizationOptions)
        {
            authorizationOptions.AddPolicy(ArticlePolicies.ListArticles, p =>
                p.AddRequirements(LcrudRequirement.Create(true), new AgeRequirement(), new AvailableRequirement()));

            authorizationOptions.AddPolicy(ArticlePolicies.CreateArticles, p =>
                p.AddRequirements(LcrudRequirement.Create(true, true), new AgeRequirement()));

            authorizationOptions.AddPolicy(ArticlePolicies.ReadArticles, p =>
                p.AddRequirements(LcrudRequirement.Create(true, false, true), new AgeRequirement(), new AvailableRequirement()));

            authorizationOptions.AddPolicy(ArticlePolicies.UpdateArticles, p =>
                p.AddRequirements(LcrudRequirement.Create(true, false, false, true), new AgeRequirement(), new AvailableRequirement()));

            authorizationOptions.AddPolicy(ArticlePolicies.DeleteArticles, p =>
                p.AddRequirements(LcrudRequirement.Create(true, false, false, true, true), new AgeRequirement(), new AvailableRequirement()));
        }

    }
}
