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
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public IList<Phone> Phone { get;set; } = new List<Phone>();

        public async Task OnGetAsync()
        {
            var response = await _productService.GetProductListAsync(null);
            if(response.Successfull && response.Data!= null)
            {
                Phone = response.Data.Items;
            }
            else 
            { 
                Phone = new List<Phone>();
            }
        }
    }
}
