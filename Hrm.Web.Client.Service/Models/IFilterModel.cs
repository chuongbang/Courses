using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Course.Web.Client.Service.Models
{
    public interface IFilterModel<T>
    {
        public (IQueryable<T>, int) CreateFilter(IQueryable<T> filter);
    }
}
