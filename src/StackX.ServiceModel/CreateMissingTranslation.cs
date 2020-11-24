using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    public class CreateMissingTranslation : CreateAuditBase<Translation, TranslationResponse>
    {
        [ValidateGreaterThan(0)] public int ApplicationId { get; set; }
        [ValidateNotEmpty] public string Key { get; set; }
        [ValidateGreaterThan(0)] public int LanguageId { get; set; } 
    }
}