using System.Collections.Generic;

namespace StackX.ServiceModel.Types
{
    public class TranslationsGroupedModel
    {
        public string Key { get; set; }
        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public List<TranslationItemModel> Translations { get; set; }
         
    }
}