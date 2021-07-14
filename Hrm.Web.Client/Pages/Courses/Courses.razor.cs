using AntDesign;
using Course.Core.Enums;
using Course.Web.Client.Data;
using Course.Web.Client.Models;
using Course.Web.Client.Shared;
using Course.Web.Share.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Course.Web.Share.Models.ViewModels;
using System.Threading.Tasks;
using AutoMapper;
using Course.Web.Share.Models.EditModels;
using Course.Web.Share.Ultils;
using Course.Core.Extentions;
using Course.Web.Client.Ultils;
using Course.Web.Share;

namespace Course.Web.Client.Pages.Courses
{
    public partial class Courses : ComponentBase
    {
        [CascadingParameter] Error Error { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject] IMapper Mapper { get; set; }
        [Inject] PermissionClaim PermissionClaim { get; set; }
        [Inject] NotificationService Notice { get; set; }

        [Inject] public CoursesAdapterService Service { get; set; }
        [Inject] public UserCoursesAdapterService UserCourseService { get; set; }

        protected LoaiHienThiEnum _displayGrid = LoaiHienThiEnum.None;
        protected List<CoursesViewModel> ListViewCourses;
        protected List<CoursesData> ListCourses;
        protected CoursesEditModel EditModel;


        CoursesData selectCourseViewModel;
        bool DetailVisible { get; set; }
        Page Page { get; set; } = new() { PageIndex = 1, PageSize = 20, Total = 0 };
        ITable table;
        IEnumerable<CoursesViewModel> selectedRows;
        string selectionType = "checkbox";
        bool loading;
        ClaimsPrincipal User;
        string KeyWord { get; set; }


        protected async override Task OnInitializedAsync()
        {
            _displayGrid = LoaiHienThiEnum.Grid;
            User = (await AuthenticationStateTask).User;
            var claims = User?.Claims?.ToList();
            EditModel = new CoursesEditModel();
            ListViewCourses = new List<CoursesViewModel>();
            await LoadDataAsync();


        }
        async Task LoadDataAsync()
        {
            try
            {
                loading = true;
                StateHasChanged();

                var data = await Service.GetByPageAsync(Page, KeyWord);
                if (data == null)
                {
                    return;
                }
                ListCourses = data.Dts;
                Page.Total = data.Total;
                ListViewCourses = Mapper.Map<List<CoursesViewModel>>(ListCourses);
                int stt = Page.PageSize * (Page.PageIndex - 1) + 1;
                ListViewCourses.ForEach(c =>
                {
                    c.Stt = stt++;
                    c.HocPhi = c.HocPhi.IsNotNullOrEmpty() ? (c.HocPhi.ToDecimal().ToDecimalFormated() + "đ") : string.Empty;
                });
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
            loading = false;

        }

        void AddNew()
        {
            EditModel = new CoursesEditModel() { IsActive = true, TuNgay = DateTime.Now, DenNgay = DateTime.Now };
            DetailVisible = true;

        }

        void OpenDetail(CoursesViewModel model)
        {
            if (model.IsNullOrEmpty())
            {
                EditModel = new CoursesEditModel() { IsActive = true, TuNgay = DateTime.Now, DenNgay = DateTime.Now };
            }
            else
            {
                selectCourseViewModel = ListCourses.FirstOrDefault(c => c.Id == model.Id);
                EditModel = Mapper.Map<CoursesEditModel>(selectCourseViewModel);
            }

            DetailVisible = true;
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

        void CloseDetail()
        {
            DetailVisible = false;
        }

        async Task SaveCourseAsync(CoursesEditModel model)
        {
            try
            {
                if (model.Id.IsNotNullOrEmpty())
                {
                    var updateModel = Mapper.Map<CoursesData>(model);
                    var result = await Service.UpdateAsync(updateModel);
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
                    var createModel = Mapper.Map<CoursesData>(model);
                    createModel.Id = ObjectExtentions.GenerateGuid();
                    var result = await Service.AddAsync(createModel);
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

        async Task SearchAsync()
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

        async Task DeleteSelected()
        {
            try
            {
                if (selectedRows.IsNotNullOrEmpty() && selectedRows.Any())
                {
                    var result = await Service.DeleteListAsync(Mapper.Map<List<CoursesData>>(selectedRows));
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
                else
                {
                    Notice.NotiWarning("Chưa có dòng nào được chọn");
                }

            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
        }
        async Task UpadateStatus(string id, bool check)
        {
            try
            {
                var current = ListViewCourses.FirstOrDefault(c => c.Id == id);
                if (current.IsNotNullOrEmpty())
                {
                    current.IsActive = check;
                    var result = await Service.UpdateAsync(Mapper.Map<CoursesData>(current));
                    if (result.State)
                    {
                        Notice.NotiSuccess(AlertResource.UpdateSuccessful);
                        await LoadDataAsync();
                    }
                    else
                    {
                        Notice.NotiError(AlertResource.UpdateFailed);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
        }


    }
}
