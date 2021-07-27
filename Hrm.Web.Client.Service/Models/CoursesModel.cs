using Course.Web.Share.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Client.Models
{

    public class CoursesModel
    {
        public List<CoursesData> Dts { get; set; }

        public CoursesModel()
        {
            Dts = new List<CoursesData>();
        }
    }
}
