﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeFinder.Data;
using RecipeFinder.Models;

namespace RecipeFinder
{
    [Authorize]
    public class RecipeCreateModel : PageModel
    {
        private readonly RecipeFinder.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RecipeCreateModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            Ingredients = new List<Ingredient> { new Ingredient() };
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Recipe Recipe { get; set; }

        [BindProperty]
        public List<Ingredient> Ingredients { get; set; }

        public async Task OnPostAddIngredientAsync()
        {
            Ingredients.Add(new Ingredient());
        }

        public async Task OnPostRemoveIngredientAsync(int index)
        {
            Ingredients.RemoveAt(index);
            ModelState.Clear();
        }

        public async Task<IActionResult> OnPostSaveRecipeAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Recipe.UserRecordNumber = _userManager.GetUserId(User);
            _context.Recipe.Add(Recipe);
            await _context.SaveChangesAsync();

            foreach (var ingredient in Ingredients)
            {
                ingredient.RecipeId = Recipe.ID;
                _context.Ingredient.Add(ingredient);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}