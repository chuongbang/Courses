using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Web.Share.Models.ViewModels
{
    public class CoursesViewModel
    {
        public virtual string Id { get; set; }

        [Display(Name = "Tên khóa học")]
        public virtual string TenKhoaHoc { get; set; }       
        [Display(Name = "Mã khóa học")]
        public virtual string MaKhoaHoc { get; set; }

        [Display(Name = "Thời lượng khóa học")]
        public virtual string ThoiLuong { get; set; }

        [Display(Name = "Bắt đầu từ")]
        public virtual DateTime? TuNgay { get; set; }

        [Display(Name = "Kết thúc từ")]
        public virtual DateTime? DenNgay { get; set; }

        [Display(Name = "STT")]
        public int Stt { get; set; }        
        
        [Display(Name = "Công khai")]
        public bool IsActive { get; set; }

        public string Selected { get; set; }
    }
}
