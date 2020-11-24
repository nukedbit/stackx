using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace StackX.ServiceModel.Types
{
    public class Language : AuditBase, IHasIntId
    {
        [AutoIncrement] public int Id { get; set; }
        [Unique]
        public string Name { get; set; }

        [Default(0)]
        public bool IsDefault { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}