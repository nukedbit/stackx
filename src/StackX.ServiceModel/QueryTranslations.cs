using System;
using ServiceStack;
using StackX.ServiceModel.Types;
using System.Collections.Generic;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasPermission(TranslationPermissions.TranslationRead)]
    [AutoApply(Behavior.AuditQuery)]
    public class QueryTranslations : QueryDb<Translation>
    {
        public int? Id { get; set; }
        public string Key { get; set; }
        public int? LanguageId { get; set; }
    }

    [ValidateIsAuthenticated]
    [ValidateHasPermission(TranslationPermissions.TranslationRead)]
    [AutoFilter(QueryTerm.Ensure, nameof(AuditBase.DeletedDate), Template = SqlTemplate.IsNotNull)]
    public class QueryDeletedTranslations : QueryDb<Translation>
    {
        public int? Id { get; set; }
        public string Key { get; set; }
        public int? LanguageId { get; set; }
    }

    [ValidateIsAuthenticated]
    [ValidateHasPermission(TranslationPermissions.TranslationRead)]
    public class QueryAllTranslations : QueryDb<Translation>
    {
        public int? Id { get; set; }
        public string Key { get; set; }
        public int? LanguageId { get; set; }
    }

    [ValidateIsAuthenticated]
    [ValidateHasPermission(TranslationPermissions.TranslationRead)]
    public class QueryTranslationsGroupedByLanguage : IReturn<QueryTranslationsGroupedByLanguageResponse>
    {
        public bool ShowDeleted { get; set; }
        public string Key { get; set; }
        public int? ApplicationId { get; set; }
    }

    public class QueryTranslationsGroupedByLanguageResponse : IHasResponseStatus {
        public List<TranslationsGroupedModel> Results {get;set;}

        public ResponseStatus ResponseStatus {get;set;}
    }
}