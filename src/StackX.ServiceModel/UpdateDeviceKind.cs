using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateIsAdmin]
    public class UpdateDeviceKind : UpdateAuditBase<DeviceKind, UpdateDeviceKindResponse>
    {
        [ValidateGreaterThan(0)]
        public int Id { get; set; }
        [ValidateNotEmpty] [ValidateNotNull] public string Name { get; set; }
    }
}