using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_253503_Yarmak.API.Data;
using WEB_253503_Yarmak.Domain.Entities;
using WEB_253503_Yarmak.Services.CategoryService;
using WEB_253503_Yarmak.Services.ProductService;

namespace WEB_253503_Yarmak.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IHttpClientFactory _httpClientFactory;
        public EditModel(
            IProductService productService,
            ICategoryService categoryService,
            IHttpClientFactory httpClientFactory)
        {
            _productService = productService;
            _categoryService = categoryService;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public Phone Phone { get; set; } = default!;
        [BindProperty]
        public IFormFile? File { get; set; } 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phone = await _productService.GetProductByIdAsync(id.Value);
            if (phone == null)
            {
                return NotFound();
            }
            Phone = phone.Data;

            var categories = await _categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categories.Data, "Id", "Name");
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (File != null)
            {
                if (!string.IsNullOrEmpty(Phone.Image))
                {
                    await DeleteImageAsync(Phone.Image);
                }
                var imgUrl = await UploadFileAsync(File);
                if (string.IsNullOrEmpty(imgUrl))
                {
                    ModelState.AddModelError("", "Error with uploading image.");
                    return Page();
                }
                Phone.Image = imgUrl;
            }

            await _productService.UpdateProductasyc(Phone.Id, Phone, File);
                        
            return RedirectToPage("./Index");
            
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

            if (response.IsSuccessStatusCode)
            {
                var imgUrl = await response.Content.ReadAsStringAsync();
                return imgUrl;
            }

            return "";
        }

        private async Task DeleteImageAsync(string imgUrl)
        {
            var client = _httpClientFactory.CreateClient();
            var url = "https://localhost:7002/api/files";

            var uri = new Uri(imgUrl);
            var fileName = Path.GetFileName(uri.LocalPath);

            var requestUri = $"{url}?fileName={fileName}";
            await client.DeleteAsync(requestUri);
        }
    }
}
