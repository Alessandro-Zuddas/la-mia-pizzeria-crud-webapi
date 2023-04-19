using la_mia_pizzeria_model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_model.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private readonly PizzaContext _context;

        public PizzaController(PizzaContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult GetPizzas([FromQuery] string? name)
        {
            var pizzas = _context.Pizzas
                //.Include(p => p.Category)
                //.Include(p => p.Ingredients)
                .Where(p => name == null || p.Name.ToLower().Contains(name.ToLower()))
                .ToList();

            return Ok(pizzas);
        }

        [HttpGet("{id}")]
        public IActionResult GetPizza(int id)
        {
            var pizza = _context.Pizzas.FirstOrDefault(p => p.Id == id);

            if (pizza == null)
            {
                return NotFound();
            }

            return Ok(pizza);
        }

        [HttpPost]
        public IActionResult CreatePizza(Pizza pizza)
        {
            _context.Pizzas.Add(pizza);
            _context.SaveChanges();

            return Ok(pizza);
        }

        [HttpPut("{id}")]
        public IActionResult PutPizza(int id, [FromBody] Pizza pizza)
        {
            var savedPizza = _context.Pizzas.FirstOrDefault(p => p.Id == id);

            if (savedPizza is null)
            {
                return NotFound();
            }

            savedPizza.Name = pizza.Name;
            savedPizza.Description = pizza.Description;
            savedPizza.Price = pizza.Price;
            savedPizza.ImgSrc = pizza.ImgSrc;
            savedPizza.CategoryId = pizza.CategoryId;

            _context.SaveChanges();

            return Ok(savedPizza);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePizza(int id)
        {
            var savedPizza = _context.Pizzas.FirstOrDefault(p => p.Id == id);

            if (savedPizza == null)
            {
                return NotFound();
            }

            _context.Pizzas.Remove(savedPizza);
            _context.SaveChanges();

            return Ok();
        }
    }
}
