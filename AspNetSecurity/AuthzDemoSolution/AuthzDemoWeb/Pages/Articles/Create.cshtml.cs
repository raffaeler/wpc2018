using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AuthzDemoWeb.Data;
using ModelLibrary;
using Microsoft.AspNetCore.Authorization;
using AuthorizationLibrary;
using Microsoft.Extensions.Logging;

namespace AuthzDemoWeb.Pages.Articles
{
    public class CreateModel : PageModel
    {
        private readonly AuthzDemoWeb.Data.ApplicationDbContext _context;
        private readonly IAuthorizationService _auth;
        private readonly ILogger _logger;

        public CreateModel(IAuthorizationService authorizationService,
            AuthzDemoWeb.Data.ApplicationDbContext context,
            ILoggerFactory loggerFactory)
        {
            _auth = authorizationService;
            _context = context;
            _logger = loggerFactory.CreateLogger<CreateModel>();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Article Article { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var authResult = await _auth.AuthorizeAsync(User, Article, ArticlePolicies.CreateArticles);
            if (!authResult.Succeeded)
            {
                ArticlesHelper.LogFailure(_logger, Article, authResult.Failure);
                return new ChallengeResult();
            }

            _context.Articles.Add(Article);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}