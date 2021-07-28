using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Course.Core.Attributes.Web;
using Course.Core.Data;
using Course.Core.Enums;
using Course.Core.Extentions;
using Course.Core.Ultis;
using Course.Web.Share.Domain;

namespace Course.Web.Share.Models.EditModels
{
    public class LessonsEditModel : EditBaseModel
    {
        public string Id { get; set; }

        [Display(Name = "Khóa học")]
        [Field(Type = FieldType.Combobox)]
        public string KhoaHocId { get; set; }

        [Display(Name = "Nội dung bài học")]
        [Field(Type = FieldType.RichTextEditor)]
        [StringLength(4000, ErrorMessage = "Nhập không quá 4000 ký tự")]
        public string NoiDung { get; set; }

        [Display(Name = "File nội dung")]
        [Field(Type = FieldType.File)]
        [File("application/pdf, image/jpeg", 100000)]
        public string FileNoiDung { get; set; }

        [Display(Name = "Mã bài học")]
        public string MaBaiHoc { get; set; }

        [Display(Name = "Cho phép học thử")]
        [Field(Type = FieldType.Switch)]
        public bool IsTrial { get; set; }

        [Required(ErrorMessageResourceType = typeof(AlertResource), ErrorMessageResourceName = "Required")]
        [StringLength(255, ErrorMessage = "Nhập không quá 255 ký tự")]
        [Display(Name = "Tên bài học")]
        public string TenBaiHoc { get; set; }

        [Required(ErrorMessageResourceType = typeof(AlertResource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Kiểu nội dung bài học")]
        [Field(Type = FieldType.Combobox)]
        public string TypeContent { get; set; }


        Property<LessonsEditModel> property;

        public LessonsEditModel(bool isEdit = true, List<CoursesData> listLS = null)
        {
            DataSource = new Dictionary<string, Dictionary<string, ISelectItem>>();
            property = new Property<LessonsEditModel>();

            InputFields.Add<LessonsEditModel>(c => c.KhoaHocId);
            InputFields.Add<LessonsEditModel>(c => c.TenBaiHoc);
            InputFields.Add<LessonsEditModel>(c => c.MaBaiHoc);
            InputFields.Add<LessonsEditModel>(c => c.TypeContent);
            InputFields.Add<LessonsEditModel>(c => c.NoiDung);
            InputFields.Add<LessonsEditModel>(c => c.FileNoiDung);
            InputFields.Add<LessonsEditModel>(c => c.IsTrial);


            List<SelectItem> typeContent = new List<SelectItem>();
            typeContent.Add(new SelectItem() { Text = "Nhập File", Value = "0" });
            typeContent.Add(new SelectItem() { Text = "Nhập nội dung Text", Value = "1" });
            DataSource[property.Name(c => c.TypeContent)] = typeContent.ToDictionary(c => c.Value, v => (ISelectItem)v);

            DataSource[property.Name(c => c.KhoaHocId)] = listLS == null ? new Dictionary<string, ISelectItem>() : listLS.ToDictionary(c => c.Id, v => (ISelectItem)v); ;

        }


    }
}
