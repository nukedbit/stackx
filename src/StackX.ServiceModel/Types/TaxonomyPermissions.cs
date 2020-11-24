using System.Collections.Generic;

namespace StackX.ServiceModel.Types
{
    public static class TaxonomyPermissions
    {
        public const string TaxonomyRead = "TaxonomyRead";
        public const string TaxonomyCreate = "TaxonomyCreate";
        public const string TaxonomyUpdate = "TaxonomyUpdate";
        public const string TaxonomyDelete = "TaxonomyDelete";
        
        public static readonly List<Permission> All = new List<Permission>()
        {
            new Permission(TaxonomyRead),
            new Permission(TaxonomyCreate),
            new Permission(TaxonomyUpdate),
            new Permission(TaxonomyDelete),
        };
    }
}