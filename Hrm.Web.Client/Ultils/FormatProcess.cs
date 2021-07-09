using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Client.Ultils
{
    public static class FormatProcess
    {


        public static string ToShortDate(this DateTime? date)
        {
            return date?.ToString("dd/MM/yyyy");
        }


    }
}
