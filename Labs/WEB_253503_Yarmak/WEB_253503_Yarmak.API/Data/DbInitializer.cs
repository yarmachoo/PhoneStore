using Microsoft.EntityFrameworkCore;
using WEB_253503_Yarmak.Domain.Entities;
using WEB_253503_Yarmak.Domain.Models;

namespace WEB_253503_Yarmak.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var config = app.Configuration;
            string adress = config["Address"];

            await context.Categories.AddRangeAsync(
                new Category { Name = "Samsung", NormalizedName = "samsung" },
                new Category { Name = "Apple", NormalizedName = "apple" },
                new Category { Name = "Honor", NormalizedName = "honor" },
                new Category { Name = "Xiaomi", NormalizedName = "xiaomi" },
                new Category { Name = "Nokia", NormalizedName = "nokia" },
                new Category { Name = "Texet", NormalizedName = "texet" },
                new Category { Name = "Poco", NormalizedName = "poco" }
            );
            await context.SaveChangesAsync();
            var samsungCategoryId = context.Categories.First(c => c.Name == "Samsung").Id;
            var appleCategoryId = context.Categories.First(c => c.Name == "Apple").Id;

            await context.Phones.AddRangeAsync(
                new Phone
                {
                    Name = "Samsung Galaxy A51",
                    Description = "Android",
                    Price = 1000,
                    Image = adress + "Images/samsungA51.jpg",
                    CategoryId = samsungCategoryId,
                    Mime = "jpg"
                },
                new Phone
                {
                    Name = "Samsung Galaxy Z Flip6",
                    Description = "Android",
                    Price = 1000,
                    Image = adress + "Images/samsungGalaxyZFlip6.jpg",
                    CategoryId = samsungCategoryId,
                    Mime = "jpg"
                },
                new Phone
                {
                    Name = "Samsung Galaxy A55",
                    Description = "Android",
                    Price = 1000,
                    Image = adress + "Images/samsungGalaxyA55.jpg",
                    CategoryId = samsungCategoryId,
                    Mime = "jpg"
                },
                new Phone
                {
                    Name = "Samsung Galaxy S24 Ultra Gray",
                    Description = "Android",
                    Price = 1000,
                    Image = adress+"Images/samsungGalaxyS24Ultra.jpg",
                    CategoryId = samsungCategoryId,
                    Mime = "jpg"
                },
                new Phone
                {
                    Name = "Iphone 8 Plus",
                    Description = "IOS",
                    Price = 1300,
                    Image = adress+"Images/iphone8Plus.jpg",
                    CategoryId = appleCategoryId,
                    Mime = "jpg"
                }
            );
            await context.SaveChangesAsync();
            await context.Database.MigrateAsync();
        }
    }
}
