using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using StackX.ServiceModel;
using StackX.ServiceModel.Types;

namespace StackX.ServiceInterface
{
    public class TaxonomiesFeature : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            appHost.AssertPlugin<ApplicationsFeature>();

            var container = appHost.GetContainer();
            var connection = container.Resolve<IDbConnectionFactory>();
            var db = connection.CreateDbConnection();
            db.Open();
            db.CreateTableIfNotExists<Taxonomy>();

            if (!db.ColumnExists<Taxonomy>(t => t.Meta))
            {
                db.AddColumn<Taxonomy>(t => t.Meta);
            }

            Permissions.All.AddRange(TaxonomyPermissions.All);
            StackXAppHostBase.ExcludedAutoCrudTypes.Remove(typeof(UpdateTaxonomy));
            StackXAppHostBase.ExcludedAutoCrudTypes.Remove(typeof(DeleteTaxonomy));
            StackXAppHostBase.ExcludedAutoQueryTypes.Remove(typeof(QueryTaxonomies));
            StackXAppHostBase.ExcludedAutoCrudTypes.Remove(typeof(CreateTaxonomy));
        }
    }
}