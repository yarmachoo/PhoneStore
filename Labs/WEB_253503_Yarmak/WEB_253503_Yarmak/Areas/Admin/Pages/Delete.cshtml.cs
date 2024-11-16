using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_253503_Yarmak.API.Data;
using WEB_253503_Yarmak.Domain.Entities;
using WEB_253503_Yarmak.Services.ProductService;

namespace WEB_253503_Yarmak.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IProductService _productService;

        public DeleteModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Phone Phone { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var phone = await _productService.GetProductByIdAsync(id);

                if (phone == null)
                {
                    return NotFound();
                }
                else
                {
                    Phone = phone.Data;
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _productService.DeleteProductAsync(id);

            return RedirectToPage("./Index");
        }
    }
}

