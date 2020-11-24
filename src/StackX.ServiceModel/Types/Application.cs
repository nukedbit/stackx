using ServiceStack;
using ServiceStack.DataAnnotations;

namespace StackX.ServiceModel.Types
{
    public class Application : AuditBase
    {
        [AutoIncrement] public int Id { get; set; }

        [Unique] public string Name { get; set; }


        public override string ToString()
        {
            return Name;
        }
    }
}