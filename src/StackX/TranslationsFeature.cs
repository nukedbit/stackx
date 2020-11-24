using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using StackX.ServiceModel;
using StackX.ServiceModel.Types;

namespace StackX.ServiceInterface
{
    public class TranslationsFeature : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            appHost.AssertPlugin<ApplicationsFeature>();
            appHost.AssertPlugin<LanguagesFeature>();

            var container = appHost.GetContainer();
            var connection = container.Resolve<IDbConnectionFactory>();
            var db = connection.CreateDbConnection();
            db.Open();
            db.CreateTableIfNotExists<Translation>();
            Permissions.All.AddRange(TranslationPermissions.All);
            
            StackXAppHostBase.ExcludedAutoCrudTypes.Remove(typeof(UpdateTranslation));
            StackXAppHostBase.ExcludedAutoCrudTypes.Remove(typeof(DeleteTranslation));
            StackXAppHostBase.ExcludedAutoQueryTypes.Remove(typeof(QueryTranslations));
            StackXAppHostBase.ExcludedAutoCrudTypes.Remove(typeof(CreateTranslation));
            StackXAppHostBase.ExcludedAutoCrudTypes.Remove(typeof(CreateMissingTranslation));
            appHost.ServiceController.RegisterService(typeof(TranslationService));
        }
    }
}