using System;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using StackX.ServiceModel;
using StackX.ServiceModel.Types;

namespace StackX.ServiceInterface
{
    public class LanguagesFeature : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            var container = appHost.GetContainer(); 
            var connection = container.Resolve<IDbConnectionFactory>();
            var db = connection.CreateDbConnection();
            db.Open();
            if (db.CreateTableIfNotExists<Language>())
            {
                var englishId = db.Insert(new Language
                {
                    Name = "English",
                    CreatedBy = "Migration",
                    ModifiedBy = "Migration",
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                });
                var italianId = db.Insert(new Language
                {
                    Name = "Italian",
                    CreatedBy = "Migration",
                    ModifiedBy = "Migration",
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                });
            }

            if (!db.ColumnExists<Language>(l => l.IsDefault))
            {
                db.AddColumn<Language>(l => l.IsDefault);
            }
            
            Permissions.All.AddRange(LanguagePermissions.All);
 
            StackXAppHostBase.ExcludedAutoCrudTypes.Remove(typeof(UpdateLanguage));
            StackXAppHostBase.ExcludedAutoCrudTypes.Remove(typeof(DeleteLanguage));
            StackXAppHostBase.ExcludedAutoQueryTypes.Remove(typeof(QueryLanguages));
            StackXAppHostBase.ExcludedAutoCrudTypes.Remove(typeof(CreateLanguage));
        }
    }
}