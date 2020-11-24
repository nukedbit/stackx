using System.Collections.Generic;

namespace StackX.ServiceModel
{
    public static class Permissions
    {
        public const string UserRead = "UserRead";
        public const string UserCreate = "UserCreate";
        public const string UserUpdate = "UserUpdate";
        public const string UserDelete = "UserDelete";
        
        public static readonly HashSet<Permission> All = new HashSet<Permission>()
        {
            new Permission(UserRead),
            new Permission(UserCreate),
            new Permission(UserUpdate),
            new Permission(UserDelete),
        };
    }
}