using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Web.Share.Models.ViewModels
{
    public class AuditLogDetailViewModel
    {
        public virtual string Id { get; set; }
        [Display(Name = "Trường dữ liệu")]
        public virtual string FieldName { get; set; }
        [Display(Name = "Dữ liệu cũ")]
        public virtual string OldValue { get; set; }
        [Display(Name = "Dữ liệu mới")]
        public virtual string NewValue { get; set; }
        [Display(Name = "Stt")]
        public int Stt { get; set; }
    }
    
}
