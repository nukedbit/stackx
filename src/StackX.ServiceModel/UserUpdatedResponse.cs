using ServiceStack;

namespace StackX.ServiceModel
{
    public class UserUpdatedResponse : IHasResponseStatus
    {
        public int Id { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}