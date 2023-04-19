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
            .Include(p => p.Category)
            .Include(p => p.Ingredients)
            .Where(p => name == null || p.Name.ToLower().Contains(name.ToLower()))
            .ToList();

            return Ok(pizzas);
        }

        [HttpGet]

    }
}
