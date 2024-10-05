using WEB_253503_Yarmak.Domain.Entities;
using WEB_253503_Yarmak.Domain.Models;

namespace WEB_253503_Yarmak.Services.CategoryService
{
    public class ApiCategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        public ApiCategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7002/");
        }
        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<List<Category>>>("Categories");
        }
    }
}
