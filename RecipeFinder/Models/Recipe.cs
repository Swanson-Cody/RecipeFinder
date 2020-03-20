using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RecipeFinder.Models
{
    public class Recipe
    {
        public int ID { get; set; }
        public string Title { get; set; }

        [Display(Name = "Date of Entry")]
        [DataType(DataType.Date)]
        public DateTime DateAdded { get; set; }

        [Display(Name = "Ingredients/Amount")]
        public string IngredientsWithAmount { get; set; }
        public string Instruction { get; set; }
    }
}
