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

namespace Course.Web.Client.Pages.Lessons
{
    public partial class Lessons : ComponentBase
    {
        [CascadingParameter] Error Error { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject] IMapper Mapper { get; set; }
        [Inject] PermissionClaim PermissionClaim { get; set; }
        [Inject] NotificationService Notice { get; set; }

        [Inject] public LessonsAdapterService Service { get; set; }
        [Inject] public CoursesAdapterService CoursesService { get; set; }

        protected LoaiHienThiEnum _displayGrid = LoaiHienThiEnum.None;
        protected List<LessonsViewModel> ListViewLessons;
        protected List<LessonsData> ListLessons;
        protected List<CoursesData> ListCourses;
        protected LessonsEditModel EditModel;
        Dictionary<CoursesViewModel, List<LessonsViewModel>> Datas;

        bool DetailVisible { get; set; }
        Page Page { get; set; } = new() { PageIndex = 1, PageSize = 20, Total = 0 };
        string selectionType = "checkbox";
        bool loading;
        ClaimsPrincipal User;
        string KeyWord { get; set; }
        Property<LessonsEditModel> property;
        List<LessonsData> ListAllLesson;

        protected async override Task OnInitializedAsync()
        {
            User = (await AuthenticationStateTask).User;
            var claims = User?.Claims?.ToList();
            EditModel = new LessonsEditModel();
            property = new Property<LessonsEditModel>();
            ListViewLessons = new List<LessonsViewModel>();
            Datas = new Dictionary<CoursesViewModel, List<LessonsViewModel>>();
            ListCourses = (await CoursesService.GetAllActiveAsync()).Dts;

            await LoadDataAsync();

        }
        async Task LoadDataAsync()
        {
            try
            {
                int sttCourse = 1;
                Datas.Clear();
                var ListAll = await CoursesService.GetCoursesActiveWithLessonsAsync();
                ListAllLesson = ListAll.LDatas;
                foreach (var cs in ListAll?.CDatas)
                {
                    var course = Mapper.Map<CoursesViewModel>(cs);
                    course.Stt = sttCourse++;
                    var ls = ListAll.LDatas.Where(c => c.KhoaHocId == cs.Id);
                    if (true)
                    {
                        Datas.TryAdd(course, Mapper.Map<List<LessonsViewModel>>(ls));
                    }
                }                
                
                foreach (var cs in ListAll?.CDatas)
                {
                    var listLs = ListAll.LDatas.Where(c => c.KhoaHocId == cs.Id);
                    LessonsViewModel ls = new LessonsViewModel();
                    ls.Id = cs.Id;
                    ls.TenKhoaHoc = cs.TenKhoaHoc;
                    ls.Lessons = new List<Lesson>();

                    listLs.ForEach((dt) => 
                    {
                        Lesson lss = new Lesson();
                        lss.Update(dt);
                        ls.Lessons.Add(lss);
                    });


                }




                //var data = await Service.GetByPageAsync(Page, KeyWord);
                //if (data == null)
                //{
                //    return;
                //}
                //ListLessons = data.Dts;
                //Page.Total = data.Total;
                //ListViewLessons = Mapper.Map<List<LessonsViewModel>>(ListLessons);
                //int stt = Page.PageSize * (Page.PageIndex - 1) + 1;
                //ListViewLessons.ForEach(c =>
                //{
                //    c.Stt = stt++;
                //});
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
            loading = false;

        }

        void AddNew()
        {
            EditModel = new LessonsEditModel() { };
            EditModel.DataSource[property.Name(c => c.KhoaHocId)] = ListCourses.ToDictionary(c => c.Id, v => (ISelectItem)v);

            DetailVisible = true;

        }
        void Edit(string id)
        {
            var current = ListAllLesson.FirstOrDefault(c => c.Id == id);
            EditModel = Mapper.Map<LessonsEditModel>(current);

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

        async Task SaveLessonAsync(LessonsEditModel model)
        {
            try
            {
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

        async Task Delete(string id)
        {
            try
            {

                var result = await Service.DeleteAsync(new LessonsData());
                //if (result.State)
                //{
                //    Notice.NotiSuccess(AlertResource.DeleteSuccessful);
                //    await LoadDataAsync();
                //}
                //else
                //{
                //    Notice.NotiError(AlertResource.DeleteFailed);
                //}

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
                var current = ListViewLessons.FirstOrDefault(c => c.Id == id);
                if (current.IsNotNullOrEmpty())
                {
                    current.IsTrial = check;
                    var result = await Service.UpdateAsync(Mapper.Map<LessonsData>(current));
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
