using ServiceStack;

namespace StackX.ServiceModel
{
    public class GetFileUrl : IGet, IReturn<GetFileUrlResponse>
    {
        public int FileId { get; set; }
    }
}