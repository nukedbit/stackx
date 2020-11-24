using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateIsAdmin]
    [AutoApply(Behavior.AuditQuery)]
    public class QueryDeviceKinds : QueryDb<DeviceKind>
    {
        public long? Id {get;set;}
    }
}