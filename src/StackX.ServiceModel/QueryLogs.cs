using System;
using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [AutoApply(Behavior.AuditQuery)]
    public class QueryLogs : QueryDb<Log>
    {

        public int? Id { get; set; }
        
        public int? ApplicationId { get; set; }

        public string ContentContains { get; set; }
        
        public LogLevel? Level { get; set; }
        
        public DevPlatform? DevPlatform { get; set; }
     
        public string Tag { get; set; } 
        
        public DateTime? LogDateGreaterThanOrEqualTo { get; set; }
    }
}