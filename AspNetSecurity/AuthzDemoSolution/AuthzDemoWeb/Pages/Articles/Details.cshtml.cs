using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AuthzDemoWeb.Data;
using ModelLibrary;
using Microsoft.AspNetCore.Authorization;
using AuthorizationLibrary;
using Microsoft.Extensions.Logging;

namespace AuthzDemoWeb.Pages.Articles
{
    public class DetailsModel : PageModel
    {
        private readonly AuthzDemoWeb.Data.ApplicationDbContext _context;
        private readonly IAuthorizationService _auth;
        private readonly ILogger _logger;

        public DetailsModel(IAuthorizationService authorizationService,
            AuthzDemoWeb.Data.ApplicationDbContext context,
            ILoggerFactory loggerFactory)
        {
            _auth = authorizationService;
            _context = context;
            _logger = loggerFactory.CreateLogger<DetailsModel>();
        }

        public Article Article { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Article = await _context.Articles.FirstOrDefaultAsync(m => m.Id == id);

            var authResult = await _auth.AuthorizeAsync(User, Article, ArticlePolicies.ReadArticles);
            if (!authResult.Succeeded)
            {
                ArticlesHelper.LogFailure(_logger, Article, authResult.Failure);
                return new ChallengeResult();
            }

            if (Article == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
