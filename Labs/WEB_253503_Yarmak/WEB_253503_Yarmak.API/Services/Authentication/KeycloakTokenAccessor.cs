
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using WEB_253503_Yarmak.API.HelperClasses;

namespace WEB_253503_Yarmak.API.Services.Authentication
{
    public class KeycloakTokenAccessor : ITokenAccessor
    {
        private readonly KeycloakData _keycloakData;
        private readonly HttpContext? _httpContext;
        private readonly HttpClient _httpClient;
        public KeycloakTokenAccessor(
            IOptions<KeycloakData> options,
            IHttpContextAccessor httpContextAccessor,
            HttpClient httpClient)
        {
            _keycloakData = options.Value;
            _httpContext = httpContextAccessor.HttpContext;
            _httpClient = httpClient;

            // Проверяем, что значение Host не null
            if (string.IsNullOrEmpty(_keycloakData.Host))
            {
                throw new ArgumentNullException(nameof(_keycloakData.Host), "Keycloak host cannot be null or empty");
            }

            // Устанавливаем BaseAddress, если host указан
            _httpClient.BaseAddress = new Uri(_keycloakData.Host);

        }
        public async Task<string> GetAccesstokenAsync()
        {
            if(_httpContext.User.Identity.IsAuthenticated)
            {
                return await _httpContext.GetTokenAsync("access_token");
            }

            var requestUri =
                $"/realms/{_keycloakData.Realm}/protocol/openid-connect/token";

            //Http request content
            HttpContent content = new FormUrlEncodedContent([
                new KeyValuePair<string, string>("client_id", _keycloakData.ClientId),
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_secret", _keycloakData.ClientSecret)
            ]);

            //send request
            var response = await _httpClient.PostAsync(requestUri, content);

            if(!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(response.StatusCode.ToString());
            }

            //extract access token from response
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonObject.Parse(jsonString)["access_token"].GetValue<string>();
        }

        public async Task SetAuthorisationHeaderAsync(HttpClient httpClient)
        {
            /*string token = await GetAccesstokenAsync();

            httpClient
                .DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue("bearer", token);*/
        }
    }
}
