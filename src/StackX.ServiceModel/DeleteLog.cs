using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated, ValidateIsAdmin]
    public class DeleteLog : SoftDeleteAuditBase<Log, LogResponse>
    {
        [ValidateGreaterThan(0)] public int Id { get; set; }
    }
}