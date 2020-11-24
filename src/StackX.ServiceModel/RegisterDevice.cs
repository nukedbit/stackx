using ServiceStack;
using StackX.ServiceModel.Types;
using System.Collections.Generic;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    public class RegisterDevice : IReturn<RegisterDeviceResponse>
    {

        [ValidateNotEmpty]
        [ValidateNotNull]
        [ValidateLength(5, 100)]
        public string Description { get; set; }

        [ValidateNotEmpty]
        [ValidateNotNull]
        [ValidateLength(10, 100)]
        public string DeviceId {get;set;}

        [ValidateGreaterThan(0)]
        public int DeviceKindId {get;set;}

        public Dictionary<string, string> Meta {get;set;}
    }
}