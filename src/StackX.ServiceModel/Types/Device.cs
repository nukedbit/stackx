using ServiceStack;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using System;

namespace StackX.ServiceModel.Types
{
    public class Device : AuditBase
    {
        [AutoIncrement] public long Id { get; set; }

        [StringLength(5, 100)]
        public string Description { get; set; }

        [Unique]
        [StringLength(10, 100)]
        public string DeviceId {get;set;}

        public string UserAuthId {get;set;}


        public DateTime LastActiveDate { get; set; }

        public string LastActiveBy { get; set; }

        public Dictionary<string, string> Meta {get;set;}

        [References(typeof(DeviceKind))]
        public long DeviceKindId{get; set;}
    }
}