using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasPermission(LanguagePermissions.LanguageDelete)]
    public class DeleteLanguage : SoftDeleteAuditBase<Language, LanguageResponse>
    {
        [ValidateGreaterThan(0)] public int Id { get; set; }
    }
}