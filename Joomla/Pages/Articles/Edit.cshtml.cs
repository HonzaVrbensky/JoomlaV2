using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Joomla.Data;
using Joomla.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Build.Framework;

namespace Joomla.Pages.Articles;

[Authorize()]
public class EditModel : PageModel
{
    private readonly Joomla.Data.ApplicationDbContext _context;

    public EditModel(Joomla.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public class InputModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var article =  await _context.Articles.FirstOrDefaultAsync(m => m.Id == id);
        if (article == null)
        {
            return NotFound();
        }

        Input = new InputModel
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content
        };

        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var article = await _context.Articles.FirstOrDefaultAsync(m => m.Id == Input.Id);
        if (article == null)
        {
            return NotFound();
        }

        article.Title = Input.Title;
        article.Content = Input.Content;
        article.UpdatedAt = DateTime.Now;

        _context.Attach(article).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ArticleExists(article.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToPage("./Index");
    }

    private bool ArticleExists(int id)
    {
        return _context.Articles.Any(e => e.Id == id);
    }
}
