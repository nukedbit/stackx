using ServiceStack;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasPermission(Permissions.UserUpdate)]
    public class ActivateUser : IReturnVoid, IPost
    {
        public string UserId { get; set; }
    }
}