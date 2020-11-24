using System;
using System.Threading.Tasks;
using System.Timers;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.DataAnnotations;
using ServiceStack.Messaging;
using ServiceStack.OrmLite;
using StackX.ServiceModel;
using StackX.ServiceModel.Types;

namespace StackX.ServiceInterface
{
    
    [Route("/mq/delete/log/history")]
    [ExcludeMetadata]
    public class DeleteLogHistory: IReturnVoid
    {
        public int Id { get; set; }
        /// <summary>
        /// Soft Delete logs entries after the specified time span
        /// </summary>
        public TimeSpan SoftDeleteAfter { get; set; }
        /// <summary>
        /// Hard Delete log entries after the specified time span
        /// </summary>
        public TimeSpan HardDeleteAfter { get; set; }
    }
    
    [ExcludeMetadata]
    public class ScheduledDeleteLogHistory: IReturnVoid
    {
        /// <summary>
        /// Soft Delete logs entries after the specified time span
        /// </summary>
        public TimeSpan SoftDeleteAfter { get; set; }
        /// <summary>
        /// Hard Delete log entries after the specified time span
        /// </summary>
        public TimeSpan HardDeleteAfter { get; set; }
    }

    //[RequiredRole("Admin")]
    public class LogsBackgroundService : Service
    {

        public async Task Any(ScheduledDeleteLogHistory request)
        {
            PublishMessage(new DeleteLogHistory()
            {
                Id = 100,
                SoftDeleteAfter = request.SoftDeleteAfter,
                HardDeleteAfter = request.HardDeleteAfter
            });
        }
        
        public async Task Any(DeleteLogHistory request)
        {
            var fromSoftDelete = DateTime.UtcNow.AddMilliseconds(-request.SoftDeleteAfter.TotalMilliseconds);
            var when = DateTime.UtcNow;
            await Db.UpdateOnlyAsync<Log>(() => new Log() {DeletedBy = "service", DeletedDate = when},
                where: l => l.ModifiedDate <= fromSoftDelete);
            
            var fromHardDelete = DateTime.UtcNow.AddMilliseconds(-request.HardDeleteAfter.TotalMilliseconds);

            await Db.DeleteAsync<Log>(l => l.ModifiedDate <= fromHardDelete);
        }
    }
    
    public class LogsOnDbFeature : IPlugin, IDisposable
    {
        /// <summary>
        /// By default we register an auto delete background service to check logs to soft delete, and then delete
        /// </summary>
        public bool RegisterAutoDeleteService { get; set; } = true;
        
        /// <summary>
        /// Soft Delete logs after default is 15 days 
        /// </summary>
        public TimeSpan SoftDeleteAfter { get; set; } = TimeSpan.FromDays(15);
        /// <summary>
        /// Hard delete logs record after default is 30 days
        /// </summary>
        public TimeSpan HardDeleteAfter { get; set; } = TimeSpan.FromDays(30);

        /// <summary>
        /// By Default check and delete or soft delete logs every 24 hours
        /// </summary>
        public TimeSpan LogDeletionCheckSpan = TimeSpan.FromDays(1);

        private Timer _deleteTimer;

        public void Register(IAppHost appHost)
        {
            var container = appHost.GetContainer(); 
            var connection = container.Resolve<IDbConnectionFactory>();
            var db = connection.CreateDbConnection();
            db.Open();
            typeof(Log).GetProperty("LogDate").AddAttributes(new DefaultAttribute(OrmLiteVariables.SystemUtc));
            db.CreateTableIfNotExists<Log>();
            if (!db.ColumnExists<Log>(t => t.LogDate))
            {
                db.AddColumn<Log>(t=> t.LogDate);
            }
 
            StackXAppHostBase.ExcludedAutoCrudTypes.Remove(typeof(CreateLog));
            StackXAppHostBase.ExcludedAutoCrudTypes.Remove(typeof(DeleteLog));
            StackXAppHostBase.ExcludedAutoQueryTypes.Remove(typeof(QueryLogs));

            if (RegisterAutoDeleteService)
            {
                if (SoftDeleteAfter > HardDeleteAfter)
                {
                    throw new ArgumentException("HardDeleteAfter must be great than SoftDeleteAfter");
                }
                
                var mqServer = container.TryResolve<IMessageService>();
                if (mqServer is null)
                {
                    container.Register<IMessageService>(c => new BackgroundMqService());
                    mqServer = container.Resolve<IMessageService>(); 
                    appHost.AfterInitCallbacks.Add(host => {
                        mqServer.Start(); 
                    });
                }
                
                mqServer.RegisterHandler<DeleteLogHistory>(appHost.ExecuteMessage, 1); 
                
                appHost.AfterInitCallbacks.Add(host => {
                    var interval =  LogDeletionCheckSpan.TotalMilliseconds;
                    _deleteTimer = new Timer(interval); 
                    _deleteTimer.Elapsed += (sender, args) =>
                    {
                        HostContext.AppHost.ExecuteService(new ScheduledDeleteLogHistory()
                        {
                            SoftDeleteAfter = SoftDeleteAfter, HardDeleteAfter = HardDeleteAfter
                        });
                    };
                    _deleteTimer.AutoReset = true;
                    _deleteTimer.Enabled = true;
                    _deleteTimer.Start();
                });
            }
        }

        public void Dispose()
        {
            _deleteTimer?.Stop();
            _deleteTimer?.Dispose();
        }
    }
}