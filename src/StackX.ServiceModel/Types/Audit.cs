using System;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace StackX.ServiceModel.Types
{
    [ValidateIsAuthenticated]
    [AutoApply(Behavior.AuditCreate)]
    public abstract class CreateAuditBase<Table, TResponse> : ICreateDb<Table>, IReturn<TResponse> { }

    [ValidateIsAuthenticated]
    [AutoApply(Behavior.AuditModify)]
    public abstract class UpdateAuditBase<Table, TResponse> : IUpdateDb<Table>, IReturn<TResponse> { }


    [ValidateIsAuthenticated]
    [AutoApply(Behavior.AuditSoftDelete)]
    public abstract class SoftDeleteAuditBase<Table, TResponse>
    : IDeleteDb<Table>, IReturn<TResponse>
    {
    }

    [ValidateIsAuthenticated]
    [AutoPopulate(nameof(AuditBase.DeletedDate), Value = null)] 
    [AutoPopulate(nameof(AuditBase.DeletedBy), Value = null)]
    public abstract class RemoveSoftDeleteAuditBase<Table, TResponse>
: IUpdateDb<Table>, IReturn<TResponse>
    {
    }
}
