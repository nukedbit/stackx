using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasPermission(ApplicationsPermissions.ApplicationUpdate)]
    public class UpdateApplication : UpdateAuditBase<Application, ApplicationResponse>
    {
        [ValidateGreaterThan(0)] public int Id { get; set; }
        [ValidateNotEmpty] public string Name { get; set; }
    }
}