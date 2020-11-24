using System;
using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated, ValidateIsAdmin]
    public class CreateLog : CreateAuditBase<Log, LogResponse>
    {
        [ValidateNotEmpty, ValidateNotNull] public string Content { get; set; }
        
        public LogLevel Level { get; set; }
        
        public int? ApplicationId { get; set; }
        
        public string Tag { get; set; }
        
        [AutoDefault(Eval = "utcNow")]
        public DateTime LogDate { get; set; }
        
        public string CallStack { get; set; }
        
        [AutoDefault(Value = Types.DevPlatform.Other)]
        public DevPlatform DevPlatform { get; set; }
    }
}