using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253503_Yarmak.API.Data;
using WEB_253503_Yarmak.Domain.Entities;
using WEB_253503_Yarmak.Services.CategoryService;
using WEB_253503_Yarmak.Services.ProductService;

namespace WEB_253503_Yarmak.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IHttpClientFactory _httpClientFactory;
        public CreateModel(
            IProductService productService,
            ICategoryService categoryService,
            IHttpClientFactory httpClientFactory)
        {
            _productService = productService;
            _categoryService = categoryService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var categories = await _categoryService.GetCategoryListAsync();
            return Page();
        }

        [BindProperty]
        public Phone Phone { get; set; } = default!;
        [BindProperty]
        public IFormFile File { get; set; } = null;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (File != null)
            {
                var imgUrl =await UploadFileAsync(File);
                if (string.IsNullOrEmpty(imgUrl))
                {
                    ModelState.AddModelError("", "Error with uploading image.");
                    return Page();
                }
                Phone.Image = imgUrl;
            }

            var response = await _productService.CreateProductAsync(Phone, File);

            if (response.Successfull)
            {
                return RedirectToPage("./Index");
            }

            ModelState.AddModelError("", $"The phone is not created {response.ErrorMessage}");
            return Page();
        }
        private async Task<string> UploadFileAsync(IFormFile formFile)
        {
            var client = _httpClientFactory.CreateClient();
            var url = "https://localhost:7002/api/files";

            using var multipartFromContent = new MultipartFormDataContent();
            using var sc = new StreamContent(formFile.OpenReadStream());
            sc.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);
            multipartFromContent.Add(sc, "file", formFile.FileName);

            var response = await client.PostAsync(url, multipartFromContent);

            if(response.IsSuccessStatusCode)
            {
                var imgUrl = await response.Content.ReadAsStringAsync();
                return imgUrl;
            }

            return "";
        }
    }
}
