using ServiceStack;

namespace StackX.ServiceModel
{

    public interface IFileUploadAdditionalMetadata
    {
        public string Folder { get; set; } 

        public string ReferencedBy { get; set; }

        public int ApplicationId { get; set; }
        
        public string ExtraAttribute1 { get; set; }
        public string ExtraAttribute2 { get; set; }
        public string ExtraAttribute3 { get; set; }
        public string ExtraAttribute4 { get; set; }
    }

    public class FileUploadAdditionalMetadata : IFileUploadAdditionalMetadata
    {
        public string Folder { get; set; }
        public string ReferencedBy { get; set; }
        public int ApplicationId { get; set; }
        public string ExtraAttribute1 { get; set; }
        public string ExtraAttribute2 { get; set; }
        public string ExtraAttribute3 { get; set; }
        public string ExtraAttribute4 { get; set; }
    }

    [ValidateIsAuthenticated]
    [ValidateRequest(Condition = "HasRole('Admin') or HasRole('Contributor')")]
    public class FileUpload : IReturn<FileUploadResponse>, IPost, IFileUploadAdditionalMetadata
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