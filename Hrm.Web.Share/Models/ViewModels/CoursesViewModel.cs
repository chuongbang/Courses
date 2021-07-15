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
        public  string Id { get; set; }

        [Display(Name = "Tên khóa học")]
        public  string TenKhoaHoc { get; set; }       
        [Display(Name = "Mã khóa học")]
        public  string MaKhoaHoc { get; set; }

        [Display(Name = "Thời lượng khóa học")]
        public  string ThoiLuong { get; set; }

        [Display(Name = "Bắt đầu từ")]
        public  DateTime? TuNgay { get; set; }

        [Display(Name = "Kết thúc từ")]
        public  DateTime? DenNgay { get; set; }

        [Display(Name = "STT")]
        public int Stt { get; set; }        
        
        [Display(Name = "Công khai")]
        public bool IsActive { get; set; }

        [Display(Name = "Học phí")]
        public string HocPhiFormat { get; set; }
        public decimal? HocPhi { get; set; }

        [Display(Name = "Giáo viên")]
        public string GiaoVien { get; set; }

        public string SoBaiHoc { get; set; }
    }
}
