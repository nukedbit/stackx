using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Web;
using StackX.ServiceModel;
using StackX.ServiceModel.Types;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StackX.ServiceInterface
{
    public class DevicesFeature : IPlugin
    {

        public bool LimitActiveDevicesForUser { get; set; }

        public Func<IRequest, Task<long>> GetUserDeviceCountLimitAsync { get; set; } = (_) => Task.FromResult(1L);

        public void Register(IAppHost appHost)
        {
            var container = appHost.GetContainer();
            var connection = container.Resolve<IDbConnectionFactory>();
            var db = connection.CreateDbConnection();
            db.Open();
            db.CreateTableIfNotExists<DeviceKind>();
            db.CreateTableIfNotExists<Device>();

            var utcNow = DateTime.UtcNow;

            if (db.Count<DeviceKind>() == 0)
            {
                db.Insert(new DeviceKind
                {
                    Name = "Tablet",
                    CreatedBy = "service",
                    ModifiedBy = "service",
                    CreatedDate = utcNow,
                    ModifiedDate = utcNow
                });

                db.Insert(new DeviceKind
                {
                    Name = "Phone",
                    CreatedBy = "service",
                    ModifiedBy = "service",
                    CreatedDate = utcNow,
                    ModifiedDate = utcNow
                });
            }

            StackXAppHostBase.ExcludedAutoQueryTypes.Remove(typeof(RegisterDevice));
            StackXAppHostBase.ExcludedAutoQueryTypes.Remove(typeof(QueryDevices));
            StackXAppHostBase.ExcludedAutoQueryTypes.Remove(typeof(QueryDeviceKinds));
            StackXAppHostBase.ExcludedAutoCrudTypes.Remove(typeof(CreateDeviceKind));
            StackXAppHostBase.ExcludedAutoCrudTypes.Remove(typeof(UpdateDeviceKind));
            StackXAppHostBase.ExcludedAutoCrudTypes.Remove(typeof(DeleteDeviceKind));
            StackXAppHostBase.ExcludedAutoCrudTypes.Remove(typeof(RestoreDeviceKind));
            appHost.ServiceController.RegisterService(typeof(DeviceService));
        }
    }
}