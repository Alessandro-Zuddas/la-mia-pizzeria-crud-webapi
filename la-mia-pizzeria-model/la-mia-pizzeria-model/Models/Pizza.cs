using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_model.Models
{
    public class Pizza
    {
        public Pizza() { }

        public Pizza(string _imgSrc, string _name, string _description, string _price) 
        {
            ImgSrc = _imgSrc;
            Name = _name;
            Description = _description;
            Price = _price;
        }

        public int Id { get; set; }
        public string? ImgSrc { get; set; }

        [Required(ErrorMessage = "Nome obbligatorio!")]
        [StringLength(50, ErrorMessage = "Il nome non può essere più lungo di 50 caratteri!")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Descrizione obbligatoria!")]
        [StringLength(150, ErrorMessage = "La descrizione non può essere più lunga di 150 caratteri!")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Prezzo obbligatorio!")]
        public string Price { get; set; } = string.Empty;

        [Required(ErrorMessage = "Categoria obbligatoria!")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<Ingredient>? Ingredients { get; set;}
    }
}
