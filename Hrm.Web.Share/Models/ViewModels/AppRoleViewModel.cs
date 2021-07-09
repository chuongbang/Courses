using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Web.Share.Models.ViewModels
{
    public class AppRoleViewModel
    {
        public int Stt { get; set; }
        public string Id { get; set; }
        public bool Checked { get; set; }
        public string NormalizedName { get; set; }
        [Display(Name = "Tên vai trò")]
        public string Name { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
    }

    public class ClaimViewModel
    {
        public string Id { get; set; }
        public string Group { get; set; }
        public bool Checked { get; set; }
        [Display(Name = "Tên chức năng")]
        public string Name { get; set; }
    }
}
