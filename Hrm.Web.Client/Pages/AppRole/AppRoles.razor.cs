using AntDesign;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Components;
using Course.Web.Share.Models.ViewModels;
using Course.Web.Client.Shared;
using Course.Web.Client.Service;
using Course.Web.Share.Domain;
using System.Linq;
using Course.Web.Client.Ultils;
using Course.Web.Share;
using Course.Web.Share.Models.EditModels;
using Course.Core.Extentions;
using Microsoft.AspNetCore.Components.Authorization;
using Course.Web.Client.Models;
using Course.Web.Share.Ultils;
using Course.Web.Client.Pages.TaiKhoan;
using System.Security.Claims;

namespace Course.Web.Client.Pages.AppRole
{
    public partial class AppRoles : ComponentBase
    {
        [Inject] AppRoleAdapterService RoleService { get; set; }
        [Inject] IMapper Mapper { get; set; }
        [Inject] NotificationService Notice { get; set; }
        [Inject] PermissionClaim PermissionClaim { get; set; }
        [CascadingParameter] Error Error { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationStateTask { get; set; }

        List<AppRoleViewModel> AppRoleViewModels { get; set; }
        List<AppRoleData> AppRoleDatas { get; set; }
        bool DetailVisible { get; set; }
        public string Search { get; set; }
        Page Page { get; set; } = new() { PageIndex = 1, PageSize = 10, Total = 0 };
        bool loading;
        AppRoleData selectAppRoleViewModel;
        AppRoleDetail appRoleDetail;
        SetClaim setClaimComponent;
        bool setClaimVisible;
        ClaimsPrincipal User;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                User = (await AuthenticationStateTask).User;
                var claims = User?.Claims?.ToList();
                
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
        }

        async Task LoadDataAsync()
        {
            try
            {
                loading = true;
                StateHasChanged();
                var rolePage = await RoleService.GetPageAsync(Search, Page);
                if (rolePage == null)
                {
                    return;
                }
                AppRoleDatas = rolePage.Data;
                Page.Total = rolePage.Total;
                AppRoleViewModels = Mapper.Map<List<AppRoleViewModel>>(AppRoleDatas);
                int stt = Page.PageSize * (Page.PageIndex - 1) + 1;
                AppRoleViewModels.ForEach(c =>
                {
                    c.Stt = stt++;
                });
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
            loading = false;
        }

        async Task PageIndexChangeAsync(PaginationEventArgs e)
        {
            try
            {
                Page.PageIndex = e.Page;
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }

        }

        async Task SearchAsync(string search)
        {
            try
            {
                Page.PageIndex = 1;
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
        }

        void OpenDetail(AppRoleViewModel appRoleViewModel)
        {
            try
            {
                selectAppRoleViewModel = AppRoleDatas.FirstOrDefault(c => c.Id == appRoleViewModel.Id);
                appRoleDetail.LoadEditModel(selectAppRoleViewModel, readOnly: !PermissionClaim.ROLE_EDIT);
                DetailVisible = true;
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
        }

        void Add()
        {
            try
            {
                selectAppRoleViewModel = new AppRoleData();
                appRoleDetail.LoadEditModel(selectAppRoleViewModel, false);
                DetailVisible = true;
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
        }

        void CloseDetail()
        {
            try
            {
                DetailVisible = false;
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
        }

        async Task SaveDetailAsync(AppRoleEditModel model)
        {
            try
            {
                if (model.Id.IsNotNullOrEmpty())
                {
                    var updateModel = Mapper.Map<AppRoleData>(model);
                    var result = await RoleService.UpdateAsync(updateModel);
                    if (result.State)
                    {
                        CloseDetail();
                        Notice.NotiSuccess(AlertResource.UpdateSuccessful);
                        await LoadDataAsync();
                    }
                    else
                    {
                        Notice.NotiError(AlertResource.InvalidData);
                    }
                }
                else
                {
                    var createModel = Mapper.Map<AppRoleData>(model);
                    createModel.NormalizedName = createModel.Name.ToUpper();
                    var result = await RoleService.AddAsync(createModel);
                    if (result.State)
                    {
                        CloseDetail();
                        Notice.NotiSuccess(AlertResource.AddSuccessful);
                        await LoadDataAsync();
                    }
                    else
                    {
                        Notice.NotiError(AlertResource.InvalidData);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
        }

        async Task OpenSetClaimAsync(AppRoleViewModel appRoleViewModel)
        {
            try
            {
                await setClaimComponent.LoadClaimAsync(appRoleViewModel.Id, appRoleViewModel.Name);
                setClaimVisible = true;
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
        }

        async Task DeleteAsync(AppRoleViewModel model)
        {
            try
            {
                if (model.Name.ToUpper() == "ADMIN")
                {
                    Notice.NotiWarning("Nhóm tài khoản không được phép xóa");
                    return;
                }
                var result = await RoleService.DeleteAsync(model.Id);
                if (result.State)
                {
                    Notice.NotiSuccess(AlertResource.DeleteSuccessful);
                    await LoadDataAsync();
                }
                else
                {
                    Notice.NotiError(AlertResource.DeleteFailed);
                }
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }

        }

        void CloseSetClaim()
        {
            setClaimVisible = false;
        }

        void ClaimChanged()
        {
            setClaimVisible = false;
        }

    }
}
