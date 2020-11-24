using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasPermission(TaxonomyPermissions.TaxonomyRead)]
    [AutoApply(Behavior.AuditQuery)]
    public class QueryTaxonomies : QueryDb<Taxonomy>, IGet
    {
        public int? Id { get; set; }
        public int? ParentId { get; set; }
    }

    [ValidateIsAuthenticated]
    [ValidateHasPermission(TaxonomyPermissions.TaxonomyRead)]
    [AutoFilter(QueryTerm.Ensure, nameof(AuditBase.DeletedDate), Template = SqlTemplate.IsNotNull)]
    public class QueryDeletedTaxonomies : QueryDb<Taxonomy>, IGet
    {
        public int? Id { get; set; }
        public int? ParentId { get; set; }
    }

    [ValidateIsAuthenticated]
    [ValidateHasPermission(TaxonomyPermissions.TaxonomyRead)]
    public class QueryAllTaxonomies : QueryDb<Taxonomy>, IGet
    {
        public int? Id { get; set; }
        public int? ParentId { get; set; }
    }
}