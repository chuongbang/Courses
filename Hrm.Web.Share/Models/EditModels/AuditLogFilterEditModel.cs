using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Course.Core.Data;
using Course.Core.Extentions;
using Course.Web.Share.Domain;
using System.Xml.Linq;
using Course.Core.Ultis;

namespace Course.Web.Share.Models.EditModels
{
    public class AuditLogFilterEditModel: EditBaseModel
    {
        public Page Page { get; set; }
        public string Id { get; set; }
        [Display(Name = "Người tác động")]
        public virtual string Username { get; set; }

        [Display(Name = "Hành động")]
        public virtual string Action { get; set; }

        [Display(Name = "Đối tượng tác động")]
        public virtual string TableName { get; set; }

        [Display(Name = "Tên dữ liệu")]
        public virtual string Title { get; set; }

        [Display(Name = "Thời gian")]
        public virtual DateTime? StartDate { get; set; } = DateTime.Now.AddMonths(-1);
        public virtual DateTime? EndDate { get; set; } = DateTime.Now;
        public Property<AuditLogFilterEditModel> Property { get; set; }
        public AuditLogFilterEditModel()
        {
            Property = new Property<AuditLogFilterEditModel>();
            DataSource[Property.Name(c => c.Action)] = new Dictionary<string, ISelectItem> { { "Thêm", new SelectItem("Thêm") }, { "Cập nhật", new SelectItem("Cập nhật") }, { "Xóa", new SelectItem("Xóa") } };
        }

        public void SetDate(DateTime?[] dateTimes)
        {
            StartDate = dateTimes[0];
            EndDate = dateTimes[1];
        }
        
        public void ClearValue()
        {
            Username = null;
            Action = null;
            TableName = null;
            Title = null;
            StartDate = null;
            EndDate = null;
        }
    }
}
