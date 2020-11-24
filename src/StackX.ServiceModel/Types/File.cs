using ServiceStack;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace StackX.ServiceModel.Types
{
    public class File : AuditBase, IHasIntId
    {
        [AutoIncrement] public int Id { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public string FileHash { get; set; }

        public string ReferencedBy { get; set; }

        public int ApplicationId { get; set; }

        [IgnoreDataMember]
        public Application Application { get; set; }
        
        public string ExtraAttribute1 { get; set; }
        public string ExtraAttribute2 { get; set; }
        public string ExtraAttribute3 { get; set; }
        public string ExtraAttribute4 { get; set; }
    }
}