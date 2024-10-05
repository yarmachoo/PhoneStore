using Microsoft.EntityFrameworkCore;
using WEB_253503_Yarmak.API.Data;
using WEB_253503_Yarmak.Domain.Entities;
using WEB_253503_Yarmak.Domain.Models;

namespace WEB_253503_Yarmak.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _appDbContext;
        public CategoryService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = await _appDbContext.Categories.ToListAsync();
            return ResponseData<List<Category>>.Success(categories);
        }
    }
}
