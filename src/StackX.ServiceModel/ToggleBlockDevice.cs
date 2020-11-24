using ServiceStack;
using StackX.ServiceModel.Types;
using System.Collections.Generic;

namespace StackX.ServiceModel
{

    [ValidateIsAuthenticated]
    [ValidateIsAdmin]
    public class ToggleBlockDevice : IReturnVoid
    {
        public long Id { get; set; }
    }
}