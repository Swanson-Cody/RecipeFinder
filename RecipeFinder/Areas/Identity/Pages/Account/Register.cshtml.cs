using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using RecipeFinder.Data;
using RecipeFinder.Models;

namespace RecipeFinder.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //seed account with default recipes
                    SeedUserRecipe(user.Id);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private void SeedUserRecipe(string userId)
        {
            var recipe = new Recipe
            {
                UserRecordNumber = userId,
                Title = "Vegetable Medley",
                DateAdded = DateTime.Parse("2020-03-02"),
                Instruction = "1) Preheat Oven 350°F. 2) Add all ingredients into a bowl and mix together. 3) Put ingredients into a 9x9 pan and cover with aluminum foil. 4) Bake for 35-40 minutes."
            };

            _context.Recipe.Add(recipe);
            _context.SaveChanges();

            _context.Ingredient.AddRange(
                    new Ingredient()
                    {
                        Name = "Zucchini",
                        Measurement = Measurements.Cup.ToString(),
                        Quantity = .5,
                        RecipeId = recipe.ID,
                        Notes = "Chopped"
                    },
                    new Ingredient()
                    {
                        Name = "Bell Pepper",
                        Measurement = Measurements.Cup.ToString(),
                        Quantity = .25,
                        RecipeId = recipe.ID,
                        Notes = "Chopped"
                    },
                    new Ingredient()
                    {
                        Name = "Celery",
                        Measurement = Measurements.Cup.ToString(),
                        Quantity = .25,
                        RecipeId = recipe.ID,
                        Notes = "Chopped"
                    },
                    new Ingredient()
                    {
                        Name = "Broccoli",
                        Measurement = Measurements.Cup.ToString(),
                        Quantity = .25,
                        RecipeId = recipe.ID,
                        Notes = "Chopped"
                    },
                    new Ingredient()
                    {
                        Name = "Chicken Breast",
                        Quantity = 1,
                        RecipeId = recipe.ID,
                        Notes = "Cooked and chopped"
                    },
                    new Ingredient()
                    {
                        Name = "Cream of Mushroom",
                        Measurement = Measurements.Ounce.ToString(),
                        Quantity = 21,
                        RecipeId = recipe.ID
                    },
                    new Ingredient()
                    {
                        Name = "Black Pepper",
                        Measurement = Measurements.Teaspoon.ToString(),
                        Quantity = .25,
                        RecipeId = recipe.ID
                    }
                );

            _context.SaveChanges();

            recipe = new Recipe
            {
                UserRecordNumber = userId,
                Title = "Hot Wings",
                DateAdded = DateTime.Parse("2020-03-02"),
                Instruction = "1) Preheat deep fryer to 375°F. 2) Mix flour, cayenne pepper, black pepper, and salt together in a bowl. 3) Coat the chicken wings with mixture and fry it until golden. Set them aside for now. 4) Add butter and hot wing sauce into a small pot. 5) Heat the small pot using medium heat on the stove. Stir until it starts to bubble. 6) Coat the chicken wings with sauce."
            };

            _context.Recipe.Add(recipe);
            _context.SaveChanges();

            _context.Ingredient.AddRange(
                    new Ingredient()
                    {
                        Name = "Chicken Wings",
                        Measurement = Measurements.Pound.ToString(),
                        Quantity = 1,
                        RecipeId = recipe.ID
                    },
                    new Ingredient()
                    {
                        Name = "Flour",
                        Measurement = Measurements.Cup.ToString(),
                        Quantity = 1,
                        RecipeId = recipe.ID,
                        Notes = "All-purpose flour is best."
                    },
                    new Ingredient()
                    {
                        Name = "Cayenne Pepper",
                        Measurement = Measurements.Teaspoon.ToString(),
                        Quantity = .25,
                        RecipeId = recipe.ID
                    },
                    new Ingredient()
                    {
                        Name = "Salt",
                        Measurement = Measurements.Teaspoon.ToString(),
                        Quantity = .25,
                        RecipeId = recipe.ID
                    },
                    new Ingredient()
                    {
                        Name = "Black Pepper",
                        Measurement = Measurements.Teaspoon.ToString(),
                        Quantity = .25,
                        RecipeId = recipe.ID
                    },
                    new Ingredient()
                    {
                        Name = "Butter",
                        Measurement = Measurements.Cup.ToString(),
                        Quantity = .25,
                        RecipeId = recipe.ID
                    },
                    new Ingredient()
                    {
                        Name = "Hot Wing Sauce",
                        Measurement = Measurements.Cup.ToString(),
                        Quantity = 1,
                        RecipeId = recipe.ID,
                        Notes = "Use favorite hot wing sauce. Frank's Redhot Original Buffalo Wing Sauce is good."
                    }
                );

            _context.SaveChanges();
        }
    }
}
