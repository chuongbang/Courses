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
        public void SetDataList(List<LessonsData> list, int stt)
        {
            if (list.IsNotNullOrEmpty())
            {
                list.ForEach((dt) =>
                {
                    Lesson lss = new Lesson();
                    lss.Update(dt);
                    lss.Stt = stt++.ToString();
                    lss.MaBaiHoc = lss.MaBaiHoc.IsNotNullOrEmpty() ? string.Format("({0})", lss.MaBaiHoc) : string.Empty;
                    lss.TenBaiHocFormat = string.Format("{0} - {1}{2}", lss.Stt, lss.TenBaiHoc, lss.MaBaiHoc);
                    Lessons.Add(lss);
                });
            }

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
        public string TenBaiHocFormat { get; set; }

        public string Stt { get; set; }


    }
}
