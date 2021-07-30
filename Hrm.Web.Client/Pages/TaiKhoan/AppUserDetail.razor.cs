using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using Course.Core.Extentions;
using Course.Web.Share.Domain;
using Course.Web.Share.Models.EditModels;
using Course.Web.Client.Data;
using Course.Core.Data;

namespace Course.Web.Client.Pages.TaiKhoan
{
    public partial class AppUserDetail: ComponentBase
    {
        [Inject] IMapper Mapper { get; set; }
        [Parameter] public EventCallback<AppUserEditModel> ValueChanged { get; set; }
        [Parameter] public EventCallback Cancel { get; set; }
        public AppUserData Value { get; set; }
        AppUserEditModel appUserEditModel;
        AppUserEditModel AppUserEditModel
        {
            get => appUserEditModel;
            set
            {
                appUserEditModel = value;
                ValueChanged.InvokeAsync(value);
            }
        }

        protected override Task OnInitializedAsync()
        {
            appUserEditModel = new AppUserEditModel();
            return base.OnInitializedAsync();
        }

        public void LoadEditModel(AppUserData value, bool isEdit = true, bool readOnly = false)
        {
            Value = value;
            if (Value != null)
            {
                appUserEditModel = new(isEdit);
                //Value.IsActive = true;
                Mapper.Map(Value, appUserEditModel);
                appUserEditModel.ReadOnly = readOnly;
                appUserEditModel.ConfirmPassword = appUserEditModel.PasswordHash;
                appUserEditModel.ExpiredDate = Value.ExpiredDate.IsNullOrEmpty() ? DateTime.Now : Value.ExpiredDate;
            }
        }

        public void DisableField()
        {
            appUserEditModel.DisableInputFields.Add<AppUserEditModel>(c => c.IsActive);
        }
    }
}
