 using TestApiJWT.Models;

namespace TestApiJWT.Services
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> GetToken(TokenRequestModel model);
        Task<string> AddRoleAsync(AddRoleModel user);
    
    }
}
