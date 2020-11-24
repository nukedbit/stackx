using System.Collections.Generic;

namespace StackX.ServiceModel
{
    public static class TranslationPermissions
    {
        public const string TranslationRead = "TranslationRead";
        public const string TranslationCreate = "TranslationCreate";
        public const string TranslationUpdate = "TranslationUpdate";
        public const string TranslationDelete = "TranslationDelete";
        
        public static readonly List<Permission> All = new List<Permission>()
        {
            new Permission(TranslationRead),
            new Permission(TranslationCreate),
            new Permission(TranslationUpdate),
            new Permission(TranslationDelete),
        };
    }
}