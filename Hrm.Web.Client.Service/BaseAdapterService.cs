using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc.Client;
using Course.Core.AuditLog;
using Course.Web.Share;

namespace Course.Web.Client.Service
{
    public class BaseAdapterService<T> where T : class
    {
        protected GrpcChannel GrpcChannel { get; set; }
        protected readonly ILogger<T> _logger;
        private readonly CallInvoker _interceptingInvoker;

        public BaseAdapterService(HostService host, FileLoggerProvider fileLoggerProvider, TokenProvider tokenProvider)
        {
            GrpcChannel = GrpcChannel.ForAddress(host.Url);
            _interceptingInvoker = GrpcChannel.Intercept(new AuthHeadersInterceptor(tokenProvider));
            _logger = fileLoggerProvider.CreateLogger<T>();
        }

        protected T Service
        {
            get
            {
                return _interceptingInvoker.CreateGrpcService<T>();
            }
        }

    }
}
