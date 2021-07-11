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
    [ServiceContract(Name = "Hyper.UserCoursesService")]
    public interface IUserCoursesService
    {
        ValueTask<UserCoursesResult> GetByIdAsync(string id, CallContext context = default);
        ValueTask<ExcuteResponse> AddAsync(UserCourseList hs, CallContext context = default);
        ValueTask<ExcuteResponse> UpdateAsync(UserCoursesData hs, CallContext context = default);
        ValueTask<ExcuteResponse> DeleteAsync(UserCoursesData hs, CallContext context = default);
    }

    [DataContract]
    public class UserCoursesResult
    {
        [DataMember(Order = 1)]
        public List<UserCoursesData> Dts { get; set; }
        [DataMember(Order = 2)]
        public int Total { get; set; }
    }    
    
    [DataContract]
    public class UserCourseList
    {
        [DataMember(Order = 1)]
        public IEnumerable<UserCoursesData> Dts { get; set; }

    }



}
