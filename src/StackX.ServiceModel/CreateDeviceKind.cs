using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{

    [ValidateIsAuthenticated]
    [ValidateIsAdmin]
    public class CreateDeviceKind : CreateAuditBase<DeviceKind, CreateDeviceKindResponse>
    {
        [ValidateNotEmpty][ValidateNotNull] public string Name { get; set; } 
    }
}