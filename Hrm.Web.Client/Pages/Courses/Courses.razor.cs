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

namespace Course.Web.Client.Pages.Courses
{
    public partial class Courses : ComponentBase
    {
        [CascadingParameter] Error Error { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject] IMapper Mapper { get; set; }

        [Inject] public CoursesAdapterService Service { get; set; }

        protected LoaiHienThiEnum _displayGrid = LoaiHienThiEnum.None;
        protected List<CoursesViewModel> ListViewCourses;
        protected List<CoursesData> ListCourses;
        protected CourseInputModel Model;


        Page Page { get; set; } = new() { PageIndex = 1, PageSize = 10, Total = 0 };
        ITable table;
        IEnumerable<CoursesViewModel> selectedRows;
        string selectionType = "checkbox";
        bool loading;
        ClaimsPrincipal User;
        string KeyWord = "";


        protected async override Task OnInitializedAsync()
        {
            _displayGrid = LoaiHienThiEnum.Grid;
            User = (await AuthenticationStateTask).User;
            var claims = User?.Claims?.ToList();
            Model = new CourseInputModel();
            ListViewCourses = new List<CoursesViewModel>();
            await LoadDataAsync();


        }
        async Task LoadDataAsync()
        {
            try
            {
                loading = true;
                StateHasChanged();
                var data = await Service.GetByIdsAsync(new List<string>() { "edbca070d23e49caaec4b9cb9256c123" }, Page, KeyWord);
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
                });

            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
            loading = false;

        }

        void ShowInput()
        {
            Model = new CourseInputModel();
            _displayGrid = LoaiHienThiEnum.Input;

        }

        public void RemoveSelection(string key)
        {
            var selected = selectedRows.Where(x => x.Id != key).ToList();
            table.SetSelection(selected.Select(x => x.TenKhoaHoc).ToArray());
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




    }
}
