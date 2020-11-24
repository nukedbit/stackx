using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasPermission(ApplicationsPermissions.ApplicationCreate)]
    public class CreateApplication : CreateAuditBase<Application, ApplicationResponse>
    {
        [ValidateNotEmpty] public string Name { get; set; }
    }
}