using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Joomla.Data;
using Joomla.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace Joomla.Pages.Articles;

[Authorize()]
public class CreateModel : PageModel
{
    private readonly Joomla.Data.ApplicationDbContext _context;
    private readonly UserManager<JUser> _userManager;

    public CreateModel(Joomla.Data.ApplicationDbContext context, UserManager<JUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public class InputModel
    {
        [Required]
        public string Title { get; set; } = default!;

        [Required]
        public string Content { get; set; } = default!;
    }

    public IActionResult OnGet()
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var lastArticle = _context.Articles.OrderByDescending(a => a.Id).FirstOrDefault();
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        var article = new Article()
        {
            Id = lastArticle?.Id + 1 ?? 1,
            Title = Input.Title,
            Content = Input.Content,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Author = user,
        };

        _context.Articles.Add(article);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
