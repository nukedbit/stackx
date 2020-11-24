using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    public class LanguageResponse : ICrudResponse<Language>
    {
        public int Id { get; set; }
        public Language Result { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}