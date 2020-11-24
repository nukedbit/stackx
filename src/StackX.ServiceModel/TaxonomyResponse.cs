using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    public class TaxonomyResponse : ICrudResponse<Taxonomy>
    {
        public int Id { get; set; }
        public Taxonomy Result { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}