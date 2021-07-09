using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Share.Ultils
{
    public enum SetClaimTypeInterface
    {
        [Description("Nhóm tài khoản")]
        AppRole = 0,
        [Description("Tài khoản")]
        AppUser = 1
        
    }

}
