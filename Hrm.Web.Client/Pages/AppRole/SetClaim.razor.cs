using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Course.Web.Client.Service;
using Course.Web.Share.Models.ViewModels;
using Course.Web.Share.Ultils;
using Course.Web.Share.Domain;
using AntDesign;
using Course.Web.Client.Ultils;
using Course.Web.Share.IServices;

namespace Course.Web.Client.Pages.AppRole
{
    public partial class SetClaim : ComponentBase
    {
        [Parameter] public EventCallback OnCreateSuccess { get; set; }
        [Parameter] public EventCallback OnCancel { get; set; }
        [Parameter] public SetClaimTypeInterface Target { get; set; } = SetClaimTypeInterface.AppRole;
        [Inject] AppRoleAdapterService RoleService { get; set; }
        [Inject] AppUserAdapterService UserService { get; set; }
        [Inject] NotificationService Notice { get; set; }
        public string RoleId { get; set; }
        public string Name { get; set; }
        List<ClaimViewModel> ClaimTemplates;
        List<ClaimData> claimOfRole;
        List<IGrouping<string, ClaimViewModel>> claimViewModels;
        string[] defaultActiveKeyGroup = new string[0];

        public async Task LoadClaimAsync(string roleId, string name)
        {
            RoleId = roleId;
            Name = name;
            ClaimTemplates = new List<ClaimViewModel>
            {
                new ClaimViewModel{ Id = PermissionKey.ACCOUNT_VIEW, Group = "Quản lý tài khoản", Name ="Xem danh sách"},
                new ClaimViewModel{ Id = PermissionKey.ACCOUNT_ADD, Group = "Quản lý tài khoản", Name ="Thêm mới"},
                new ClaimViewModel{ Id = PermissionKey.ACCOUNT_EDIT, Group = "Quản lý tài khoản", Name ="Chỉnh sửa"},
                new ClaimViewModel{ Id = PermissionKey.ACCOUNT_DELETE, Group = "Quản lý tài khoản", Name ="Xóa"},
                new ClaimViewModel{ Id = PermissionKey.ACCOUNT_SETROLE, Group = "Quản lý tài khoản", Name ="Gán nhóm tài khoản"},
                new ClaimViewModel{ Id = PermissionKey.ACCOUNT_SETCLAIM, Group = "Quản lý tài khoản", Name ="Phân quyền tài khoản"},
                new ClaimViewModel{ Id = PermissionKey.ACCOUNT_CHANGEPASSWORD, Group = "Quản lý tài khoản", Name ="Đổi mật khẩu"},

                new ClaimViewModel{ Id = PermissionKey.ROLE_VIEW, Group = "Quản lý nhóm tài khoản", Name ="Xem danh sách"},
                new ClaimViewModel{ Id = PermissionKey.ROLE_ADD, Group = "Quản lý nhóm tài khoản", Name ="Thêm mới"},
                new ClaimViewModel{ Id = PermissionKey.ROLE_EDIT, Group = "Quản lý nhóm tài khoản", Name ="Chỉnh sửa"},
                new ClaimViewModel{ Id = PermissionKey.ROLE_DELETE, Group = "Quản lý nhóm tài khoản", Name ="Xóa"},
                new ClaimViewModel{ Id = PermissionKey.ROLE_SETCLAIM, Group = "Quản lý nhóm tài khoản", Name ="Phân quyền"},                
                
                new ClaimViewModel{ Id = PermissionKey.COURSE_VIEW, Group = "Quản lý khóa học", Name ="Xem danh sách"},
                new ClaimViewModel{ Id = PermissionKey.COURSE_ADD, Group = "Quản lý khóa học", Name ="Thêm mới"},
                new ClaimViewModel{ Id = PermissionKey.COURSE_DELETE, Group = "Quản lý khóa học", Name ="Chỉnh sửa"},
                new ClaimViewModel{ Id = PermissionKey.COURSE_DELETE, Group = "Quản lý khóa học", Name ="Xóa"},                
                
                new ClaimViewModel{ Id = PermissionKey.LESSON_VIEW, Group = "Quản lý bài học", Name ="Xem danh sách"},
                new ClaimViewModel{ Id = PermissionKey.LESSON_ADD, Group = "Quản lý bài học", Name ="Thêm mới"},
                new ClaimViewModel{ Id = PermissionKey.LESSON_DELETE, Group = "Quản lý bài học", Name ="Chỉnh sửa"},
                new ClaimViewModel{ Id = PermissionKey.LESSON_DELETE, Group = "Quản lý bài học", Name ="Xóa"},


            };
            if (Target == SetClaimTypeInterface.AppRole)
            {
                claimOfRole = await RoleService.GetAllClaimAsync(new AppRoleData { Id = RoleId });
            }
            else
            {
                claimOfRole = await UserService.GetUserClaimsAsync(name);
            }
            ClaimTemplates = ClaimTemplates.GroupJoin(claimOfRole, c => c.Id, cr => cr.Value, (c, cr) => new { c, cr })
                .Select(
                    claim => new ClaimViewModel
                    {
                        Id = claim.c.Id,
                        Checked = IsAdmin() || (claim.cr != null && claim.cr.Any()),
                        Name = claim.c.Name,
                        Group = claim.c.Group
                    }).ToList();
            claimViewModels = ClaimTemplates.GroupBy(c => c.Group).ToList();
            defaultActiveKeyGroup = claimViewModels.Select(c => c.Key).ToArray();
        }

        protected async Task HandleAddRoleValidSubmitAsync()
        {
            ExcuteResponse result = new ExcuteResponse();
            var changedClaims = ClaimTemplates.Where(c => c.Checked).Select(c => c.Id).ToList();
            if (Target == SetClaimTypeInterface.AppRole)
            {
                result = await RoleService.UpdateClaimsAsync(new AppRoleData { Id = RoleId }, changedClaims);
            }
            else
            {
                result = await UserService.UpdateUserClaimsAsync(Name, changedClaims);
            }
            if (result.State)
            {
                Notice.NotiSuccess("Cập nhật quyền thành công.");
                await OnCreateSuccess.InvokeAsync(null);
            }
            else
            {
                Notice.NotiError("Cập nhật thất bại.");
            }
        }

        bool IsAdmin()
        {
            return Name?.ToUpper() == "ADMIN";
        }
    }

}
