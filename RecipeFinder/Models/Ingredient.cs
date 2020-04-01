using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RecipeFinder.Models
{
    public class Ingredient
    {
        public int ID { get; set; }

        public int RecipeId { get; set; }

        [Display(Name = "Ingredient")]
        public string Name { get; set; }

        public double? Quantity { get; set; }

        public string Measurement { get; set; }

        public string Notes { get; set; }
    }
}
