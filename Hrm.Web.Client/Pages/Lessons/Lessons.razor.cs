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
using Course.Core.Ultis;
using Course.Core.Data;
using Microsoft.JSInterop;

namespace Course.Web.Client.Pages.Lessons
{
    public partial class Lessons : ComponentBase
    {
        [CascadingParameter] Error Error { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject] IMapper Mapper { get; set; }
        [Inject] PermissionClaim PermissionClaim { get; set; }
        [Inject] NotificationService Notice { get; set; }
        [Inject] IJSRuntime JsRuntime { get; set; }

        [Inject] public LessonsAdapterService Service { get; set; }
        [Inject] public CoursesAdapterService CoursesService { get; set; }

        protected List<LessonsViewModel> ListViewLessons;
        protected List<LessonsData> ListLessons;
        protected List<CoursesData> ListCourses;
        protected LessonsEditModel EditModel;

        ITable table;
        bool loading;
        bool DetailVisible { get; set; }
        Page Page { get; set; } = new() { PageIndex = 1, PageSize = 15, Total = 0 };
        ClaimsPrincipal User;
        string KeyWord { get; set; }
        Property<LessonsEditModel> property;
        List<LessonsData> ListAllLesson;
        string _quillHtmlContent;
        public string QuillHtmlContent
        {
            get => _quillHtmlContent;
            set
            {
                if (value != null)
                {
                    _quillHtmlContent = value;
                }
            }
        }
        bool render = false;    


        protected async override Task OnInitializedAsync()
        {
            User = (await AuthenticationStateTask).User;
            var claims = User?.Claims?.ToList();
            EditModel = new LessonsEditModel();
            property = new Property<LessonsEditModel>();
            ListViewLessons = new List<LessonsViewModel>();
            ListCourses = (await CoursesService.GetAllActiveAsync()).Dts;
            await LoadDataAsync();

        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);    
        }

        async Task LoadDataAsync()
        {
            try
            {
                loading = true;
                StateHasChanged();
                var ListAll = await CoursesService.GetCoursesActiveWithLessonsAsync(Page, KeyWord);
                Page.Total = ListAll.Total;
                int Stt = Page.PageSize * (Page.PageIndex - 1) + 1;
                ListAllLesson = ListAll.LDatas;
                ListViewLessons.Clear();
                if (ListAll.CDatas.IsNotNullOrEmpty())
                {
                    foreach (var cs in ListAll.CDatas)
                    {
                        var listLs = ListAll.LDatas?.Where(c => c.KhoaHocId == cs.Id).ToList();
                        LessonsViewModel ls = new LessonsViewModel();
                        ls.Update(cs);
                        ls.Stt = Stt++;
                        ls.SetDataList(listLs, 1);
                        ListViewLessons.Add(ls);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
            loading = false;
        }

        void AddNew()
        {
            EditModel = new LessonsEditModel(listLS: ListCourses);

            ShowDrawer();
        }
        void Edit(string id)
        {
            var current = ListAllLesson.FirstOrDefault(c => c.Id == id);
            EditModel = Mapper.Map<LessonsEditModel>(current);
            EditModel.DataSource[property.Name(c => c.KhoaHocId)] = ListCourses.ToDictionary(c => c.Id, v => (ISelectItem)v);
            ShowDrawer();
        }
        void ShowDrawer()
        {
            render = true;
            DetailVisible = true;
            StateHasChanged();
        }
        async Task Delete(string id)
        {
            try
            {
                var current = ListAllLesson.FirstOrDefault(c => c.Id == id);

                var result = await Service.DeleteAsync(current);
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

        async Task SaveLessonAsync(LessonsEditModel model)
        {
            try
            {
                model.NoiDung = _quillHtmlContent;

                if (model.Id.IsNotNullOrEmpty())
                {
                    var updateModel = Mapper.Map<LessonsData>(model);
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
                    var createModel = Mapper.Map<LessonsData>(model);
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



    }
}
