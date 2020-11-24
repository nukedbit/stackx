using ServiceStack;
using System;
using System.Data.Common;
using ServiceStack.DataAnnotations;

namespace StackX.ServiceModel.Types
{
    [EnumAsInt]
    public enum DevPlatform : int
    {
        DotNet = 10,
        Swift = 20,
        TypeScript = 30,
        JavaScript = 40,
        Other = 500
    }
    
    public class Log : AuditBase
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string Content { get; set; }
        [Index]
        public LogLevel Level { get; set; }
        [References(typeof(Application))]
        public int? ApplicationId { get; set; }
        [Index]
        public string Tag { get; set; }
        [Index]
        public DateTime LogDate { get; set; }
        
        public string CallStack { get; set; }
        
        [Index]
        public DevPlatform DevPlatform { get; set; }
    }
}