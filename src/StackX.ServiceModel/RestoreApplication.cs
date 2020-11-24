using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasPermission(ApplicationsPermissions.ApplicationUpdate)]
    public class RestoreApplication : RemoveSoftDeleteAuditBase<Application, ApplicationResponse>
    {
        [ValidateGreaterThan(0)] public int Id { get; set; }
    }
}