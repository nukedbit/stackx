using ServiceStack;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    public class GetFileContent : IReturn<GetFileContentResponse>
    {
        public int Id { get; set; }
    }
}