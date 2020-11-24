using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasRole(Roles.Admin)]
    public class CreateMenuItem : CreateAuditBase<MenuItem, MenuItemResponse>, IPost
    {
        [ValidateNotEmpty] public string Title { get; set; }
        
        public int? ResourceId { get; set; }
        
        public string Url { get; set; }
        
        [ValidateGreaterThan(0)] public int MenuId { get; set; }
        
        public int? ParentId { get; set; }
    }

    [ValidateIsAuthenticated]
    [ValidateHasRole(Roles.Admin)]
    public class UpdateMenuItem : UpdateAuditBase<MenuItem, MenuItemResponse>, IPut
    {
        [ValidateGreaterThan(0)] public int Id { get; set; }
        [ValidateNotEmpty] public string Title { get; set; }
        
        public int? ResourceId { get; set; }
        
        public string Url { get; set; }
        
        [ValidateGreaterThan(0)] public int MenuId { get; set; }
        
        public int? ParentId { get; set; }
    }

    [ValidateIsAuthenticated]
    [ValidateHasRole(Roles.Admin)]
    public class DeleteMenuItem : IDeleteDb<MenuItem>, IDelete, IReturnVoid
    {
        [ValidateGreaterThan(0)] public int Id { get; set; }
    }

    public class MenuItemResponse
    {
        public int Id { get; set; }
        public MenuItem Result { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [ValidateIsAuthenticated]
    [ValidateHasRole(Roles.Admin)]
    [AutoApply(Behavior.AuditQuery)]
    public class QueryMenuItems : QueryDb<MenuItem>, IGet
    {
        public int? Id { get; set; }
    }
}