
using Microsoft.AspNetCore.Http.HttpResults;
using WEB_253503_Yarmak.API.Services.Authentication;
using WEB_253503_Yarmak.Domain.Entities;
using WEB_253503_Yarmak.Domain.Models;

namespace WEB_253503_Yarmak.Services.FileService
{
    public class ApiFileService : IFileService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpContext _httpContext;
        private readonly ITokenAccessor _tokenAccessor;
        public ApiFileService(HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor,
            ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _httpContext = httpContextAccessor.HttpContext;
            _tokenAccessor = tokenAccessor;
        }
        public async Task DeleteFileAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Console.WriteLine("Error: there is no Uri to delete file.");
                throw new ArgumentNullException(nameof(fileName));
            }

            Console.WriteLine($"Start deleting file via URI: {fileName}");

            await _tokenAccessor.SetAuthorisationHeaderAsync(_httpClient);

            var response = await _httpClient.DeleteAsync(fileName);

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("File deleted successfull");
            }
            else
            {
                Console.WriteLine($"Error with deleting file: {response.StatusCode}");
                throw new InvalidOperationException($"Error with deleting file: {response.StatusCode}");
            }
        }
        public async Task<string> SaveFileAsync(IFormFile formFile)
        {
            if(formFile == null)
            {
                Console.WriteLine("Error: File is not get");
                throw new ArgumentNullException(nameof(formFile));
            }

            Console.WriteLine($"Start uploading file {formFile.FileName}");

            await _tokenAccessor.SetAuthorisationHeaderAsync(_httpClient);
            
            var request = new HttpRequestMessage(HttpMethod.Post, "files");

            var extension = Path.GetExtension(formFile.FileName);
            var fileName = Path.ChangeExtension(Path.GetRandomFileName(), extension);
            
            Console.WriteLine($"New file name is {fileName}");

            var content = new MultipartFormDataContent();
            var sc = new StreamContent(formFile.OpenReadStream());
            content.Add(sc, "file", fileName);

            request.Content = content;

            Console.WriteLine("Send request to server");
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else {
                throw new InvalidOperationException("Error with uploading file");
            }
            ////Создать объект запроса для отправки данных на сервер
            //var request = new HttpRequestMessage
            //{
            //    Method = HttpMethod.Post
            //};
            ////Сформировать случайное имя файла, сохраняя расширение
            //var extension = Path.GetExtension(formFile.FileName);
            //var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);
            //// Создать контент, содержащий поток загруженного файла
            //var content = new MultipartFormDataContent();
            //var streamContent = new StreamContent(formFile.OpenReadStream());
            //content.Add(streamContent, "file", newName);
            ////поместить контент в запрос
            //request.Content = content;
            ////Отправить запрос к API
            //var response = await _httpClient.SendAsync(request);
            //if(response.IsSuccessStatusCode)
            //{
            //    return await response.Content.ReadAsStringAsync();
            //}
            //return String.Empty;
        }
    }
}
