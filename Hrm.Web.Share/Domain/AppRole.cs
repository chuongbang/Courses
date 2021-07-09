using NHibernate.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Course.Web.Share.Domain
{
    public class AppRole : IdentityRole
    {
        public virtual string Description { get; set; }

    }
}
