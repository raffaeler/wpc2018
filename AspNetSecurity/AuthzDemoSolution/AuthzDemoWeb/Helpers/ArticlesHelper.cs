using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ModelLibrary;

namespace AuthzDemoWeb
{
    public static class ArticlesHelper
    {
        public static void LogFailure(ILogger logger, IEnumerable<FailureDescriptor> denied)
        {
            foreach (var d in denied)
            {
                LogFailure(logger, d);
            }
        }

        public static void LogFailure(ILogger logger, FailureDescriptor denied)
        {
            var failures = string.Join(", ", denied.Failure.FailedRequirements.Select(f => f.ToString()));
            logger.LogWarning($"Article {denied.Article.Name} is denied because {failures}");
        }

        public static void LogFailure(ILogger logger, Article article, AuthorizationFailure failure)
        {
            var failures = string.Join(", ", failure.FailedRequirements.Select(f => f.GetType().Name));
            logger.LogWarning($"Article {article.Name} is denied because {failures}");
        }
    }
}
