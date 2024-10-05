using WEB_253503_Yarmak.Domain.Entities;
using WEB_253503_Yarmak.Domain.Models;

namespace WEB_253503_Yarmak.Services.CategoryService
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = new List<Category>
            {
                new Category{ Id = 1, Name = "Samsung", NormalizedName = "samsung"},
                new Category{ Id = 2, Name = "Apple", NormalizedName = "apple"},
                new Category{ Id = 1, Name = "Honor", NormalizedName = "honor"},
                new Category{ Id = 1, Name = "Xiaomi", NormalizedName = "xiaomi"},
                new Category{ Id = 1, Name = "Nokia", NormalizedName = "nokia"},
                new Category{ Id = 1, Name = "Texet", NormalizedName = "texet"},
                new Category{ Id = 1, Name = "Poco", NormalizedName = "poco"}
            };
            var result = ResponseData<List<Category>>.Success(categories);
            return Task.FromResult(result);
        }
    }
}
