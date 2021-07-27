using Grpc.Core.Interceptors;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static Grpc.Core.Interceptors.Interceptor;
using Course.Web.Share;

namespace Course.Web.Client.Service
{
    public static class ServiceExtentions
    {
        public static void AddDataService(this IServiceCollection services)
        {
            #region Đăng ký các custom service
            // Tự động đăng ký các service
            var assembly = Assembly.GetExecutingAssembly();
            var classes = assembly.ExportedTypes
               .Where(a => a.FullName.EndsWith("AdapterService"));
            foreach (Type implement in classes)
            {
                services.AddScoped(implement);
            }
            #endregion
        }
    }

    public class AuthHeadersInterceptor : Interceptor
    {
        private readonly TokenProvider _tokenProvider;
        public AuthHeadersInterceptor(TokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var metadata = new Metadata();
            if (_tokenProvider.IsAuthenticated == true)
            {
                metadata.Add("Authorization", $"Bearer {_tokenProvider.AccessToken}");
            }
            var callOption = context.Options.WithHeaders(metadata);
            context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, callOption);

            return base.AsyncUnaryCall(request, context, continuation);
        }

        public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var metadata = new Metadata();
            if (_tokenProvider.IsAuthenticated == true)
            {
                metadata.Add("Authorization", $"Bearer {_tokenProvider.AccessToken}");
            }
            var callOption = context.Options.WithHeaders(metadata);
            context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, callOption);

            return base.BlockingUnaryCall(request, context, continuation);
        }
    }
}
