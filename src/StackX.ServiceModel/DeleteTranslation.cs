using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasPermission(TranslationPermissions.TranslationDelete)]
    public class DeleteTranslation : SoftDeleteAuditBase<Translation, TranslationResponse>
    {
        [ValidateGreaterThan(0)] public int Id { get; set; }
    }
}