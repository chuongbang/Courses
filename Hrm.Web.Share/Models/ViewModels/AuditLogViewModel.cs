using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Web.Share.Models.ViewModels
{
    public class AuditLogViewModel
    {
        public virtual string Id { get; set; }
        [Display(Name = "Hành động")]
        public virtual string Action { get; set; }
        [Display(Name = "Người tác động")]
        public virtual string Username { get; set; }
        [Display(Name = "Đối tượng tác động")]
        public virtual string TableName { get; set; }
        [Display(Name = "Thời gian")]
        public virtual DateTime Timestamp { get; set; }
        [Display(Name = "Tên dữ liệu")]
        public virtual string Title { get; set; }
        [Display(Name = "Stt")]
        public int Stt { get; set; }
    }
}
