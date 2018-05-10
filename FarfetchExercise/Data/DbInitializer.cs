using FarfetchExercise.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarfetchExercise.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IWebHost host, ApplicationDbContext context)
        {
            // Create Db if it does not exist
            context.Database.EnsureCreated();

            var services = (IServiceScopeFactory)host.Services.GetService(typeof(IServiceScopeFactory));
            var serviceScope = services.CreateScope();

            SeedRoles(serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>(), context);
            SeedUsers(serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>(), context);

            SeedData(context);
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            if (!context.Roles.Any())
            {
                var rolesToAdd = new List<IdentityRole>(){
                    new IdentityRole { Name= "Admin" },
                    new IdentityRole { Name= "User" }
                };
                foreach (var role in rolesToAdd)
                {
                    if (!roleManager.RoleExistsAsync(role.Name).Result)
                    {
                        roleManager.CreateAsync(role).Result.ToString();
                    }
                }
            }
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            if (!context.ApplicationUsers.Any())
            {
                /* ADMIN */
                userManager.CreateAsync(new ApplicationUser { UserName = "genericAdmin@admin.com", Email = "genericAdmin@admin.com", EmailConfirmed = true }, "Pass123.").Result.ToString();
                userManager.AddToRoleAsync(userManager.FindByNameAsync("genericAdmin@admin.com").GetAwaiter().GetResult(), "Admin").Result.ToString();

                /* USER */
                userManager.CreateAsync(new ApplicationUser { UserName = "genericUser@user.com", Email = "genericUser@user.com", EmailConfirmed = true }, "Pass123.").Result.ToString();
                userManager.AddToRoleAsync(userManager.FindByNameAsync("genericUser@user.com").GetAwaiter().GetResult(), "User").Result.ToString();
            }
        }

        private static void SeedData(ApplicationDbContext context)
        {
            // Look for any values.
            if (context.Points.Any() || context.Routes.Any())
            {
                return;   // DB has been seeded
            }

            SeedPoints(context);

            SeedRoutes(context);
        }

        private static void SeedRoutes(ApplicationDbContext context)
        {
            var routes = new Route[] {
                new Route{ Name= "teste", Time= 10, Cost= 1, OriginId= 1, DestinationId= 8 },
                new Route{ Name= "teste", Time= 1, Cost= 20, OriginId= 1, DestinationId= 3},
                new Route{ Name= "teste", Time= 30, Cost= 5, OriginId= 1, DestinationId= 5},
                new Route{ Name= "teste", Time= 30, Cost= 1, OriginId= 8, DestinationId= 5},
                new Route{ Name= "teste", Time= 3, Cost= 5, OriginId= 5, DestinationId= 4},
                new Route{ Name= "teste", Time= 4, Cost= 50, OriginId= 4, DestinationId= 6},
                new Route{ Name= "teste", Time= 45, Cost= 50, OriginId= 6, DestinationId= 9},
                new Route{ Name= "teste", Time= 40, Cost= 50, OriginId= 6, DestinationId= 7},
                new Route{ Name= "teste", Time= 1, Cost= 12, OriginId= 3, DestinationId= 2},
                new Route{ Name= "teste", Time= 65, Cost= 5, OriginId= 9, DestinationId= 2},
                new Route{ Name= "teste", Time= 64, Cost= 73, OriginId= 7, DestinationId= 2}
            };

            foreach (Route r in routes)
            {
                context.Routes.Add(r);
            }
            context.SaveChanges();
        }

        private static void SeedPoints(ApplicationDbContext context)
        {
            var points = new Point[] {
                new Point{ Name= "A"} ,
                new Point{ Name= "B" },
                new Point{ Name= "C" },
                new Point{ Name= "D" },
                new Point{ Name= "E" },
                new Point{ Name= "F" },
                new Point{ Name= "G" },
                new Point{ Name= "H" },
                new Point{ Name= "I" }
            };

            foreach (Point p in points)
            {
                context.Points.Add(p);
            }
            context.SaveChanges();
        }
    }
}
