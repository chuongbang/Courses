using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;
using System.Xml.Linq;
using ProtoBuf.Grpc;
using System.ServiceModel;
using Course.Web.Share.Domain;
using System.Security.Claims;

namespace Course.Web.Share.IServices
{
    [ServiceContract(Name = "Course.AppRoleService")]
    public interface IAppRoleService
    {
        Task<AppRoleData> GetAsync(string name, CallContext context = default);
        Task<ListAppRoleResult> GetAllAsync(CallContext context = default);
        Task<ListAppRoleResult> GetByUserAsync(string userId, CallContext context = default);
        Task<ListAppRoleResult> GetPageAsync(AppRoleSearch model, CallContext context = default);
        Task<ExcuteResponse> AddAsync(AppRoleData model, CallContext context = default);
        Task<ExcuteResponse> UpdateAsync(AppRoleData model, CallContext context = default);
        Task<ExcuteResponse> DeleteAsync(string modelId, CallContext context = default);
        Task<ListClaimResult> GetAllClaimAsync(AppRoleData model, CallContext context = default);
        Task<ExcuteResponse> HasUserInRole(string roleName, CallContext context = default);
        Task<ExcuteResponse> UpdateClaimsAsync(UpdateClaim model, CallContext context = default);
    }

    [DataContract]
    public class ListAppRoleResult
    {
        [DataMember(Order = 1)]
        public List<AppRoleData> Data { get; set; }
        [DataMember(Order = 2)]
        public int Total { get; set; }

    }

    [DataContract]
    public class AppRoleResult
    {
        [DataMember(Order = 1)]
        public AppRoleData Data { get; set; }

    }

    [DataContract]
    public class AppRoleSearch
    {
        [DataMember(Order = 1)]
        public string Keyword { get; set; }
        [DataMember(Order = 2)]
        public Page Page { get; set; }
    }

    [DataContract]
    public class DeleteAppRole
    {
        [DataMember(Order = 1)]
        public string Id { get; set; }
    }

    [DataContract]
    public class UpdateClaim
    {
        [DataMember(Order = 1)]
        public AppRoleData Role { get; set; }
        [DataMember(Order = 2)]
        public List<string> PermissionKeys { get; set; }
    }

    [DataContract]
    public class ListClaimResult
    {
        [DataMember(Order = 1)]
        public List<ClaimData> Data { get; set; }

    }

    [DataContract]
    public class ClaimData
    {
        [DataMember(Order = 1)]
        public string Type { get; set; }
        [DataMember(Order = 2)]
        public string Value { get; set; }

    }
}
