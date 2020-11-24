using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    public class TranslationResponse : ICrudResponse<Translation>
    {
        public int Id { get; set; }
        public Translation Result { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}