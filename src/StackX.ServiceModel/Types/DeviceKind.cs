using ServiceStack;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using System;

namespace StackX.ServiceModel.Types
{

    public class DeviceKind : AuditBase {
        [AutoIncrement] public long Id { get; set; }

        [Unique]
        public string Name { get; set; }
    }
}