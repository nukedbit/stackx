using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateIsAdmin]
    public class RestoreDeviceKind : SoftDeleteAuditBase<DeviceKind, RestoreDeviceKindResponse>
    {
        [ValidateGreaterThan(0)]
        public int Id { get; set; }
    }
}