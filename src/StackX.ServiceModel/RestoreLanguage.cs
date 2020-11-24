using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasPermission(LanguagePermissions.LanguageUpdate)]
    public class RestoreLanguage : RemoveSoftDeleteAuditBase<Language, LanguageResponse>
    {
        [ValidateGreaterThan(0)] public int Id { get; set; }
    }
}