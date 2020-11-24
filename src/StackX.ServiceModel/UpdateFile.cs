using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateRequest(Condition = "HasRole('Admin') or HasRole('Contributor')")]
    public class UpdateFile : UpdateAuditBase<File, FileUploadResponse>, IPut
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public string FileHash { get; set; }

        public string ReferencedBy { get; set; }

        public string ExtraAttribute1 { get; set; }
        public string ExtraAttribute2 { get; set; }
        public string ExtraAttribute3 { get; set; }
        public string ExtraAttribute4 { get; set; }
    }
}