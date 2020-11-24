using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{

    [ValidateIsAuthenticated]
    [ValidateIsAdmin]
    public class DeleteDeviceKind : SoftDeleteAuditBase<DeviceKind, DeleteDeviceKindResponse>
    {
        [ValidateGreaterThan(0)]
        public int Id { get; set; }
    }
}