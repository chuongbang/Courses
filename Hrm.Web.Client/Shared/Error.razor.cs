using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign;
using Course.Web.Client.Ultils;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Course.Web.Share;
using Grpc.Core;

namespace Course.Web.Client.Shared
{
    public partial class Error : ComponentBase
    {
        [Inject] ILogger<Error> Logger { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] NotificationService Notice { get; set; }
        [Inject] MessageService Message { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }

        public void ProcessError(Exception ex)
        {
            if (ex is RpcException)
            {
                if ((ex as RpcException).StatusCode == StatusCode.Unauthenticated)
                {
                    NavigationManager.NavigateTo("/dang-xuat");
                }
                else if ((ex as RpcException).StatusCode == StatusCode.Unimplemented)
                {
                    Notice.NotiError("Chức năng chưa được cài đặt");
                }
            }
            else
            {
                Notice.NotiError(AlertResource.Exception);
            }
            Logger.LogError("Error:ProcessError - Type: {Type} Message: {Message}",
                ex.GetType(), ex.Message);
        }

        public void InDevelopment()
        {
            Message.Info("Tính năng đang trong quá trình phát triển");
        }
    }
}
