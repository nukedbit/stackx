using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{

    public class RegisterDeviceResponse : IHasResponseStatus
    {
        public bool DeviceBlocked { get; set;}
        public ResponseStatus ResponseStatus { get; set; }
    }
}