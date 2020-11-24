using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasPermission(LanguagePermissions.LanguageUpdate)]
    public class UpdateLanguage : UpdateAuditBase<Language, LanguageResponse>
    {
        [ValidateGreaterThan(0)] public int Id { get; set; }
        [ValidateNotEmpty] public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}