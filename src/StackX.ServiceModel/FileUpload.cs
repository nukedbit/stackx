using ServiceStack;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateRequest(Condition = "HasRole('Admin') or HasRole('Contributor')")]
    public class FileUpload : IReturn<FileUploadResponse>, IPost
    {
        public string Folder { get; set; } 

        public string ReferencedBy { get; set; }

        [ValidateGreaterThan(0)] public int ApplicationId { get; set; }
        
        public string ExtraAttribute1 { get; set; }
        public string ExtraAttribute2 { get; set; }
        public string ExtraAttribute3 { get; set; }
        public string ExtraAttribute4 { get; set; }
    }
}