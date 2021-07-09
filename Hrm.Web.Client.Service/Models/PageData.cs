using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Client.Service.Models
{
    public class PageData<T>
    {
        public int Total { get; set; }
        public List<T> Items { get; set; }
        public PageData(List<T> items, int total)
        {
            Items = items;
            Total = total;
        }
    }
}
