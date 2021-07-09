using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Course.Core.Extentions;

namespace Course.Web.Share.Ultils
{

    public static class CustomClaimTypes
    {
        public static string Permission = "http://example.org/claims/permission";
        public static bool HasClaim(this IEnumerable<Claim> claims, string value)
        {
            if (claims == null)
            {
                return false;
            }
            if (claims.IsAdmin())
            {
                return true;
            }
            return claims.Any(c => c.Type == Permission && c.Value == value);
        }

        public static bool IsAdmin(this IEnumerable<Claim> claims) => claims.Any(c => c.Type == Permission && c.Value == PermissionKey.ADMIN);
    }

    public class PermissionKey
    {
        public static string ADMIN = "Admin";
        public static string ACCOUNT_VIEW = "Account.View";
        public static string ACCOUNT_ADD = "Account.Add";
        public static string ACCOUNT_EDIT = "Account.Edit";
        public static string ACCOUNT_DELETE = "Account.Delete";
        public static string ACCOUNT_SETROLE = "Account.SetRole";
        public static string ACCOUNT_SETCLAIM = "Account.SetClaim";
        public static string ACCOUNT_CHANGEPASSWORD = "Account.ChangePassword";

        public static string ROLE_VIEW = "Role.View";
        public static string ROLE_ADD = "Role.Add";
        public static string ROLE_EDIT = "Role.Edit";
        public static string ROLE_DELETE = "Role.Delete";
        public static string ROLE_SETCLAIM = "Role.SetClaim";

        public static string AUDITLOG_VIEW = "AuditLog.View";
        public static string AUDITLOG_ADD = "AuditLog.Add";
        public static string AUDITLOG_EDIT = "AuditLog.Edit";
        public static string AUDITLOG_DELETE = "AuditLog.Delete";        
        
        public static string COURSE_VIEW = "Course.View";
        public static string COURSE_ADD = "Course.Add";
        public static string COURSE_EDIT = "Course.Edit";
        public static string COURSE_DELETE = "Course.Delete";        
        
        public static string LESSON_VIEW = "Lesson.View";
        public static string LESSON_ADD = "Lesson.Add";
        public static string LESSON_EDIT = "Lesson.Edit";
        public static string LESSON_DELETE = "Lesson.Delete";
    }

    public class PermissionClaim
    {
        public PermissionClaim(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    var claims = httpContextAccessor.HttpContext.User?.Claims?.ToList();
                    var fields = this.GetType().GetFields();
                    foreach (var field in fields)
                    {
                        field.SetValue(this, claims.HasClaim(typeof(PermissionKey).GetValueField(field.Name).ToString()));
                    }
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public bool ADMIN;
        public bool ACCOUNT_VIEW;
        public bool ACCOUNT_ADD;
        public bool ACCOUNT_EDIT;
        public bool ACCOUNT_DELETE;
        public bool ACCOUNT_SETROLE;
        public bool ACCOUNT_SETCLAIM;
        public bool ACCOUNT_CHANGEPASSWORD;

        public bool ROLE_VIEW;
        public bool ROLE_ADD;
        public bool ROLE_EDIT;
        public bool ROLE_DELETE;
        public bool ROLE_SETCLAIM;

        public bool AUDITLOG_VIEW;
        public bool AUDITLOG_ADD;
        public bool AUDITLOG_EDIT;
        public bool AUDITLOG_DELETE;

    }
}
