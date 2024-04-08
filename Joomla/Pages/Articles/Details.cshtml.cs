using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Joomla.Data;
using Joomla.Models;
using Microsoft.AspNetCore.Authorization;

namespace Joomla.Pages.Articles;

[Authorize()]
public class DetailsModel : PageModel
{
    private readonly Joomla.Data.ApplicationDbContext _context;

    public DetailsModel(Joomla.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    public Article Article { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var article = await _context.Articles.FirstOrDefaultAsync(m => m.Id == id);
        if (article == null)
        {
            return NotFound();
        }
        else
        {
            Article = article;
        }
        return Page();
    }
}
