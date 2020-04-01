using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeFinder.Models;

namespace RecipeFinder.Pages
{
    [Authorize]
    
    public class RecipeDisplayModel : PageModel
    {
        private readonly RecipeFinder.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RecipeDisplayModel(RecipeFinder.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Recipe Recipe { get; set; }
        public string UserId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Recipe = await _context.Recipe.FirstOrDefaultAsync(m => m.ID == id);
            Recipe.Ingredients = await _context.Ingredient.Where(x => x.RecipeId == Recipe.ID).ToListAsync();

            if (Recipe == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}