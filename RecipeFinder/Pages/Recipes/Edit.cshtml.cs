using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeFinder.Data;
using RecipeFinder.Models;

namespace RecipeFinder
{
    [Authorize]
    public class RecipeEditModel : PageModel
    {
        private readonly RecipeFinder.Data.ApplicationDbContext _context;

        public RecipeEditModel(RecipeFinder.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Recipe Recipe { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Recipe = await _context.Recipe.FirstOrDefaultAsync(m => m.ID == id);

            if (Recipe == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Recipe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(Recipe.ID))
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

        private bool RecipeExists(int id)
        {
            return _context.Recipe.Any(e => e.ID == id);
        }
        //new Recipe
        //{
        //    Title = "Mash Potato with Chicken Bowl",
        //    DateAdded = DateTime.Parse("2020-03-02"),
        //    IngredientsWithAmount = "potatoes - 2 medium sized, butter - 1/4 cup, milk - 1/4 cup, salt - taste preference, pepper - taste preference, cheese - 1/4 cup, corn - 1/4 cup, popcorn chicken - 1 cup",
        //    Instruction = "1) Boil potatoes until they are soft. 2) Mash together in a mixing bowl the softened potatoes, butter, milk, salt, and pepper. 3) Cook popcorn chicken in deep fryer at 370°F or air fryer for 400°F. 4) Add cheese, corn, and popcorn chicken on top of the mashed potatoes in order listed."
        //},

        //new Recipe
        //{
        //    Title = "Parmesan Chicken",
        //    DateAdded = DateTime.Parse("2020-03-02"),
        //    IngredientsWithAmount = "chicken breast - 2 pieces, parmesan cheese (grated) - 1/2 cup, salt - dash, black pepper - dash, parsley (flakes) - 1 teaspoon, butter (softened) - 2/3 stick",
        //    Instruction = "1) Preheat oven to 350°F. 2) Mix butter, parsley, salt, and black pepper together in a bowl. 3) Coat chicken with mixture from step 2 and then coat with parmesan cheese. 4) Put chicken in baking pan, cover with aluminum foil. Bake for 15-20 minutes. 5) Remove aluminum foil. Bake until golden."
        //}
    }
}
