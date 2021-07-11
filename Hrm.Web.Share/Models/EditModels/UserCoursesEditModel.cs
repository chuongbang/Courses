using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Course.Core.Extentions;

namespace Course.Web.Share.Models.EditModels
{
    public class UserCoursesEditModel : EditBaseModel
    {
        public virtual string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(AlertResource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Tên khóa học")]
        public virtual string TenKhoaHoc { get; set; }

        [Display(Name = "Bắt đầu từ")]
        public virtual string TuNgay { get; set; }

        [Display(Name = "Kết thúc từ")]
        public virtual string DenNgay { get; set; }

        public UserCoursesEditModel()
        {
            InputFields.Add<CoursesEditModel>(c => c.TenKhoaHoc);
            InputFields.Add<CoursesEditModel>(c => c.ThoiLuong);
            InputFields.Add<CoursesEditModel>(c => c.TuNgay);
            InputFields.Add<CoursesEditModel>(c => c.DenNgay);

        }


    }
}
