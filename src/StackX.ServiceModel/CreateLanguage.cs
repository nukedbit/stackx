using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasPermission(LanguagePermissions.LanguageCreate)]
    public class CreateLanguage : CreateAuditBase<Language, LanguageResponse>
    {
        [ValidateNotEmpty] public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}