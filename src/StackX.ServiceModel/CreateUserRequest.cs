using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasPermission(Permissions.UserCreate)]
    public class CreateUserRequest : IReturn<UserUpdatedResponse>, IPost
    {
        public AppUserDto AppUser { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}