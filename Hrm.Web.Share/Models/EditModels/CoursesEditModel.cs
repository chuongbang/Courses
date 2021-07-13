using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Course.Core.Attributes.Web;
using Course.Core.Extentions;

namespace Course.Web.Share.Models.EditModels
{
    public class CoursesEditModel : EditBaseModel
    {
        public virtual string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(AlertResource), ErrorMessageResourceName = "Required")]
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
        
        [Display(Name = "Công khai")]
        [Field(Type = Core.Enums.FieldType.Switch)]
        public virtual bool IsActive { get; set; }



        public CoursesEditModel(bool isEdit = true)
        {
            InputFields.Add<CoursesEditModel>(c => c.TenKhoaHoc);
            InputFields.Add<CoursesEditModel>(c => c.MaKhoaHoc);
            InputFields.Add<CoursesEditModel>(c => c.ThoiLuong);
            InputFields.Add<CoursesEditModel>(c => c.TuNgay);
            InputFields.Add<CoursesEditModel>(c => c.DenNgay);
            InputFields.Add<CoursesEditModel>(c => c.IsActive);

        }


    }
}
