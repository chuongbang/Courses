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
using Grpc.Core;

namespace Course.Web.Share.IServices
{
    [ServiceContract(Name = "Course.AppUserService")]
    public interface IAppUserService
    {
        ValueTask<AppUserData> GetAsync(string name, CallContext context = default);
        ValueTask<ListAppUserResult> GetPageAsync(AppUserSearch searchPage, CallContext context = default);
        ValueTask<ExcuteResponse> AddAsync(AppUserData appUser, CallContext context = default);
        ValueTask<ExcuteResponse> UpdateAsync(AppUserData appUser, CallContext context = default);
        ValueTask<ExcuteResponse> DeleteAsync(DeleteAppUser appUser, CallContext context = default);
        ValueTask<SignInResultData> LoginAsync(AppUserData appUser, CallContext context = default);
        ValueTask<ExcuteResponse> ChangePasswordAsync(ChangePasswordAppUser appUser, CallContext context = default);
        Task<ListAppRoleResult> GetRoleByUserAsync(string id, CallContext context = default);
        Task<ListAppRoleResult> ChangeRolesAsync(ChangeRolesData model, CallContext context = default);
        Task<ListClaimResult> GetAllClaimsAsync(string userName, CallContext context = default);
        Task<ListClaimResult> GetUserClaimsAsync(string userName, CallContext context = default);
        ValueTask<ExcuteResponse> ResetPasswordAsync(ResetPasswordAppUser appUser, CallContext context = default);
        Task<ExcuteResponse> UpdateUserClaimsAsync(UpdateUserClaim model, CallContext context = default);
    }

    [DataContract]
    public class ExcuteResponse
    {
        [DataMember(Order = 1)]
        public bool State { get; set; }
        [DataMember(Order = 2)]
        public string Id { get; set; }

    }

    [DataContract]
    public class SignInResultData
    {
        [DataMember(Order = 1)]
        public bool Succeeded { get; set; }
        [DataMember(Order = 2)]
        public bool IsLockedOut { get; set; }
        [DataMember(Order = 3)]
        public bool IsNotAllowed { get; set; }
        [DataMember(Order = 4)]
        public bool RequiresTwoFactor { get; set; }
        [DataMember(Order = 5)]
        public string AuthenToken { get; set; }

    }

    [DataContract]
    public class ListAppUserResult
    {
        [DataMember(Order = 1)]
        public List<AppUserData> Data { get; set; }
        [DataMember(Order = 2)]
        public int Total { get; set; }

    }

    [DataContract]
    public class AppUserResult
    {
        [DataMember(Order = 1)]
        public AppUserData Data { get; set; }

    }

    [DataContract]
    public class AppUserSearch
    {
        [DataMember(Order = 1)]
        public string Keyword { get; set; }
        [DataMember(Order = 2)]
        public Page Page { get; set; }
    }

    [DataContract]
    public class DeleteAppUser
    {
        [DataMember(Order = 1)]
        public string Id { get; set; }
        [DataMember(Order = 2)]
        public string VerifyUsername { get; set; }
    }

    [DataContract]
    public class ChangePasswordAppUser
    {
        [DataMember(Order = 1)]
        public string Id { get; set; }
        [DataMember(Order = 2)]
        public string OldPassword { get; set; }
        [DataMember(Order = 3)]
        public string NewPassword { get; set; }
    }

    [DataContract]
    public class ResetPasswordAppUser
    {
        [DataMember(Order = 1)]
        public string Id { get; set; }
        [DataMember(Order = 2)]
        public string NewPassword { get; set; }
    }

    [DataContract]
    public class ChangeRolesData
    {
        [DataMember(Order = 1)]
        public List<AppRoleData> Roles { get; set; }
        [DataMember(Order = 2)]
        public string UserId { get; set; }
    }

    [DataContract]
    public class UpdateUserClaim
    {
        [DataMember(Order = 1)]
        public string UserName { get; set; }
        [DataMember(Order = 2)]
        public List<string> PermissionKeys { get; set; }
    }
}
