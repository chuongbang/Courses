using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Course.Web.Share.Domain;

namespace Course.Web.Share.IServices
{
    [ServiceContract(Name = "Hyper.CoursesService")]
    public interface ICoursesService
    {
        ValueTask<CoursesResult> GetByPageAsync(CoursesSearch cs, CallContext context = default);
        ValueTask<CoursesResult> GetAllActiveAsync(CallContext context = default);
        ValueTask<ExcuteResponse> AddAsync(CoursesData hs, CallContext context = default);
        ValueTask<ExcuteResponse> UpdateAsync(CoursesData hs, CallContext context = default);
        ValueTask<ExcuteResponse> DeleteAsync(CoursesData hs, CallContext context = default);
        ValueTask<ExcuteResponse> DeleteListAsync(List<CoursesData> ls, CallContext context = default);
        ValueTask<CourseLessons> GetCoursesActiveWithLessonsAsync(CoursesSearch cs, CallContext context = default);

    }

    [DataContract]
    public class CoursesResult
    {
        [DataMember(Order = 1)]
        public List<CoursesData> Dts { get; set; }
        [DataMember(Order = 2)]
        public int Total { get; set; }
    }

    [DataContract]
    public class CoursesSearch
    {
        [DataMember(Order = 1)]
        public string Keyword { get; set; }
        [DataMember(Order = 2)]
        public Page Page { get; set; }
        [DataMember(Order = 3)]
        public IEnumerable<string> Ids { get; set; }

    }
    [DataContract]
    public class CourseLessons
    {
        [DataMember(Order = 1)]
        public List<CoursesData> CDatas { get; set; }        
        
        [DataMember(Order = 2)]
        public List<LessonsData> LDatas { get; set; }

        [DataMember(Order = 3)]
        public int Total { get; set; }

    }

}
