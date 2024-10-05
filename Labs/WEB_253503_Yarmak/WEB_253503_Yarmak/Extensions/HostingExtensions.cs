using WEB_253503_Yarmak.Services.CategoryService;
using WEB_253503_Yarmak.Services.ProductService;

namespace WEB_253503_Yarmak.Extensions
{
    public static class HostingExtensions
    {
        /// <summary>
        /// Регистрация созданных сервисов
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterCustomServices(
            this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoryService, ApiCategoryService>();
            builder.Services.AddScoped<IProductService, ApiProductService>();
        }
    }
}
