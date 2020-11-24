using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateIsAdmin]
    [AutoApply(Behavior.AuditQuery)]
    public class QueryDevices : QueryDb<Device>
    {
        public long? Id {get;set;}
        public string DeviceId {get;set;}
    }
    [ValidateIsAuthenticated]
    [ValidateIsAdmin]
    public class QueryAllDevices : QueryDb<Device>
    {
        public long? Id {get;set;}
        public string DeviceId {get;set;}
    }
}