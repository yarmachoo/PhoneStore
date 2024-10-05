using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using WEB_253503_Yarmak.Domain.Entities;
using WEB_253503_Yarmak.Domain.Models;

namespace WEB_253503_Yarmak.Services.ProductService
{
    public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiProductService> _logger;
        private string? _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        public ApiProductService(HttpClient httpClient,
            IConfiguration configuration,
            ILogger<ApiProductService> logger)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7002/");
            _logger = logger;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _pageSize = configuration.GetSection("ItemsPerPage").Value;

        }
       
        public async Task<ResponseData<Phone>> CreateProductAsync(Phone product, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Phones");
            var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content
                    .ReadFromJsonAsync<ResponseData<Phone>>(_serializerOptions);
                return data;
            }
            _logger.LogError($"-----> object is not created. Error: {response.StatusCode.ToString()}");

            return ResponseData<Phone>.Error($"-----> object is not created. Error: {response.StatusCode.ToString()}");
        }

        public async Task DeleteProductAsync(int id)
        {
            string url = $"phone/delete/{id}?";
            await _httpClient.DeleteAsync(url);
        }

        public async Task<ResponseData<Phone>> GetProductByIdAsync(int id)
        {
            string url = $"phone/getphone/{id}";
            return await _httpClient.GetFromJsonAsync<ResponseData<Phone>>(url);
        }

        public async Task<ResponseData<ProductListModel<Phone>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            string url = $"phone/getphones/{categoryNormalizedName}?pageNo={pageNo}";
            Console.WriteLine(url);
            return await _httpClient.GetFromJsonAsync<ResponseData<ProductListModel<Phone>>>(url);
        }

        public Task UpdateProductasyc(int id, Phone product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<ProductListModel<Phone>>> GetProductListWithoutPageAsync(string? categoryNormalizedName)
        {
            string url = $"phone/phones/{categoryNormalizedName}";
            Console.WriteLine(url);
            return await _httpClient.GetFromJsonAsync<ResponseData<ProductListModel<Phone>>>(url);
        }
    }
}
