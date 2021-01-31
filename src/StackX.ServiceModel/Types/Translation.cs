using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace StackX.ServiceModel.Types
{
    [CompositeIndex(nameof(Key), nameof(LanguageId), nameof(ApplicationId), Unique = true)]
    public class Translation : AuditBase, IHasIntId
    {
        [AutoIncrement] public int Id { get; set; }

        [Reference] public Application Application { get; set; }
        public int ApplicationId { get; set; }
        public string Key { get; set; }

        [Reference] public Language Language { get; set; }
        public int LanguageId { get; set; }
        
        [CustomField("{NMAX_TEXT}")]
        public string Value { get; set; }
    }
}