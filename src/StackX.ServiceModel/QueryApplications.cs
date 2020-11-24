using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasPermission(ApplicationsPermissions.ApplicationRead)]
    [AutoApply(Behavior.AuditQuery)]
    public class QueryApplications : QueryDb<Application>
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }

    [ValidateIsAuthenticated]

    [ValidateHasPermission(ApplicationsPermissions.ApplicationRead)]
    [AutoFilter(QueryTerm.Ensure, nameof(AuditBase.DeletedDate), Template = SqlTemplate.IsNotNull)]
    public class QueryDeletedApplications : QueryDb<Application>
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }
    [ValidateIsAuthenticated]
    [ValidateHasPermission(ApplicationsPermissions.ApplicationRead)]
    public class QueryAllApplications : QueryDb<Application>
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }
}