using ServiceStack;
using StackX.ServiceModel.Types;
using System.Collections.Generic;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasPermission(TaxonomyPermissions.TaxonomyUpdate)]
    public class UpdateTaxonomy : UpdateAuditBase<Taxonomy, TaxonomyResponse>
    {
        [ValidateGreaterThan(0)] public int Id { get; set; }
        [ValidateNotEmpty] public string Name { get; set; }
        [ValidateGreaterThan(0)] public int ApplicationId { get; set; }
        [ValidateGreaterThan(0)] public int? ParentId { get; set; }

        public Dictionary<string, string> Meta { get; set; }
    }
}