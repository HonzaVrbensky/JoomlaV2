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
    public Article Article { get; set; } = default!;

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
        Article = article;
        ViewData["AuthorId"] = new SelectList(_context.JUsers, "Id", "Id");
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

    private bool ArticleExists(int id)
    {
        return _context.Articles.Any(e => e.Id == id);
    }
}
