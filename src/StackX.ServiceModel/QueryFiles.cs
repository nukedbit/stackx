using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateRequest(Condition = "HasRole('Admin') or HasRole('Contributor')")]
    [AutoApply(Behavior.AuditQuery)]
    public class QueryFiles : QueryDb<ServiceModel.Types.File>
    {
        public int[] Ids { get; set; }
        public string ExtraAttribute1 { get; set; }
        public string ExtraAttribute2 { get; set; }
        public string ExtraAttribute3 { get; set; }
        public string ExtraAttribute4 { get; set; }
    }

    [ValidateIsAuthenticated]
    [ValidateRequest(Condition = "HasRole('Admin') or HasRole('Contributor')")]
    [AutoFilter(QueryTerm.Ensure, nameof(AuditBase.DeletedDate), Template = SqlTemplate.IsNotNull)]
    public class QueryDeletedFiles : QueryDb<ServiceModel.Types.File>
    {
        public int[] Ids { get; set; }
        public string ExtraAttribute1 { get; set; }
        public string ExtraAttribute2 { get; set; }
        public string ExtraAttribute3 { get; set; }
        public string ExtraAttribute4 { get; set; }
    }

    [ValidateIsAuthenticated]
    [ValidateRequest(Condition = "HasRole('Admin') or HasRole('Contributor')")]
    public class QueryAllFiles : QueryDb<ServiceModel.Types.File>
    {
        public int[] Ids { get; set; }
        public string ExtraAttribute1 { get; set; }
        public string ExtraAttribute2 { get; set; }
        public string ExtraAttribute3 { get; set; }
        public string ExtraAttribute4 { get; set; }
    }
}