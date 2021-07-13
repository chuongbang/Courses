using Course.Web.Share.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Client.Models
{

    public class LessonsModel
    {
        public List<LessonsData> Dts { get; set; }

        public LessonsModel()
        {
            Dts = new List<LessonsData>();
        }
    }
}
