using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    public class LogResponse : IHasResponseStatus
    {
        public Log Result { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}