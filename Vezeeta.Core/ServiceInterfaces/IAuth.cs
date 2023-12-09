using Vezeeta.Core.Dtos.AuthDtos;
using Vezeeta.Core.Models;

namespace Vezeeta.Core.ServiceInterfaces
{
    public interface IAuth
    {
        public Task<ApplicationUser> FindUserAsync(string id);
        public Task<AuthModel> OnRegisterAsync(RegisterDTO regInfo, string FolderName, string userType);
        public Task<AuthModel> OnLoginAsync(LoginDTO logInfo);
        public Task<string> CreateJWTAsync(ApplicationUser user);
        public Task<string> OnSetRoleAsync(RoleSetModelDTO requestInfo);
    }
}
