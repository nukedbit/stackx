using ServiceStack;
using ServiceStack.DataAnnotations;

namespace StackX.ServiceModel.Types
{
    [Alias("menu_item")]
    public class MenuItem : AuditBase
    {
        [AutoIncrement] public int Id { get; set; }
        
        [Required] public string Title { get; set; }
        
        public int? ResourceId { get; set; }
        
        public string Url { get; set; }

        [Reference] public Menu Menu { get; set; }
        
        public int MenuId { get; set; }
        
        public int? ParentId { get; set; }
    }
}