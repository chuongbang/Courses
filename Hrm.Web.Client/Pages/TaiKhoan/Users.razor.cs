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
using System.Security.Claims;
using Course.Web.Share.Ultils;
using Course.Web.Client.Pages.AppRole;

namespace Course.Web.Client.Pages.TaiKhoan
{
    public partial class Users : ComponentBase
    {
        [Inject] AppUserAdapterService UserService { get; set; }
        [Inject] IMapper Mapper { get; set; }
        [Inject] NotificationService Notice { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] PermissionClaim PermissionClaim { get; set; }

        [CascadingParameter] Error Error { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationStateTask { get; set; }

        List<AppUserViewModel> AppUserViewModels { get; set; }
        List<AppUserData> AppUserDatas { get; set; }
        bool DetailVisible { get; set; }
        public string Search { get; set; }
        Page Page { get; set; } = new() { PageIndex = 1, PageSize = 10, Total = 0 };
        bool loading;
        AppUserData selectAppUserViewModel;
        AppUserDetail appUserDetail;
        ResetPasswordModel resetPasswordModel;
        bool changePasswordVisible = false;
        SetRole setRoleComponent;
        bool setRoleVisible = false;
        ClaimsPrincipal User;
        SetClaim setClaimComponent;
        bool setClaimVisible;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                User = (await AuthenticationStateTask).User;
                var claims = User?.Claims?.ToList();
                resetPasswordModel = new ResetPasswordModel();
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
                var userPage = await UserService.GetPageAsync(Search, Page);
                if (userPage == null)
                {
                    return;
                }
                AppUserDatas = userPage.Data;
                Page.Total = userPage.Total;
                AppUserViewModels = Mapper.Map<List<AppUserViewModel>>(AppUserDatas);
                int stt = Page.PageSize * (Page.PageIndex - 1) + 1;
                AppUserViewModels.ForEach(c =>
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

        void OpenDetail(AppUserViewModel model)
        {
            try
            {
                selectAppUserViewModel = AppUserDatas.FirstOrDefault(c => c.Id == model.Id);
                appUserDetail.LoadEditModel(selectAppUserViewModel, readOnly: !PermissionClaim.ACCOUNT_EDIT);
                if (IsAdmin(model) || IsCurrentAccount(model))
                {
                    appUserDetail.DisableField();
                }
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
                selectAppUserViewModel = new AppUserData();
                appUserDetail.LoadEditModel(selectAppUserViewModel, false);
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

        async Task SaveDetailAsync(AppUserEditModel model)
        {
            try
            {
                if (model.Id.IsNotNullOrEmpty())
                {
                    var updateModel = Mapper.Map<AppUserData>(model);
                    var result = await UserService.UpdateAsync(updateModel);
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
                    var createModel = Mapper.Map<AppUserData>(model);
                    var result = await UserService.AddAsync(createModel);
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

        async Task OpenSetRoleAsync(AppUserViewModel model)
        {
            try
            {
                if (IsCurrentAccount(model))
                {
                    Notice.NotiWarning("Không thể tự gán nhóm cho tài khoản của bạn");
                    return;
                }
                await setRoleComponent.LoadRoleAsync(model.Id, model.UserName);
                setRoleVisible = true;
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
        }

        async Task DeleteAsync(AppUserViewModel model)
        {
            try
            {
                if (IsAdmin(model))
                {
                    Notice.NotiWarning("Tài khoản không được phép xóa");
                    return;
                }
                if (IsCurrentAccount(model))
                {
                    Notice.NotiWarning("Không thể xóa tài khoản của chính bạn");
                    return;
                }
                var result = await UserService.DeleteAsync(Mapper.Map<AppUserData>(model), User.Identity.Name);
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

        void OpenChangePasswordForm(AppUserViewModel model)
        {
            if (IsAdmin(model) && !IsCurrentAccount(model))
            {
                Notice.NotiWarning("Không thể đổi mật khẩu tài khoản quản trị");
                return;
            }
            resetPasswordModel = new() { UserName = model.UserName, Id = model.Id };
            changePasswordVisible = true;
        }

        async Task OnPasswordChangedAsync(ResetPasswordModel model)
        {
            changePasswordVisible = false;
            try
            {
                var result = await UserService.ResetPasswordAsync(model.Id, model.Password);
                if (result.State)
                {
                    CloseChangePassword();
                    Notice.NotiSuccess(AlertResource.ChangePasswordSuccessful);
                    await LoadDataAsync();
                }
                else
                {
                    Notice.NotiError(AlertResource.ChangePasswordFailed);
                }
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }


        }

        void CloseChangePassword()
        {
            changePasswordVisible = false;
        }

        void CloseSetRole()
        {
            setRoleVisible = false;
        }

        void AppRoleChanged()
        {
            Notice.NotiSuccess("Cập nhật quyền cho tài khoản thành công");
        }

        bool IsAdmin(AppUserViewModel model)
        {
            return model?.UserName?.ToUpper() == "ADMIN";
        }

        bool IsCurrentAccount(AppUserViewModel model)
        {
            return model?.UserName?.ToUpper() == User.Identity.Name?.ToUpper();
        }

        async Task OpenSetClaimAsync(AppUserViewModel model)
        {
            try
            {
                if (IsCurrentAccount(model))
                {
                    Notice.NotiWarning("Không thể tự phân quyền cho tài khoản của bạn");
                    return;
                }
                await setClaimComponent.LoadClaimAsync(null, model.UserName);
                setClaimVisible = true;
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
