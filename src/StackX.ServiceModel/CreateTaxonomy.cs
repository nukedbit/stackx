using ServiceStack;
using StackX.ServiceModel.Types;
using System.Collections.Generic;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasPermission(TaxonomyPermissions.TaxonomyCreate)]
    public class CreateTaxonomy : CreateAuditBase<Taxonomy, TaxonomyResponse>
    {
        [ValidateNotEmpty] public string Name { get; set; }
        [ValidateGreaterThan(0)] public int ApplicationId { get; set; }
        [ValidateGreaterThan(0)] public int? ParentId { get; set; }
        
        public Dictionary<string, string> Meta { get; set; }
    }
}