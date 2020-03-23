using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace RecipeFinder.Models
{
    public class Recipe : DbContext
    {
        public int ID { get; set; }
        public string Title { get; set; }

        [Display(Name = "Date of Entry")]
        [DataType(DataType.Date)]
        public DateTime DateAdded { get; set; }

        public List<Ingredient> Ingredients { get; set; }
        public string Instruction { get; set; }

        public string UserId { get; set; }
    }
}
