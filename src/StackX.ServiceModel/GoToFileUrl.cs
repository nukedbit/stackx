using ServiceStack;

namespace StackX.ServiceModel
{
    [Route("/api/files/redirect")]
    public class GoToFileUrl : IGet
    {
        public int FileId { get; set; }
    }
}