using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManagerApp.Data;
using TaskManagerApp.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        context.Database.Migrate(); // Ensures DB is created and migrations applied

        // Seed Categories
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Name = "Work" },
                new Category { Name = "Personal" },
                new Category { Name = "Urgent" }
            );
            context.SaveChanges();
        }

        // Seed Tasks (only if there are no tasks)
        if (!context.TaskItems.Any())
        {
            var user = userManager.FindByEmailAsync("test@example.com").Result;

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "test@example.com",
                    Email = "test@example.com"
                };
                userManager.CreateAsync(user, "Password123!").Wait();
            }

            context.TaskItems.AddRange(
                new TaskItem
                {
                    Name = "Finish ASP.NET Project",
                    Description = "Complete task management web app",
                    DueDate = DateTime.Now.AddDays(3),
                    Status = "In Progress",
                    UserId = user.Id
                },
                new TaskItem
                {
                    Name = "Buy Groceries",
                    Description = "Milk, Eggs, Bread",
                    DueDate = DateTime.Now.AddDays(1),
                    Status = "Pending",
                    UserId = user.Id
                }
            );

            context.SaveChanges();
        }
    }
}