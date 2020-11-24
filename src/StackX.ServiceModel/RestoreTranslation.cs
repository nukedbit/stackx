using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasPermission(TranslationPermissions.TranslationUpdate)]
    public class RestoreTranslation : RemoveSoftDeleteAuditBase<Translation, TranslationResponse>
    {
        [ValidateGreaterThan(0)] public int Id { get; set; }
    }
}