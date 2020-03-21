using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeFinder.Data;
using RecipeFinder.Models;

namespace RecipeFinder
{
    public class IngredientsIndexModel : PageModel
    {
        private readonly RecipeFinder.Data.ApplicationDbContext _context;

        public IngredientsIndexModel(RecipeFinder.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Ingredient> Ingredient { get;set; }

        public async Task OnGetAsync()
        {
            Ingredient = await _context.Ingredient.ToListAsync();
        }
    }
}
