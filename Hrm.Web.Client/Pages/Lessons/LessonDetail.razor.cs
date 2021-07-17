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

        [Parameter] public string KhoaHocId { get; set; }

        string Title { get; set; } = "Chi tiết khóa học";
        string SubTitle { get; set; } = "Bài số ...";

        protected override Task OnInitializedAsync()
        {

            return base.OnInitializedAsync();   
        }


        async Task BackToMyCourses()
        {

        }
    }
}
