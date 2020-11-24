using ServiceStack;

namespace StackX.ServiceModel
{
    public class AvailableRolePermissionResponse : IHasResponseStatus
    {
        public Role[] Roles { get; set; }
        public Permission[] Permissions { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}