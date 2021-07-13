using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Course.Core.Attributes.Web;
using Course.Core.Extentions;
using Course.Core.Ultis;

namespace Course.Web.Share.Models.EditModels
{
    public class AppUserEditModel : EditBaseModel
    {
        public virtual string Id { get; set; }
        [Display(Name = "Ngày tạo")]
        public virtual DateTime CreateTime { get; set; }
        [Display(Name = "Ngày đăng nhập cuối")]
        public virtual DateTime? LastLogin { get; set; }
        [Display(Name = "Số lần đăng nhập")]
        public virtual int LoginCount { get; set; }
        [Display(Name = "Ngày sinh")]
        public virtual DateTime? DateOfBirth { get; set; }
        [Display(Name = "Tài khoản")]
        [Required(ErrorMessageResourceType = typeof(AlertResource), ErrorMessageResourceName = "Required")]
        public virtual string UserName { get; set; }
        [Display(Name = "Họ và tên")]
        public virtual string FullName { get; set; }
        [Display(Name = "Chức vụ")]
        public virtual string JobTitle { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessageResourceType = typeof(AlertResource), ErrorMessageResourceName = "Required")]
        public virtual string Email { get; set; }
        [Display(Name = "Cho phép hoạt động")]
        public virtual bool IsActive { get; set; }
        [Display(Name = "Mật khẩu")]
        [Field(Type = Core.Enums.FieldType.Password)]
        [RequiredIf("Id", null, ErrorMessageResourceType = typeof(AlertResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^\da-zA-Z]).+$", ErrorMessage = "Mật khẩu phải bao gồm đầy đủ ký tự chữ thường, chữ hoa, số và ký tự đặc biệt.")]
        public virtual string PasswordHash { get; set; }
        [Display(Name = "Xác nhận mật khẩu")]
        [Field(Type = Core.Enums.FieldType.Password)]
        [RequiredIf("Id", null, ErrorMessageResourceType = typeof(AlertResource), ErrorMessageResourceName = "Required")]
        [Compare("PasswordHash", ErrorMessage = "Xác nhận mật khẩu phải giống với mật khẩu.")]
        public virtual string ConfirmPassword { get; set; }


        Property<AppUserEditModel> _property;
        public AppUserEditModel(bool isEdit = true)
        {
            if (isEdit)
            {
                DisableInputFields.Add<AppUserEditModel>(c => c.UserName);
                DisableInputFields.Add<AppUserEditModel>(c => c.LastLogin);
                DisableInputFields.Add<AppUserEditModel>(c => c.LoginCount);
                DisableInputFields.Add<AppUserEditModel>(c => c.Email);
            }
            InputFields.Add<AppUserEditModel>(c => c.UserName);
            if (!isEdit)
            {
                InputFields.Add<AppUserEditModel>(c => c.PasswordHash);
                InputFields.Add<AppUserEditModel>(c => c.ConfirmPassword);
            }
            InputFields.Add<AppUserEditModel>(c => c.FullName);
            InputFields.Add<AppUserEditModel>(c => c.DateOfBirth);
            InputFields.Add<AppUserEditModel>(c => c.JobTitle);
            InputFields.Add<AppUserEditModel>(c => c.Email);
            InputFields.Add<AppUserEditModel>(c => c.IsActive);
            if (isEdit)
            {
                InputFields.Add<AppUserEditModel>(c => c.LastLogin);
                InputFields.Add<AppUserEditModel>(c => c.LoginCount);
            }


        }
    }
}
