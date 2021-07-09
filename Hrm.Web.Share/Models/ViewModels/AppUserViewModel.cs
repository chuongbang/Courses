using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Web.Share.Models.ViewModels
{
    public class AppUserViewModel
    {
        public virtual string Id { get; set; }
        [Display(Name = "Tài khoản")]
        public virtual string UserName { get; set; }
        [Display(Name = "Họ và tên")]
        public virtual string FullName { get; set; }
        [Display(Name = "Chức vụ")]
        public virtual string JobTitle { get; set; }
        [Display(Name = "Trạng thái")]
        public virtual bool IsActive { get; set; }
        [Display(Name = "Stt")]
        public int Stt { get; set; }
    }

}
