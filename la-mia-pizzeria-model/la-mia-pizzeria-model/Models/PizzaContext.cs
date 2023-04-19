using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_model.Models
{
    public class PizzaContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Category> Categories { get; set; }
		public DbSet<Ingredient> Ingredients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=PizzaDb;Integrated Security=True;Pooling=False;TrustServerCertificate = True");
        }

        public void Seed()
        {

			var pizzaSeed = new Pizza[]
			{
				new Pizza
				{
					ImgSrc = "/img/pizza-margherita.jpg",
					Name = "Pizza Margherita",
					Description = "La classica pizza italiana",
					Price = "4.99 €"
				},
				new Pizza
				{
					ImgSrc = "/img/pizza-americana.jpg",
					Name = "Pizza Americana",
					Description = "La pizza con patatine e wurstel",
					Price = "6.99 €"
				},
				new Pizza
				{
					ImgSrc = "/img/pizza-napoli.jpg",
					Name = "Pizza Napoli",
					Description = "La preferita dai napoletani",
					Price = "5.99 €"
				},
			};

            if (!Pizzas.Any())
            {
                Pizzas.AddRange(pizzaSeed);
            };

			if (!Categories.Any())
			{
				var seed = new Category[]
				{
					new Category
					{
						Name = "Pizze classiche",
						Pizzas = pizzaSeed,
                    },
                    new Category
                    {
                        Name = "Pizze speciali",
                    },
                    new Category
                    {
                        Name = "Pizze della casa",
                    }
				};

                Categories.AddRange(seed);
			};

			if (!Ingredients.Any())
			{
                var seed = new Ingredient[]
                {
                    new Ingredient
                    {
                        Name = "Farina",
                        Pizzas = pizzaSeed,
                    },
                    new Ingredient
                    {
                        Name = "Pomodoro",
                    },
                    new Ingredient
                    {
                        Name = "Mozzarella",
                    },
                    new Ingredient
                    {
                        Name = "Basilico",
                    },
                    new Ingredient
                    {
                        Name = "Patatine fritte",
                    },
                    new Ingredient
                    {
                        Name = "Acciughe",
                    }
                };

                Ingredients.AddRange(seed);
            }

            if (!Roles.Any())
            {
                var seed = new IdentityRole[]
                {
                    new("Admin"),
                    new("User"),
                };

                Roles.AddRange(seed);
            }

            if (Users.Any(u => u.Email == "admin2@dev.com" || u.Email == "user@dev.com") && !UserRoles.Any())
            {
                var admin = Users.First(u => u.Email == "admin2@dev.com");
                var user = Users.First(u => u.Email == "user@dev.com");

                var adminRole = Roles.First(r => r.Name == "Admin");
                var userRole = Roles.First(r => r.Name == "User");

                var seed = new IdentityUserRole<string>[]
                {
                    new()
                    {
                        UserId = admin.Id,
                        RoleId = adminRole.Id,
                    },
                    new()
                    {
                        UserId = user.Id,
                        RoleId = userRole.Id,
                    }
                };

                UserRoles.AddRange(seed);
            }

			SaveChanges();
		}
    }
}
