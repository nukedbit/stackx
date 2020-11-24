using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    public class UpdateDeviceKindResponse : ICrudResponse<DeviceKind>
    {
        public int Id { get; set; }
        public DeviceKind Result { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}