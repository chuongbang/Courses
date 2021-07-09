using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using Course.Core.Extentions;
using Course.Web.Share.Domain;
using Course.Web.Share.Models.EditModels;

namespace Course.Web.Client.Pages.AppRole
{
    public partial class AppRoleDetail: ComponentBase
    {
        [Inject] IMapper Mapper { get; set; }
        [Parameter] public EventCallback<AppRoleEditModel> ValueChanged { get; set; }
        [Parameter] public EventCallback Cancel { get; set; }
        public AppRoleData Value { get; set; }
        AppRoleEditModel appRoleEditModel;
        AppRoleEditModel AppRoleEditModel
        {
            get => appRoleEditModel;
            set
            {
                appRoleEditModel = value;
                ValueChanged.InvokeAsync(value);
            }
        }

        protected override Task OnInitializedAsync()
        {
            appRoleEditModel = new AppRoleEditModel();
            return base.OnInitializedAsync();
        }

        public void LoadEditModel(AppRoleData value, bool isEdit = true, bool readOnly = false)
        {
            Value = value;
            if (Value != null)
            {
                appRoleEditModel = new(isEdit);
                Mapper.Map(Value, appRoleEditModel);
                appRoleEditModel.ReadOnly = readOnly;
            }
        }
    }
}
