using Course.Web.Share.Domain;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course.Web.Client.Service;
using Course.Web.Share.Models.ViewModels;
using Course.Web.Share.Ultils;

namespace Course.Web.Client.Pages.TaiKhoan
{
    public partial class SetRole : ComponentBase
    {
        [Inject] AppRoleAdapterService RoleService { get; set; }
        [Inject] AppUserAdapterService UserService { get; set; }
        [Parameter] public EventCallback OnCreateSuccess { get; set; }
        [Parameter] public EventCallback CancelChanged { get; set; }

        string UserId { get; set; }
        public string UserName { get; private set; }

        List<AppRoleData> Roles { get; set; }
        List<AppRoleViewModel> ChooseUserRoles { get; set; }
        List<AppRoleData> UserRoles { get; set; }
        AppRoleData Role = new AppRoleData();

        async Task<List<AppRoleData>> QueryDataAsync() => await UserService.GetRoleByUserAsync(UserId);

        void GetUserRoleData(List<AppRoleData> appRoles)
        {
            UserRoles = appRoles ??= new List<AppRoleData>();
            if (Roles?.Any() == true && UserRoles != null)
            {
                ChooseUserRoles = Roles.GroupJoin(UserRoles, r => r.Id, u => u.Id, (r, u) => new { r, u })
                    .Select(
                        role => new AppRoleViewModel
                        {
                            Id = role.r.Id,
                            Checked = role.u != null && role.u.Any(),
                            Name = role.r.Name,
                            NormalizedName = role.r.NormalizedName,
                            Description = role.r.Description
                        }
                    ).ToList();
            }

        }

        public async Task LoadRoleAsync(string userId, string userName)
        {
            UserId = userId;
            UserName = userName;
            Roles = await RoleService.GetAllAsync();
            GetUserRoleData(await QueryDataAsync());
        }

        async Task HandleAddRoleValidSubmitAsync()
        {
            var changedRoles = ChooseUserRoles.Where(c => c.Checked).Select(c => new AppRoleData { Id = c.Id, Name = c.Name, NormalizedName = c.NormalizedName }).ToList();
            List<AppRoleData> updateRoles = await UserService.ChangeRolesAsync(changedRoles, UserId);

            GetUserRoleData(updateRoles);
            await OnCreateSuccess.InvokeAsync(null);

        }

    }
}
