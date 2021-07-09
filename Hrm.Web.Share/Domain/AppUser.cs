using NHibernate.AspNetCore.Identity;
using Course.Web.Share;
using System;
using System.Collections.Generic;
using System.Text;

namespace Course.Web.Share.Domain
{
    public class AppUser : IdentityUser
    {
        public virtual DateTime CreateTime { get; set; }
        public virtual DateTime? LastLogin { get; set; }
        public virtual int LoginCount { get; set; }
        public virtual DateTime? DateOfBirth { get; set; }
        [RepresentativeName]
        public virtual string FullName { get; set; }
        public virtual string JobTitle { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string KhoaHocIds { get; set; }
        public virtual DateTime? ExpiredDate { get; set; }
    }
}
