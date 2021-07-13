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
    public class LessonsEditModel : EditBaseModel
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


        public LessonsEditModel(bool isEdit = true)
        {
            InputFields.Add<LessonsEditModel>(c => c.TenBaiHoc);
            InputFields.Add<LessonsEditModel>(c => c.MaBaiHoc);
            InputFields.Add<LessonsEditModel>(c => c.NoiDung);
            InputFields.Add<LessonsEditModel>(c => c.FileNoiDung);

        }


    }
}
