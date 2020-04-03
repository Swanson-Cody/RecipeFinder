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
                Instruction = "1) Preheat Oven 350°F.\r\n2) Add all ingredients into a bowl and mix together.\r\n3) Put ingredients into a 9x9 pan and cover with aluminum foil.\r\n4) Bake for 35-40 minutes."
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
                Instruction = "1) Preheat deep fryer to 375°F.\r\n2) Mix flour, cayenne pepper, black pepper, and salt together in a bowl.\r\n3) Coat the chicken wings with mixture and fry it until golden. Set them aside for now.\r\n4) Add butter and hot wing sauce into a small pot.\r\n5) Heat the small pot using medium heat on the stove. Stir until it starts to bubble.\r\n6) Coat the chicken wings with sauce."
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

            recipe = new Recipe
            {
                UserRecordNumber = userId,
                Title = "Mash Potato with Chicken Bowl",
                DateAdded = DateTime.Parse("2020-03-02"),
                Instruction = "1) Boil potatoes until they are soft.\r\n2) Mash together in a mixing bowl the softened potatoes, butter, milk, salt, and pepper.\r\n3) Cook popcorn chicken in deep fryer at 370°F or air fryer for 400°F.\r\n4) Add cheese, corn, and popcorn chicken on top of the mashed potatoes in order listed."
            };

            _context.Recipe.Add(recipe);
            _context.SaveChanges();

            _context.Ingredient.AddRange(
                    new Ingredient()
                    {
                        Name = "Potatoes",
                        Quantity = 2,
                        RecipeId = recipe.ID,
                        Notes = "Medium sized."
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
                        Name = "Milk",
                        Measurement = Measurements.Cup.ToString(),
                        Quantity = .25,
                        RecipeId = recipe.ID
                    },
                    new Ingredient()
                    {
                        Name = "Salt",
                        RecipeId = recipe.ID,
                        Notes = "Add enough to satisfy taste preference."
                    },
                    new Ingredient()
                    {
                        Name = "Black Pepper",
                        RecipeId = recipe.ID,
                        Notes = "Add enough to satisfy taste preference."
                    },
                    new Ingredient()
                    {
                        Name = "Cheese",
                        Measurement = Measurements.Cup.ToString(),
                        Quantity = .25,
                        RecipeId = recipe.ID
                    },
                    new Ingredient()
                    {
                        Name = "Corn",
                        Measurement = Measurements.Cup.ToString(),
                        Quantity = .25,
                        RecipeId = recipe.ID
                    },
                    new Ingredient()
                    {
                        Name = "Popcorn Chicken",
                        Measurement = Measurements.Cup.ToString(),
                        Quantity = 1,
                        RecipeId = recipe.ID
                    }
            );

            _context.SaveChanges();

            recipe = new Recipe
            {
                UserRecordNumber = userId,
                Title = "Parmesan Chicken",
                DateAdded = DateTime.Parse("2020-03-02"),
                Instruction = "1) Preheat oven to 350°F.\r\n2) Mix butter, parsley, salt, and black pepper together in a bowl.\r\n3) Coat chicken with mixture from step 2 and then coat with parmesan cheese.\r\n4) Put chicken in baking pan, cover with aluminum foil. Bake for 15-20 minutes.\r\n5) Remove aluminum foil. Bake until golden."
            };

            _context.Recipe.Add(recipe);
            _context.SaveChanges();

            _context.Ingredient.AddRange(
                   new Ingredient()
                   {
                       Name = "Chicken Breast",
                       Quantity = 2,
                       RecipeId = recipe.ID
                   },
                   new Ingredient()
                   {
                       Name = "Parmesan Cheese",
                       Measurement = Measurements.Cup.ToString(),
                       Quantity = .50,
                       RecipeId = recipe.ID,
                       Notes = "Grated"
                   },
                   new Ingredient()
                   {
                       Name = "Salt",
                       Measurement = Measurements.Dash.ToString(),
                       RecipeId = recipe.ID
                   },
                   new Ingredient()
                   {
                       Name = "Black Pepper",
                       Measurement = Measurements.Dash.ToString(),
                       RecipeId = recipe.ID,
                   },
                   new Ingredient()
                   {
                       Name = "Parsley",
                       Measurement = Measurements.Teaspoon.ToString(),
                       Quantity = 1,
                       RecipeId = recipe.ID,
                       Notes = "Flakes"
                   },
                   new Ingredient()
                   {
                       Name = "Butter",
                       Measurement = Measurements.Stick.ToString(),
                       Quantity = .66,
                       RecipeId = recipe.ID,
                       Notes = "Softened"
                   }
           );

            _context.SaveChanges();

            recipe = new Recipe
            {
                UserRecordNumber = userId,
                Title = "Taco Salad",
                DateAdded = DateTime.Parse("2020-03-02"),
                Instruction = "1) Cook ground beef over medium-high heat.\r\n2) Turn off heat and add taco seasoning.\r\n3) Mix all the ingredients in a bowl in the quantity desired."
            };

            _context.Recipe.Add(recipe);
            _context.SaveChanges();

            _context.Ingredient.AddRange(
                   new Ingredient()
                   {
                       Name = "Lettuce",
                       RecipeId = recipe.ID,
                       Notes = "Use amount desired."
                   },
                   new Ingredient()
                   {
                       Name = "Tomatoes",
                       RecipeId = recipe.ID,
                       Notes = "Use amount desired."
                   },
                   new Ingredient()
                   {
                       Name = "Black Beans",
                       RecipeId = recipe.ID,
                       Notes = "Use amount desired."
                   },
                   new Ingredient()
                   {
                       Name = "Corn",
                       RecipeId = recipe.ID,
                       Notes = "Use amount desired."
                   },
                   new Ingredient()
                   {
                       Name = "Avocado",
                       RecipeId = recipe.ID,
                       Notes = "Use amount desired."
                   },
                   new Ingredient()
                   {
                       Name = "Cheese",
                       RecipeId = recipe.ID,
                       Notes = "Cut into small cubes and use amount desired."
                   }, new Ingredient()
                   {
                       Name = "Doritos",
                       RecipeId = recipe.ID,
                       Notes = "Use amount desired."
                   },
                new Ingredient()
                {
                    Name = "Thousand Island Dressing",
                    RecipeId = recipe.ID,
                    Notes = "Use amount desired."
                },
                new Ingredient()
                {
                    Name = "Ground Beef",
                    Measurement = Measurements.Pound.ToString(),
                    Quantity = 1,
                    RecipeId = recipe.ID
                },
                new Ingredient()
                {
                    Name = "Taco Seasoning",
                    Measurement = Measurements.Tablespoon.ToString(),
                    Quantity = 3,
                    RecipeId = recipe.ID
                }
            );

            _context.SaveChanges();

            recipe = new Recipe
            {
                UserRecordNumber = userId,
                Title = "Baked Ham Sandwich",
                DateAdded = DateTime.Parse("2020-03-02"),
                Instruction = "1) Preheat oven to 350°F\r\n2) Cut Hawaiian rolls in half. (Hamburger bun style)\r\n3) Add mayo on both of the inside parts of the rolls.\r\n4) Fold ham and cheese to fit in each roll and place them in the rolls.\r\n5) Bake for 12-15 minutes."
            };

            _context.Recipe.Add(recipe);
            _context.SaveChanges();

            _context.Ingredient.AddRange(
                new Ingredient()
                {
                    Name = "Hawaiian Rolls",
                    Quantity = 1,
                    RecipeId = recipe.ID,
                    Notes = "Use amount desired for number of sandwiches."
                },
                new Ingredient()
                {
                    Name = "Ham",
                    RecipeId = recipe.ID,
                    Notes = "Sliced. Use amount desired."
                },
                new Ingredient()
                {
                    Name = "Mayo",
                    RecipeId = recipe.ID,
                    Notes = "Use amount desired."
                },
                new Ingredient()
                {
                    Name = "Cheese",
                    RecipeId = recipe.ID,
                    Notes = "Use amount desired."
                }
            );

            _context.SaveChanges();

            recipe = new Recipe
            {
                UserRecordNumber = userId,
                Title = "Breakfast Sandwich",
                DateAdded = DateTime.Parse("2020-03-02"),
                Instruction = "1) Put mayo on burger buns.\r\n2) Cut bacon in half and cook them on a frying pan using medium-heat on the stove. Set aside when cooked.\r\n3) Cook egg on a frying pan using medium heat on the stove. (Cook to preference)\r\n4) Assemble ingredients together within burger bun."
            };

            _context.Recipe.Add(recipe);
            _context.SaveChanges();

            _context.Ingredient.AddRange(
                new Ingredient()
                {
                    Name = "Burger Buns",
                    Quantity = 1,
                    RecipeId = recipe.ID
                },
                new Ingredient()
                {
                    Name = "egg",
                    Quantity = 1,
                    RecipeId = recipe.ID
                },
                new Ingredient()
                {
                    Name = "Cheese",
                    Measurement = Measurements.Slice.ToString(),
                    Quantity = 1,
                    RecipeId = recipe.ID,
                    Notes = "Cheese lovers should use 2 slices. Use cheese of choice."
                },
                new Ingredient()
                {
                    Name = "Bacon",
                    Measurement = Measurements.Slice.ToString(),
                    Quantity = 2,
                    RecipeId = recipe.ID,
                    Notes = "Bacon lovers should use as much bacon as desired."
                },
                new Ingredient()
                {
                    Name = "Mayo",
                    Measurement = Measurements.Tablespoon.ToString(),
                    Quantity = 1,
                    RecipeId = recipe.ID
                }
            );

            _context.SaveChanges();

            recipe = new Recipe
            {
                UserRecordNumber = userId,
                Title = "Noodle Stir Fry",
                DateAdded = DateTime.Parse("2020-03-02"),
                Instruction = "1) Heat the vegetable oil in a large skillet over medium-high heat. Add chicken, stir until cooked.\r\n2) Add carrot, celery, and broccoli. Cook it for 3 minutes.\r\n3) Add noodles into the pan mix it well.\r\n4) Add oyster sauce and sugar. Add salt to desired level of saltiness through taste tests."
            };

            _context.Recipe.Add(recipe);
            _context.SaveChanges();

            _context.Ingredient.AddRange(
                new Ingredient()
                {
                    Name = "Noodles",
                    Measurement = Measurements.Cup.ToString(),
                    Quantity = 2,
                    RecipeId = recipe.ID,
                    Notes = "Have the noodles already boiled/cooked for this recipe."
                },
                new Ingredient()
                {
                    Name = "Carrot",
                    Measurement = Measurements.Cup.ToString(),
                    Quantity = .5,
                    RecipeId = recipe.ID,
                    Notes = "Sliced and diced."
                },
                new Ingredient()
                {
                    Name = "Celery",
                    Measurement = Measurements.Cup.ToString(),
                    Quantity = .5,
                    RecipeId = recipe.ID,
                    Notes = "Sliced and diced."
                },
                new Ingredient()
                {
                    Name = "Broccoli",
                    Measurement = Measurements.Cup.ToString(),
                    Quantity = .5,
                    RecipeId = recipe.ID,
                    Notes = "Sliced and diced."
                },
                new Ingredient()
                {
                    Name = "Chicken",
                    Measurement = Measurements.Cup.ToString(),
                    Quantity = .5,
                    RecipeId = recipe.ID,
                    Notes = "Chopped into bite-sized pieces."
                },
                new Ingredient()
                {
                    Name = "Vegetable Oil",
                    Measurement = Measurements.Tablespoon.ToString(),
                    Quantity = 1,
                    RecipeId = recipe.ID,
                    Notes = "Can use whatever preferred oil you want."
                },
                new Ingredient()
                {
                    Name = "Oyster Sauce",
                    Measurement = Measurements.Tablespoon.ToString(),
                    Quantity = 2,
                    RecipeId = recipe.ID,
                    Notes = "Can use less or more as guided by taste preference."
                },
                new Ingredient()
                {
                    Name = "Sugar",
                    Measurement = Measurements.Tablespoon.ToString(),
                    Quantity = .5,
                    RecipeId = recipe.ID,
                    Notes = "Can use less or more as guided by taste preference."
                },
                new Ingredient()
                {
                    Name = "Salt",
                    RecipeId = recipe.ID,
                    Notes = "Salt to taste preference."
                }
            );

            _context.SaveChanges();

            recipe = new Recipe
            {
                UserRecordNumber = userId,
                Title = "Barbeque Baked Ribs",
                DateAdded = DateTime.Parse("2020-03-02"),
                Instruction = "1) Preheat oven to 275°F\r\n2) In mixing bowl, mix bbq sauce, maple syrup, and salt together.\r\n3) Put ribs on baking tray. Brush the mixture from step 2 onto both sides of the ribs.\r\n4) Cover with aluminum foil. Bake it for 1 hour and 20 minutes.\r\n5) Take out from oven. Remove aluminum foil. Flip the ribs. Cover it with aluminum foil again. Bake for another 1 hour and 20 minutes.\r\n6) Take out from oven. Remove the aluminum foil.\r\n7) Preheat oven to 425°F. Put the ribs back in the oven and bake for another 10-15 minutes."
            };

            _context.Recipe.Add(recipe);
            _context.SaveChanges();

            _context.Ingredient.AddRange(
                new Ingredient()
                {
                    Name = "Ribs",
                    Quantity = 1,
                    RecipeId = recipe.ID,
                    Notes = "Use 1 rack."
                },
                new Ingredient()
                {
                    Name = "BBQ Sauce",
                    Quantity = .5,
                    RecipeId = recipe.ID,
                    Notes = "Use half a bottle of your preferred bbq sauce."
                },
                new Ingredient()
                {
                    Name = "Maple Syrup",
                    Measurement = Measurements.Tablespoon.ToString(),
                    Quantity = 2,
                    RecipeId = recipe.ID
                },
                new Ingredient()
                {
                    Name = "Salt",
                    Measurement = Measurements.Dash.ToString(),
                    RecipeId = recipe.ID
                }
            );

            _context.SaveChanges();
        }
    }
}

