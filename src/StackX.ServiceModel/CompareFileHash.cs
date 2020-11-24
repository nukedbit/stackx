using ServiceStack;

namespace StackX.ServiceModel
{
    public class CompareFileHash: IReturn<CompareFileHasResponse>
    {
        public string Hash { get; set; }
        public int FileId { get; set; }
    }
}