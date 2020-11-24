using System.Collections.Generic;

namespace StackX.ServiceModel.Types
{
    public static class LanguagePermissions
    {
        public const string LanguageRead = "LanguageRead";
        public const string LanguageCreate = "LanguageCreate";
        public const string LanguageUpdate = "LanguageUpdate";
        public const string LanguageDelete = "LanguageDelete";
        
        public static readonly List<Permission> All = new List<Permission>()
        {
            new Permission(LanguageRead),
            new Permission(LanguageCreate),
            new Permission(LanguageUpdate),
            new Permission(LanguageDelete),
        };
    }
}