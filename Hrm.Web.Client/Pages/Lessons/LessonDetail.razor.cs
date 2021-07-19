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
namespace Course.Web.Client.Pages.Lessons
{
    public partial class LessonDetail : ComponentBase
    {
        [CascadingParameter] Error Error { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [CascadingParameter] SessionData SessionData { get; set; }
        [Inject] LessonsAdapterService Service { get; set; }
        [Inject] CoursesAdapterService CourseService { get; set; }
        [Inject] IMapper Mapper { get; set; }

        [Parameter] public string KhoaHocId { get; set; }

        string Title { get; set; } = "Chi tiết khóa học";
        string SubTitle { get; set; } = "Bài số ...";
        CoursesData Course;
        List<LessonsData> ListLessonView;
        string LinkFile { get; set; } = string.Empty;

        protected async override Task OnInitializedAsync()
        {
            Course = new CoursesData();
            ListLessonView = new List<LessonsData>();
            await LoadDataAsync();
        }


        async Task LoadDataAsync()
        {
            var data = await Service.GetLessonsByCourseId(KhoaHocId);
            Course = data.Course;
            Title = Course.TenKhoaHoc;
            ListLessonView = data.Dts;

        }

        void LessonClick(LessonsData lesson)
        {
            LinkFile = lesson.FileNoiDung;

            StateHasChanged();
        }

        async Task BackToMyCourses()
        {


        }

    }
}
