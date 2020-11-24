using ServiceStack;

namespace StackX.ServiceModel
{
    public class CompareFileHasResponse : IHasResponseStatus
    {
        public bool AreEqual { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}