using AutoMapper;
using Course.Web.Share.IServices;
using Course.Web.Share.Models.EditModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Course.Web.Share.Map
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {

            #region Đăng ký các mapping giữa Data class và DB class
            var assembly = Assembly.GetAssembly(typeof(AppMappingProfile));
            var classes = assembly.ExportedTypes
               .Where(a => a.Namespace.Equals("Course.Web.Share.Domain") && !a.Name.EndsWith("Data"));
            foreach (Type type in classes)
            {
                var dataClass = assembly.ExportedTypes.FirstOrDefault(c => c.Name == type.Name + "Data");
                if (dataClass != null)
                {
                    CreateMap(type, dataClass).ReverseMap();
                    var editModelClass = assembly.ExportedTypes.FirstOrDefault(c => c.Name == type.Name + "EditModel");
                    if (editModelClass != null)
                    {
                        CreateMap(dataClass, editModelClass).ReverseMap();
                    }
                    var viewModelClass = assembly.ExportedTypes.FirstOrDefault(c => c.Name == type.Name + "ViewModel");
                    if (viewModelClass != null)
                    {
                        CreateMap(dataClass, viewModelClass).ReverseMap();
                    }
                }
            }
            #endregion



            CreateMap<Microsoft.AspNetCore.Identity.SignInResult, SignInResultData>().ReverseMap();
            CreateMap<ClaimData, System.Security.Claims.Claim>().ReverseMap();
            CreateMap<AuditLogSearch, AuditLogFilterEditModel>().ReverseMap();


        }
    }
}
