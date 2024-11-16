namespace WEB_253503_Yarmak.API.Services.Authentication
{
    public interface ITokenAccessor
    {
        Task<string> GetAccesstokenAsync();
        /// <summary>
        /// Добавление заголовка Authorisation: bearer 
        /// </summary>
        /// <param name="httpClient">HttpClientб в который добавляется заголовок</param>
        /// <returns></returns>
        Task SetAuthorisationHeaderAsync(HttpClient httpClient);
    }
}
