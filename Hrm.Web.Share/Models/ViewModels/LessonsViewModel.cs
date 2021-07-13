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


        [Display(Name = "Khóa học")]
        public string TenKhoaHoc { get; set; }             
        
        [Display(Name = "Khóa học")]
        public string TenKhoaHoc { get; set; }        
        
        [Display(Name = "Trạng thái")]
        public bool IsTrial { get; set; }





        public List<Lesson> Children { get; set; }

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

        [Required(ErrorMessageResourceType = typeof(AlertResource), ErrorMessageResourceName = "Required")]
        [StringLength(255, ErrorMessage = "Nhập không quá 255 ký tự")]
        public string TenBaiHoc { get; set; }
    }
}
