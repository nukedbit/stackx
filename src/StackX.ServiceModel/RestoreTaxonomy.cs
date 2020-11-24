using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasPermission(TaxonomyPermissions.TaxonomyUpdate)]
    public class RestoreTaxonomy : RemoveSoftDeleteAuditBase<Taxonomy, TaxonomyResponse>
    {
        [ValidateGreaterThan(0)] public int Id { get; set; }
    }
}