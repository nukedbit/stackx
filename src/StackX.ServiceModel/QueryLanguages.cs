using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasPermission(LanguagePermissions.LanguageRead)]
    [AutoApply(Behavior.AuditQuery)]
    public class QueryLanguages : QueryDb<Language>
    {
        public int? Id { get; set; }
    }
    [ValidateIsAuthenticated]
    [ValidateHasPermission(LanguagePermissions.LanguageRead)]
    public class QueryAllLanguages : QueryDb<Language>
    {
        public int? Id { get; set; }
    }

    [ValidateIsAuthenticated]
    [ValidateHasPermission(LanguagePermissions.LanguageRead)]
    [AutoFilter(QueryTerm.Ensure, nameof(AuditBase.DeletedDate), Template = SqlTemplate.IsNotNull)]
    public class QueryDeletedLanguages : QueryDb<Language>
    {
        public int? Id { get; set; }
    }
}