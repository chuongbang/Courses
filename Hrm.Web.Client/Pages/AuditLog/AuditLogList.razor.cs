using AntDesign;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Course.Web.Client.Models;
using Course.Web.Client.Pages.AuditLog;
using Course.Web.Client.Service;
using Course.Web.Share.Domain;
using Course.Web.Share.Models.EditModels;
using Course.Web.Share.Models.ViewModels;
using Course.Web.Share;
using Course.Web.Share.Ultils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Course.Web.Client.Shared;
using Course.Core.Extentions;
using Course.Web.Client.Ultils;
using FluentNHibernate.Testing.Values;
using Course.Web.Share.IServices;

namespace Course.Web.Client.Pages.AuditLog
{
    public partial class AuditLogList: ComponentBase
    {
        [Inject] AuditLogAdapterService AuditLogService { get; set; }
        [Inject] IMapper Mapper { get; set; }
        [Inject] PermissionClaim PermissionClaim { get; set; }

        [CascadingParameter] Error Error { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationStateTask { get; set; }

        AuditLogFilterEditModel AuditLogFilterEditModel { get; set; }
        List<AuditLogViewModel> AuditLogViewModels { get; set; }
        List<AuditLogData> AuditLogDatas { get; set; }
        bool DetailVisible { get; set; }
        List<AuditLogDetailViewModel> auditLogDetailViewModels;
        AuditLogData selectAuditLogViewModel;
        List<Claim> claims;
        bool loading;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                AuditLogFilterEditModel = new AuditLogFilterEditModel();
                AuditLogFilterEditModel.Page = new() { PageIndex = 1, PageSize = 10, Total = 0 };
                claims = (await AuthenticationStateTask).User?.Claims?.ToList();
                auditLogDetailViewModels = new List<AuditLogDetailViewModel>();
                
                if (PermissionClaim.AUDITLOG_VIEW)
                {
                    await LoadDataAsync();
                }
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
                var auditLogSearch = Mapper.Map<AuditLogSearch>(AuditLogFilterEditModel);
                var rolePage = await AuditLogService.GetPageAsync(auditLogSearch);
                if (rolePage == null)
                {
                    return;
                }
                AuditLogDatas = rolePage.Data;
                AuditLogFilterEditModel.Page.Total = rolePage.Total;
                AuditLogViewModels = Mapper.Map<List<AuditLogViewModel>>(AuditLogDatas);
                int stt = AuditLogFilterEditModel.Page.PageSize * (AuditLogFilterEditModel.Page.PageIndex - 1) + 1;
                AuditLogViewModels.ForEach(c =>
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
                AuditLogFilterEditModel.Page.PageIndex = e.Page;
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
        }

        async Task PageSizeChangeAsync(PaginationEventArgs e)
        {
            try
            {
                AuditLogFilterEditModel.Page.PageIndex = 1;
                AuditLogFilterEditModel.Page.PageSize = e.PageSize;
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
        }

        async Task SearchAsync()
        {
            try
            {
                AuditLogFilterEditModel.Page.PageIndex = 1;
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
        }

        void OpenDetail(AuditLogViewModel model)
        {
            try
            {
                selectAuditLogViewModel = AuditLogDatas.FirstOrDefault(c => c.Id == model.Id);
                auditLogDetailViewModels = Mapper.Map<List<AuditLogDetailViewModel>>(selectAuditLogViewModel.AuditLogDetails);
                int stt = 1;
                auditLogDetailViewModels.ForEach(c =>
                {
                    c.Stt = stt++;
                });
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

        async Task CLearFilterAsync()
        {
            AuditLogFilterEditModel.ClearValue();
            await SearchAsync();
        }
    }
}
