namespace StackX.ServiceModel.Types
{
    public class TranslationItemModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }

        public bool IsMissing { get;set;}

        public bool IsDeleted { get;set;}
    }
}