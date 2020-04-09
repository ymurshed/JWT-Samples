using Auth.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Services
{
    public interface IJwtService
    {
        public string GetToken(User user);
        public TokenValidationParameters GeTokenValidationParameters();
    }
}
