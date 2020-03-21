namespace RecipeFinder.Models
{
    public class Ingredient
    {
        public int ID { get; set; }
        public int RecipeId { get; set; }
        public int Quantity { get; set; }
        public string Measurement { get; set; }
        public string Notes { get; set; }
    }
}
