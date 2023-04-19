using la_mia_pizzeria_model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

namespace la_mia_pizzeria_model.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class PizzaController : Controller
    {
        private readonly ILogger<PizzaController> _logger;

        public PizzaController(ILogger<PizzaController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            using var ctx = new PizzaContext();

            var pizze = ctx.Pizzas.ToArray();

            return View(pizze);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            using var ctx = new PizzaContext();

            var pizza = ctx.Pizzas
                .Include(p => p.Category)
                .Include(p => p.Ingredients)
                .SingleOrDefault(p => p.Id == id);

            if(pizza == null)
            {
                return NotFound("Pizza non trovata!");
            }

            return View(pizza);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
			using var ctx = new PizzaContext();

			var formModel = new PizzaFormModel
            {
                Categories = ctx.Categories.ToArray(),
                Ingredients = ctx.Ingredients.Select(i => new SelectListItem(i.Name, i.Id.ToString())).ToArray(),
            }; 

            return View(formModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaFormModel form)
        {
            using var ctx = new PizzaContext();

            if (!ModelState.IsValid)
            {
                form.Categories = ctx.Categories.ToArray();
                form.Ingredients = ctx.Ingredients.Select(i => new SelectListItem(i.Name, i.Id.ToString())).ToArray();

                return View(form);
            }

            form.Pizza.Ingredients = form.SelectedIngredients.Select(si => ctx.Ingredients.First(i => i.Id == Convert.ToInt32(si))).ToList();

            ctx.Pizzas.Add(form.Pizza);

            ctx.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id)
        {
            using var ctx = new PizzaContext();
            
            var pizza = ctx.Pizzas.Include(p => p.Ingredients).FirstOrDefault(p => p.Id == id);

            if(pizza == null)
            {
                return NotFound();
            }

            var formModel = new PizzaFormModel
            {
                Pizza = pizza,

                Categories = ctx.Categories.ToArray(),

                Ingredients = ctx.Ingredients.ToArray().Select(i => new SelectListItem(i.Name, i.Id.ToString(), pizza.Ingredients!
                .Any(_i => _i.Id == i.Id)))
                .ToArray(),
            };

            formModel.SelectedIngredients = formModel.Ingredients.Where(i => i.Selected).Select(i => i.Value).ToList();

            return View(formModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Update(int id, PizzaFormModel form)
        {
            using var ctx = new PizzaContext();

            if (!ModelState.IsValid)
            {
                form.Categories = ctx.Categories.ToArray();
                form.Ingredients = ctx.Ingredients.Select(i => new SelectListItem(i.Name, i.Id.ToString())).ToArray();

                return View(form);
            }

            //var pizzaToUpdate = ctx.Pizzas.FirstOrDefault(p => p.Id == id);

            //if(pizzaToUpdate is null)
            //{
            //    return NotFound();
            //}

            //pizzaToUpdate.Name = form.Pizza.Name;
            //pizzaToUpdate.Description = form.Pizza.Description;
            //pizzaToUpdate.Price = form.Pizza.Price;
            //pizzaToUpdate.ImgSrc = form.Pizza.ImgSrc;
            //pizzaToUpdate.Category = form.Pizza.Category;

            form.Pizza.Ingredients = form.SelectedIngredients.Select(si => ctx.Ingredients.First(i => i.Id == Convert.ToInt32(si))).ToList();

            ctx.Pizzas.Update(form.Pizza);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(int id)
        {
			using var ctx = new PizzaContext();

            var pizzaToDelete = ctx.Pizzas.FirstOrDefault(p => p.Id == id);

            if(pizzaToDelete is null)
            {
                return NotFound();
            }

            ctx.Pizzas.Remove(pizzaToDelete);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}