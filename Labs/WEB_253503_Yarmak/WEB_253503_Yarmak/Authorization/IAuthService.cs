namespace WEB_253503_Yarmak.Authorization
{
    public interface IAuthService
    {
        Task<(bool result, string ErrorMessage)> RegisterUserAsync(
            string email, string password, IFormFile? avatar);
    }
}
