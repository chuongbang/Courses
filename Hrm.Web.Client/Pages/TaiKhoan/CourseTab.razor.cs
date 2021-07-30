using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Course.Web.Client.Data;
using Course.Web.Share.Models.ViewModels;
using AutoMapper;
using Course.Core.Data;
using AntDesign;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Course.Web.Client.Models;
using Course.Web.Share.Domain;
using Microsoft.AspNetCore.Components.Web;
using Course.Core.Extentions;
using Course.Web.Client.Ultils;
using Course.Web.Share;

namespace Course.Web.Client.Pages.TaiKhoan
{
    public partial class CourseTab : ComponentBase
    {
        [CascadingParameter] Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [CascadingParameter] SessionData SessionData { get; set; }

        [Inject] NotificationService Notice { get; set; }
        [Inject] CoursesAdapterService CoursesService { get; set; }
        [Inject] UserCoursesAdapterService UserCoursesService { get; set; }
        [Inject] IMapper Mapper { get; set; }

        [Parameter] public string UserId { get; set; }
        [Parameter] public EventCallback CancelChanged { get; set; }
        AppUserData currentUser => SessionData.User;
        List<UserCoursesViewModel> ListUserCourseView;
        List<CoursesData> allKhoaHoc;
        Page Page { get; set; } = new() { PageIndex = 1, PageSize = 10000, Total = 0 };

        Dictionary<string, ISelectItem> DataSource;
        Dictionary<string, ISelectItem> DataSourceFull;
        int sttId = 0;
        int stt = 1;

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }


        public async Task LoadUserCoursesDetail()
        {
            allKhoaHoc = (await CoursesService.GetAllActiveAsync()).Dts;
            var allMap = await UserCoursesService.GetByIdAsync(UserId, Page);
            ListUserCourseView = allMap.Dts != null ? Mapper.Map<List<UserCoursesViewModel>>(allMap.Dts) : null;
            DataSourceFull = allKhoaHoc != null ? allKhoaHoc.ToDictionary(c => c.Id, v => (ISelectItem)v) : new Dictionary<string, ISelectItem>();
            UpdateListView();
        }

        void AddRow()
        {
            ListUserCourseView ??= new List<UserCoursesViewModel>();
            ListUserCourseView.Add(new UserCoursesViewModel() { Stt = stt++, TuNgay = DateTime.Now, DenNgay = DateTime.Now });
        }

        async Task DeleteRow(int stt)
        {
            var del = ListUserCourseView.FirstOrDefault(c => c.Stt == stt);
            if (del != null && del.Id == null)
            {
                ListUserCourseView.Remove(del);
                Notice.NotiSuccess(AlertResource.DeleteSuccessful);

            }
            else
            {
                bool delResult = await UserCoursesService.DeleteAsync(Mapper.Map<UserCoursesData>(del));
                if (delResult)
                {
                    ListUserCourseView.Remove(del);
                    Notice.NotiSuccess(AlertResource.DeleteSuccessful);
                }
                else
                {
                    Notice.NotiError(AlertResource.HasError);
                }
            }

        }

        void StartEdit(int stt)
        {
            sttId = stt;
        }


        void OnSelectionChange(AutoCompleteOption item, int sttEdit)
        {
            var value = (KeyValuePair<string, ISelectItem>)item.Value;
            CoursesData v = (CoursesData)value.Value;
            var change = ListUserCourseView.FirstOrDefault(c => c.Stt == sttEdit);
            change.KhoaHocId = v.Id;
            change.UserId = UserId;
            UpdateListView(false, false);
        }

        async Task SaveAsync()
        {
            var listAdd = ListUserCourseView.Where(c => !c.IsSave).ToList();

            listAdd.ForEach(c => c.Id = ObjectExtentions.GenerateGuid());
            listAdd.ForEach(c => c.IsSave = true);
            var list = Mapper.Map<List<UserCoursesData>>(listAdd);
            if (list.IsNotNullOrEmpty() && list.Any())
            {
                bool resultAdd = await UserCoursesService.AddAsync(list);
                if (resultAdd)
                {
                    UpdateListView(false);
                    Notice.NotiSuccess(AlertResource.AddSuccessful);
                    await CancelChanged.InvokeAsync(null);
                }
                else
                {
                    Notice.NotiError(AlertResource.HasError);
                }
            }
            else
            {
                Notice.NotiSuccess(AlertResource.UpdateSuccessful);
                await CancelChanged.InvokeAsync(null);
            }

        }

        void UpdateListView(bool updateStt = true, bool updateIsSave = true)
        {
            ListUserCourseView?.Where(c => c.KhoaHocId != null).ForEach(c =>
            {
                DataSourceFull.TryGetValue(c.KhoaHocId, out var tenKhoaHoc);
                if (updateStt)
                {
                    c.Stt = stt++;
                }
                c.TenKhoaHoc = tenKhoaHoc.GetDisplay();
                if (updateIsSave)
                {
                    c.IsSave = true;
                }
            });

        }



    }
}
