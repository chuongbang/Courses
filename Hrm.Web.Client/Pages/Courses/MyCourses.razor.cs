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

namespace Course.Web.Client.Pages.Courses
{
    public partial class MyCourses : ComponentBase
    {
        [CascadingParameter] Error Error { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [CascadingParameter] SessionData SessionData { get; set; }
        [Inject] IMapper Mapper { get; set; }
        [Inject] PermissionClaim PermissionClaim { get; set; }
        [Inject] NotificationService Notice { get; set; }
        [Inject] NavigationManager Navigation { get; set; }

        [Inject] public CoursesAdapterService Service { get; set; }
        [Inject] public UserCoursesAdapterService UserCoursesService { get; set; }
        AppUserData currentUser => SessionData.User;
        Page Page { get; set; } = new() { PageIndex = 1, PageSize = 15, Total = 0 };
        string KeyWord { get; set; }
        List<CoursesData> ListCourses;
        List<CoursesViewModel> ListViewCourses;


        protected async override Task OnInitializedAsync()
        {
            ListViewCourses = new List<CoursesViewModel>();
            await LoadDataAsync();
        }

        async Task LoadDataAsync()
        {
            try
            {

                var data = await UserCoursesService.GetPageByIdAsync(currentUser?.Id, Page, KeyWord);
                if (data == null)
                {
                    return;
                }
                ListCourses = data.Dts;
                Page.Total = data.Total;
                ListViewCourses = Mapper.Map<List<CoursesViewModel>>(ListCourses);
                //int stt = Page.PageSize * (Page.PageIndex - 1) + 1;
                ListViewCourses.ForEach(c =>
                {
                    c.HocPhiFormat = c.HocPhi.IsNotNullOrEmpty() ? (c.HocPhi.ToDecimalFormated() + "đ") : "&nbsp;";
                    c.ThoiLuong ??= "&nbsp;";
                    c.GiaoVien ??= "&nbsp;";
                });
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
        }

        void DetailCourse(string id)
        {
            Navigation.NavigateTo($"/chi-tiet-khoa-hoc/{id}");
        }

    }
}
