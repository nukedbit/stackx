using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasPermission(ApplicationsPermissions.ApplicationDelete)]
    public class DeleteApplication : SoftDeleteAuditBase<Application, ApplicationResponse>
    {
        [ValidateGreaterThan(0)] public int Id { get; set; }
    }
}