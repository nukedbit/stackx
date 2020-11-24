using System.Data.Common;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.OrmLite;
using StackX.ServiceModel;
using StackX.ServiceModel.Types;

namespace StackX.ServiceInterface
{
    public class DeviceService : Service
    {
        public async Task<object> AnyAsync(RegisterDevice request)
        {
            string userAuthName = GetSession().UserAuthName;
            string userAuthId = GetSession().UserAuthId;
            var utcNow = DateTime.UtcNow;
            var existings = await Db.SelectAsync<Device>(d => d.DeviceId.ToLower() == request.DeviceId.ToLower());
            if (existings.FirstOrDefault() is { } existing)
            {
                if (existing.DeletedDate is not null)
                {
                    return new RegisterDeviceResponse()
                    {
                        DeviceBlocked = true
                    };
                }

                existing.LastActiveBy = userAuthName;
                existing.LastActiveDate = utcNow;
                existing.Meta = request.Meta;
                await Db.SaveAsync(existing);
                return new RegisterDeviceResponse()
                {
                    DeviceBlocked = false
                };
            }

            var deviceFeature = HostContext.AssertPlugin<DevicesFeature>();

            if (deviceFeature.LimitActiveDevicesForUser)
            {
                var limit = await deviceFeature.GetUserDeviceCountLimitAsync(Request);
                var deviceCount = await Db.CountAsync<Device>(d => d.UserAuthId == userAuthId && d.DeletedDate == null);
                if (deviceCount >= limit)
                {
                    throw new HttpError(HttpStatusCode.Forbidden, "Device Limit Reached");
                }
            }

            var device = new Device()
            {
                UserAuthId = userAuthId,
                DeviceId = request.DeviceId,
                Description = request.Description,
                ModifiedBy = userAuthName,
                CreatedBy = userAuthName,
                CreatedDate = utcNow,
                ModifiedDate = utcNow,
                LastActiveBy = userAuthName,
                LastActiveDate = utcNow,
                Meta = request.Meta,
                DeviceKindId = request.DeviceKindId
            };
            await Db.InsertAsync(device);
            return new RegisterDeviceResponse()
            {
                DeviceBlocked = false
            };
        }

        public async Task<object> PostAsync(ToggleBlockDevice request)
        {
            var device = await Db.LoadSingleByIdAsync<Device>(request.Id);
            if (device is null)
            {
                throw new HttpError(HttpStatusCode.NotFound, "device not found");
            }

            if (device.DeletedDate is null)
            {
                device.DeletedDate = DateTime.UtcNow;
            }
            else
            {
                device.DeletedDate = null;
            }
            await Db.SaveAsync(device);

            return new HttpResult(HttpStatusCode.Accepted, "OK");
        }
    }
}