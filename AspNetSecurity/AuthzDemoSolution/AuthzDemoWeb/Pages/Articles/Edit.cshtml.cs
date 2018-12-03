using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuthzDemoWeb.Data;
using ModelLibrary;
using AuthorizationLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AuthzDemoWeb.Pages.Articles
{
    public class EditModel : PageModel
    {
        private readonly AuthzDemoWeb.Data.ApplicationDbContext _context;
        private readonly IAuthorizationService _auth;
        private readonly ILogger _logger;

        public EditModel(IAuthorizationService authorizationService,
            AuthzDemoWeb.Data.ApplicationDbContext context,
            ILoggerFactory loggerFactory)
        {
            _auth = authorizationService;
            _context = context;
            _logger = loggerFactory.CreateLogger<EditModel>();
        }

        [BindProperty]
        public Article Article { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Article = await _context.Articles.FirstOrDefaultAsync(m => m.Id == id);

            if (Article == null)
            {
                return NotFound();
            }

            var authResult = await _auth.AuthorizeAsync(User, Article, ArticlePolicies.ReadArticles);
            if (!authResult.Succeeded)
            {
                ArticlesHelper.LogFailure(_logger, Article, authResult.Failure);
                return new ChallengeResult();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var authResult = await _auth.AuthorizeAsync(User, Article, ArticlePolicies.UpdateArticles);
            if (!authResult.Succeeded)
            {
                ArticlesHelper.LogFailure(_logger, Article, authResult.Failure);
                return new ChallengeResult();
            }

            _context.Attach(Article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(Article.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ArticleExists(Guid id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}
