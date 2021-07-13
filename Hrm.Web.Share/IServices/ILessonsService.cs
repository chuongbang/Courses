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
    [ServiceContract(Name = "Hyper.LessonsService")]
    public interface ILessonsService
    {
        ValueTask<LessonsResult> GetByPageAsync(LessonsSearch cs, CallContext context = default);
        ValueTask<LessonsResult> GetAllActiveAsync(CallContext context = default);
        ValueTask<ExcuteResponse> AddAsync(LessonsData hs, CallContext context = default);
        ValueTask<ExcuteResponse> UpdateAsync(LessonsData hs, CallContext context = default);
        ValueTask<ExcuteResponse> DeleteAsync(LessonsData hs, CallContext context = default);
        ValueTask<ExcuteResponse> DeleteListAsync(List<LessonsData> ls, CallContext context = default);
    }

    [DataContract]
    public class LessonsResult
    {
        [DataMember(Order = 1)]
        public List<LessonsData> Dts { get; set; }
        [DataMember(Order = 2)]
        public int Total { get; set; }
    }

    [DataContract]
    public class LessonsSearch
    {
        [DataMember(Order = 1)]
        public string Keyword { get; set; }
        [DataMember(Order = 2)]
        public Page Page { get; set; }

    }

}
