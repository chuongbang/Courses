using System.ComponentModel.DataAnnotations;
using Course.Core.Attributes.Web;
using Course.Core.Enums;
using Course.Core.Extentions;
using Course.Web.Share.Models.EditModels;

namespace Course.Web.Client.Models
{
    public class LoginInputModel
    {
        //[Required(ErrorMessageResourceType = typeof(ErrorMessageResource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Tài khoản")]
        public string UserName { get; set; }

        //[Required(ErrorMessageResourceType = typeof(ErrorMessageResource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemember { get; set; }

    }
    
    
    public class ChangePasswordModel : EditBaseModel
    {
        [Required(ErrorMessage = "Mật khẩu không được trống.")]
        [DataType(DataType.Password)]
        [Field(FieldType.Password)]
        [Display(Name = "Mật khẩu hiện tại")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(32, ErrorMessage = "Mật khẩu phải có độ dài từ 8 đến 32 ký tự.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^\da-zA-Z]).+$", ErrorMessage = "Mật khẩu phải bao gồm đầy đủ ký tự chữ thường, chữ hoa, số và ký tự đặc biệt.")]
        [DataType(DataType.Password)]
        [Field(FieldType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public virtual string Password { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống.")]
        [DataType(DataType.Password)]
        [Field(FieldType.Password)]
        [CompareProperty("Password", ErrorMessage = "Xác nhận mật khẩu phải giống với mật khẩu.")]
        [Display(Name = "Nhập lại mật khẩu")]
        public virtual string ConfirmPassword { get; set; }

        public string UserName { get; set; }
        public string Id { get; set; }

        public ChangePasswordModel()
        {
            InputFields.Add<ChangePasswordModel>(c => c.OldPassword);
            InputFields.Add<ChangePasswordModel>(c => c.Password);
            InputFields.Add<ChangePasswordModel>(c => c.ConfirmPassword);
        }
        
        
    }

    public class ResetPasswordModel : EditBaseModel
    {
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(32, ErrorMessage = "Mật khẩu phải có độ dài từ 8 đến 32 ký tự.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^\da-zA-Z]).+$", ErrorMessage = "Mật khẩu phải bao gồm đầy đủ ký tự chữ thường, chữ hoa, số và ký tự đặc biệt.")]
        [DataType(DataType.Password)]
        [Field(FieldType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public virtual string Password { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống.")]
        [DataType(DataType.Password)]
        [Field(FieldType.Password)]
        [CompareProperty("Password", ErrorMessage = "Xác nhận mật khẩu phải giống với mật khẩu.")]
        [Display(Name = "Nhập lại mật khẩu")]
        public virtual string ConfirmPassword { get; set; }

        public string UserName { get; set; }
        public string Id { get; set; }

        public ResetPasswordModel()
        {
            InputFields.Add<ResetPasswordModel>(c => c.Password);
            InputFields.Add<ResetPasswordModel>(c => c.ConfirmPassword);
        }


    }


}
