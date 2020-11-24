using System.Collections.Generic;

namespace StackX.ServiceModel.Types
{
    public static class ApplicationsPermissions
    {
        public const string ApplicationRead = "ApplicationRead";
        public const string ApplicationCreate = "ApplicationCreate";
        public const string ApplicationUpdate = "ApplicationUpdate";
        public const string ApplicationDelete = "ApplicationDelete";

        public const string AllAuthorize = "ApplicationRead,ApplicationCreate,ApplicationUpdate,ApplicationDelete";
        
        public static readonly List<Permission> All = new List<Permission>()
        {
            new Permission(ApplicationRead),
            new Permission(ApplicationCreate),
            new Permission(ApplicationUpdate),
            new Permission(ApplicationDelete),
        };
    }
}