using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;

namespace Course.Web.Share.Domain
{
    [DataContract]
    public class Page
    {
        [DataMember(Order = 1)]
        public int Total { get; set; }
        [DataMember(Order = 2)]
        public int PageSize { get; set; }
        [DataMember(Order = 3)]
        public int PageIndex { get; set; }
    }
}
