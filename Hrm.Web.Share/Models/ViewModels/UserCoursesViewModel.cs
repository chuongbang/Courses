using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Web.Share.Models.ViewModels
{
    public class UserCoursesViewModel
    {
        public virtual string Id { get; set; }
        public virtual string KhoaHocId { get; set; }
        public virtual string UserId { get; set; }

        [Display(Name = "Tên khóa học")]
        public virtual string TenKhoaHoc { get; set; }

        [Display(Name = "Bắt đầu từ")]
        public virtual DateTime? TuNgay { get; set; }

        [Display(Name = "Kết thúc từ")]
        public virtual DateTime? DenNgay { get; set; }

        [Display(Name = "STT")]
        public int Stt { get; set; }

        public bool IsSave { get; set; }
    }
}
