using HRMS.Domain.Entities;

namespace HRMS.Application.Common.Interfaces.Auth;

public interface ITokenService
{
    string GenerateAccessToken(User user, IList<string> roles);
    string GenerateRefreshToken();
}
