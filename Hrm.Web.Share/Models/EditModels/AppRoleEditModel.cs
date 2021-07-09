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
    public class AppRoleEditModel : EditBaseModel
    {
        public virtual string Id { get; set; }
        [Display(Name = "Tên vai trò")]
        [Required(ErrorMessageResourceType = typeof(AlertResource), ErrorMessageResourceName = "Required")]
        [MaxLength(255, ErrorMessage = "Tên vai trò không được dài hơn {0} ký tự")]
        [RegularExpression("^[a-zA-Z0-9_]+$", ErrorMessage = "Các ký tự cho phép bao gồm 'a-z', 'A-Z', '0-9' và '_'")]
        public virtual string Name { get; set; }
        [Display(Name = "Mô tả")]
        [Required(ErrorMessageResourceType = typeof(AlertResource), ErrorMessageResourceName = "Required")]
        [MaxLength(255, ErrorMessage = "Mô tả không được dài hơn {0} ký tự")]
        public virtual string Description { get; set; }


        public AppRoleEditModel(bool isEdit = true)
        {
            if (isEdit)
            {
                DisableInputFields.Add<AppRoleEditModel>(c => c.Name);
            }
            InputFields.Add<AppRoleEditModel>(c => c.Name);
            InputFields.Add<AppRoleEditModel>(c => c.Description);
            
        }
    }
}
