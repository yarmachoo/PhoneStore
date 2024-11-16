using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using WEB_253503_Yarmak.API.Services.Authentication;
using WEB_253503_Yarmak.Domain.Entities;
using WEB_253503_Yarmak.Domain.Models;
using WEB_253503_Yarmak.Services.FileService;

namespace WEB_253503_Yarmak.Services.ProductService
{
    public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiProductService> _logger;
        private string? _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly IFileService _fileService; 
        private readonly ITokenAccessor _tokenAccessor;
        
        public ApiProductService(HttpClient httpClient,
            IConfiguration configuration,
            ILogger<ApiProductService> logger,
            IFileService fileService,
            ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7002/");
            _logger = logger;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _fileService = fileService;
            _tokenAccessor = tokenAccessor;
        }
       
        public async Task<ResponseData<Phone>> CreateProductAsync(Phone product, IFormFile? formFile)
        {
            await _tokenAccessor.SetAuthorisationHeaderAsync(_httpClient);
            //product.Image = "Images/nophoto.jpg";

            if(formFile!=null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    product.Image = imageUrl;
                }
            }

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
            await _tokenAccessor.SetAuthorisationHeaderAsync(_httpClient);

            string url = $"phone/delete/{id}?";
            await _httpClient.DeleteAsync(url);
        }

        public async Task<ResponseData<Phone>> GetProductByIdAsync(int id)
        {
            await _tokenAccessor.SetAuthorisationHeaderAsync(_httpClient);

            string url = $"phone/{id}";
            return await _httpClient.GetFromJsonAsync<ResponseData<Phone>>(url);
        }

        public async Task<ResponseData<ProductListModel<Phone>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            await _tokenAccessor.SetAuthorisationHeaderAsync(_httpClient);

            string url = $"phone/getphones/{categoryNormalizedName}?pageNo={pageNo}";
            Console.WriteLine(url);
            var result =  await _httpClient.GetFromJsonAsync<ResponseData<ProductListModel<Phone>>>(url);
            return result;
        }

        public async Task UpdateProductasyc(int id, Phone product, IFormFile? formFile)
        {
            await _tokenAccessor.SetAuthorisationHeaderAsync(_httpClient);

            if(formFile!=null)
            {
                var imgUrl = await _fileService.SaveFileAsync(formFile);
                if(!string.IsNullOrEmpty(imgUrl))
                {
                    product.Image = imgUrl;
                }
            }
            var response = await _httpClient.PutAsJsonAsync($"phone/put/{id}", product);
            if(!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error with updating phone: {response.StatusCode}");
            }

        }

        public async Task<ResponseData<ProductListModel<Phone>>> GetProductListWithoutPageAsync(string? categoryNormalizedName)
        {
            await _tokenAccessor.SetAuthorisationHeaderAsync(_httpClient );

            string url = $"phone/phones/{categoryNormalizedName}";
            Console.WriteLine(url);
            return await _httpClient.GetFromJsonAsync<ResponseData<ProductListModel<Phone>>>(url);
        }
    }
}
