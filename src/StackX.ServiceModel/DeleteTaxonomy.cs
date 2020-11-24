using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasPermission(TaxonomyPermissions.TaxonomyDelete)]
    public class DeleteTaxonomy : SoftDeleteAuditBase<Taxonomy, TaxonomyResponse>
    {
        [ValidateGreaterThan(0)] public int Id { get; set; }
    }
}