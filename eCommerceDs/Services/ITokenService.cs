using eCommerceDs.DTOs;

namespace eCommerceDs.Services
{
    public interface ITokenService
    {
        LoginResponseDTO GenerateToken(UserLoginDTO user);
    }
}
