using ServiceStack;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace StackX.ServiceModel.Types
{
    [Alias("menu")]
    public class Menu : AuditBase
    {
        [AutoIncrement] public int Id { get; set; }

        [Required] public string Title { get; set; }
        
        [Required][Unique] public string Key { get; set; }
        
        [Reference] public List<MenuItem> MenuItems { get; set; }
    }
}