using System;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using StackX.ServiceModel;
using StackX.ServiceModel.Types;

namespace StackX.ServiceInterface
{
    public class ApplicationsFeature : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            var container = appHost.GetContainer();
            var connection = container.Resolve<IDbConnectionFactory>();
            var db = connection.CreateDbConnection();
            db.Open();
            if (db.CreateTableIfNotExists<Application>())
            {
                db.Insert(new Application()
                {
                    Name = "Admin",
                    CreatedBy = "Migration",
                    ModifiedBy = "Migration",
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                });
            }
            
            Permissions.All.AddRange(ApplicationsPermissions.All);
            StackXAppHostBase.ExcludedAutoCrudTypes.Remove(typeof(UpdateApplication));
            StackXAppHostBase.ExcludedAutoCrudTypes.Remove(typeof(DeleteApplication));
            StackXAppHostBase.ExcludedAutoQueryTypes.Remove(typeof(QueryApplications));
            StackXAppHostBase.ExcludedAutoCrudTypes.Remove(typeof(CreateApplication));
        }
    }
}