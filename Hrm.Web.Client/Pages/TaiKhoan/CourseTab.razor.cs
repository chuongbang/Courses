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

namespace Course.Web.Client.Pages.TaiKhoan
{
    public partial class CourseTab : ComponentBase
    {
        [Inject] CoursesAdapterService CoursesService { get; set; }
        [Inject] IMapper Mapper { get; set; }


        List<CoursesViewModel> ListCourseView;

        string editId;
        record ItemData(string Id, string Age, string Address)
        {
            public string Name { get; set; }
        };

        ItemData[] listOfData = { };

        Dictionary<string, ISelectItem> DataSource;
        private AutoCompleteOption selectItem;
        int stt = 1;
        protected async override Task OnInitializedAsync()
        {
            var allKhoaHoc = await CoursesService.GetAllActiveAsync();
            ListCourseView = Mapper.Map<List<CoursesViewModel>>(allKhoaHoc.Dts);
            ListCourseView.ForEach(c =>
            {
                c.Stt = stt++;
            });

            DataSource = allKhoaHoc.Dts != null ? allKhoaHoc.Dts.ToDictionary(c => c.Id, v => (ISelectItem)v) : new Dictionary<string, ISelectItem>();
        }


        void AddRow()
        {
            ListCourseView.Add(new CoursesViewModel() { Stt = stt++});
        }

        void DeleteRow(string id)
        {
            listOfData = listOfData.Where(d => d.Id != id).ToArray();
        }

        void StartEdit(string id)
        {
            editId = id;
        }

        void StopEdit()
        {
            var editedData = listOfData.FirstOrDefault(x => x.Id == editId);
            Console.WriteLine(JsonSerializer.Serialize(editedData));
            editId = null;
        }


        void OnSelectionChange(AutoCompleteOption item)
        {
            selectItem = item;
        }

    }
}
