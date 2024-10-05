using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using WEB_253503_Yarmak.Domain.Entities;
using WEB_253503_Yarmak.Domain.Models;
using WEB_253503_Yarmak.Services.CategoryService;
using WEB_253503_Yarmak.Services.ProductService;

namespace WEB_253503_Yarmak.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductService productService,
            ICategoryService categoryService,
            ILogger<ProductController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _logger = logger;   
        }

        public async Task<IActionResult> Index(string? category, int pageNo = 1)
       {
            List<Category> categoryList = (await _categoryService.GetCategoryListAsync()).Data;
            ViewData["Categories"] = categoryList;
            ViewData["currentCategory"] = (category is null)?null:char.ToUpper(category[0]) + category.Substring(1);


            var request = HttpContext.Request;
            string? _category = request.RouteValues["category"]?.ToString();
            int categoryId = 0;

            ResponseData<ProductListModel<Phone>> productResponse;
            if (category == null || category=="Все")
            {
                productResponse =
                    await _productService.GetProductListAsync(null, pageNo);
            }
            else
            {
                productResponse =
                    await _productService.GetProductListAsync(category, pageNo);
                categoryId = categoryList.Find(c => c.NormalizedName == category).Id;
            }
            if (!productResponse.Successfull)
                return NotFound(productResponse.ErrorMessage);

            ProductListModel<Phone> productWithCategoryListWithoutPages  = (await _productService.GetProductListWithoutPageAsync(category)).Data;
            ProductListModel<Phone> productListWithoutPages = (await _productService.GetProductListWithoutPageAsync(null)).Data;


            ProductListModel<Phone> productListModel = new ProductListModel<Phone>(
                (category == null || category == "все") ? productListWithoutPages.Items.Count : productWithCategoryListWithoutPages.Items.Count,
            pageNo,
                3
            );

            productResponse.Data.TotalPages = productListModel.TotalPages;

            ViewData["ProductListModel"] = productListModel;
            return View(productResponse.Data);
        }
    }
}
