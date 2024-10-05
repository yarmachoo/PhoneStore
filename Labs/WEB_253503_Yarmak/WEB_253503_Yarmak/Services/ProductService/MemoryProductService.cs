using Microsoft.AspNetCore.Mvc;
using WEB_253503_Yarmak.Domain.Entities;
using WEB_253503_Yarmak.Domain.Models;
using WEB_253503_Yarmak.Services.CategoryService;
using System.Linq;

namespace WEB_253503_Yarmak.Services.ProductService
{
    public class MemoryProductService : IProductService
    {
        private List<Phone> _phones;
        private List<Category> _categories;
        public readonly int AmountOfPages;
        public MemoryProductService(
            [FromServices]IConfiguration config,
            ICategoryService categoryService)
        {
            //double diff = _phones.Count/config.GetValue<int>("ItemsPerPage");
            //_amountOfPages = (int)Math.Ceiling(diff);
            AmountOfPages = config.GetValue<int>("ItemsPerPage");

            _categories = categoryService.GetCategoryListAsync().Result.Data;
            SetupData();
        }
        /// <summary>
        /// Инициализация списков
        /// </summary>
        public void SetupData()
        {
            _phones = new List<Phone>()
            {
                new Phone{Id = 1, Name = "Samsung Galaxy A51",
                    Description = "Android",
                    Price = 1000,
                    Image = "Images/samsungA51.jpg",
                    CategoryId = 1 },
                new Phone{Id = 2, Name = "Samsung Galaxy Z Flip6",
                    Description = "Android",
                    Price = 1000,
                    Image = "Images/samsungGalaxyZFlip6.jpg",
                    CategoryId =1 },
                new Phone{Id = 3, Name = "Samsung Galaxy A55",
                    Description = "Android",
                    Price = 1000,
                    Image = "Images/samsungGalaxyA55.jpg",
                    CategoryId =1 },
                new Phone{Id = 4, Name = "Samsung Galaxy S24 Ultra Gray",
                    Description = "Android",
                    Price = 1000,
                    Image = "Images/samsungGalaxyS24Ultra.jpg",
                    CategoryId =1 },
                new Phone{Id = 5, Name = "Iphone 8 Plus",
                    Description = "IOS",
                    Price = 1300,
                    Image = "Images/iphone8Plus.jpg",
                    CategoryId =2 }
            };
        }
        public int GetAmountOfAllProduct()
        {
            return _phones.Count;
        }
        public int GetAmountOfProductWithCategory(int? categoryId)
        {
            return _phones.Where(c=>c.CategoryId == categoryId).Count();
        }
        public Task<ResponseData<ProductListModel<Phone>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            if (categoryNormalizedName == null)
            {
                List<Phone> productListPage = _phones.Skip((pageNo-1)*AmountOfPages).Take(AmountOfPages).ToList();
                var productList = new ProductListModel<Phone>() { Items = productListPage };

                var result = ResponseData<ProductListModel<Phone>>.Success(productList);
                return Task.FromResult(result);
            }
            else
            {
                int categoryId = _categories.Find(c => c.NormalizedName.Equals(categoryNormalizedName)).Id;
                var phones = _phones.FindAll(c => c.CategoryId.Equals(categoryId));
                if (phones is not null)
                {
                    List<Phone> productListPage = phones.Skip((pageNo - 1) * AmountOfPages).Take(AmountOfPages).ToList();
                    var productList = new ProductListModel<Phone>() { Items = productListPage };
                    var result = ResponseData<ProductListModel<Phone>>.Success(productList);
                    return Task.FromResult(result);
                }
                else
                {
                    var result = ResponseData<ProductListModel<Phone>>.Error("Data is not found");
                    return Task.FromResult(result);
                }
            }
            
        }

        public Task UpdateProductasyc(int id, Phone product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
        public Task<ResponseData<Phone>> CreateProductAsync(Phone product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Phone>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ProductListModel<Phone>>> GetProductListWithoutPageAsync(string? categoryNormalizedName)
        {
            throw new NotImplementedException();
        }
    }
}
