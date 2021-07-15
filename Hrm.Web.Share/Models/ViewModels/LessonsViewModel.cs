using Course.Core.Extentions;
using Course.Web.Share.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Web.Share.Models.ViewModels
{
    public class LessonsViewModel
    {
        public string Id { get; set; }
        public string KhoaHocId { get; set; }

        [Display(Name = "Khóa học")]
        public string TenKhoaHoc { get; set; }

        [Display(Name = "Thời lượng")]
        public string ThoiLuong { get; set; }

        [Display(Name = "Giáo viên")]
        public string GiaoVien { get; set; }

        [Display(Name = "STT")]
        public int Stt { get; set; }

        public List<Lesson> Lessons { get; set; }

        public LessonsViewModel()
        {
            Lessons = new List<Lesson>();

        }
        public void SetDataList(List<LessonsData> list)
        {

            list.ForEach((dt) =>
            {
                Lesson lss = new Lesson();
                lss.Update(dt);
                Lessons.Add(lss);
            });

        }

    }

    public class Lesson
    {
        public string Id { get; set; }
        public string KhoaHocId { get; set; }

        [Display(Name = "Nội dung bài học")]

        public string NoiDung { get; set; }

        [Display(Name = "File nội dung")]

        public string FileNoiDung { get; set; }

        [Display(Name = "Mã bài học")]
        public string MaBaiHoc { get; set; }

        public bool IsTrial { get; set; }

        public string TenBaiHoc { get; set; }


    }
}
