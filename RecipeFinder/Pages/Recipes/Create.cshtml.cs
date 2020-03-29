﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecipeFinder.Data;
using RecipeFinder.Models;

namespace RecipeFinder
{
    [Authorize]
    public class RecipeCreateModel : PageModel
    {
        private readonly RecipeFinder.Data.ApplicationDbContext _context;


        public RecipeCreateModel(ApplicationDbContext context)
        {
            _context = context;
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

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Recipe.Add(Recipe);
            await _context.SaveChangesAsync();

            foreach (var ingredient in Ingredients)
            {
                _context.Ingredient.Add(ingredient);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public async Task OnPostAddIngredientAsync()
        {
            Ingredients.Add(new Ingredient());
        }
    }
}