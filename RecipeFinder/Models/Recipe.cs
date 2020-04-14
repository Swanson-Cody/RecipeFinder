using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace RecipeFinder.Models
{
    public class Recipe
    {
        public int ID { get; set; }
        
        [Required]
        public string Title { get; set; }

        [Display(Name = "Date of Entry")]
        [DataType(DataType.Date)]
        public DateTime DateAdded { get; set; }

        public List<Ingredient> Ingredients { get; set; }
        
        public string Instruction { get; set; }

        public string UserRecordNumber { get; set; }
    }
}
