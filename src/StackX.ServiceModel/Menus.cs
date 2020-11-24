using ServiceStack;
using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    [ValidateIsAuthenticated]
    [ValidateHasRole(Roles.Admin)]
    public class CreateMenu : CreateAuditBase<Menu, MenuResponse>, IPost
    {
        [ValidateNotEmpty] public string Title { get; set; }
        [ValidateNotEmpty] public string Key { get; set; }
    }

    [ValidateIsAuthenticated]
    [ValidateHasRole(Roles.Admin)]
    public class UpdateMenu : UpdateAuditBase<Menu, MenuResponse>, IPut
    {
        [ValidateGreaterThan(0)] public int Id { get; set; }
        [ValidateNotEmpty] public string Title { get; set; }
        [ValidateNotEmpty] public string Key { get; set; }
    }

    [ValidateIsAuthenticated]
    [ValidateHasRole(Roles.Admin)]
    public class DeleteMenu : SoftDeleteAuditBase<Menu, MenuResponse>
    {
        [ValidateGreaterThan(0)] public int Id { get; set; }
    }

    public class MenuResponse : ICrudResponse<Menu>
    {
        public int Id { get; set; }
        public Menu Result { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [ValidateIsAuthenticated]
    [ValidateHasRole(Roles.Admin)]
    [AutoApply(Behavior.AuditQuery)]
    public class QueryMenus : QueryDb<Menu>, IGet
    {
        public int? Id { get; set; }
    }
}