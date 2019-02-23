using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersService.Authentication
{
    public static class QueryStringAuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseQueryStringAuthorization(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<QueryStringAuthenticationMiddleware>();
        }
    }

    public class QueryStringAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private static string _queryStringName = "authorization";
        private static string _authorizationHeaderName = "Authorization";

        public QueryStringAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var authorizationQueryStringValue = context.Request.Query[_queryStringName];

            if (!string.IsNullOrWhiteSpace(authorizationQueryStringValue) && 
                !context.Request.Headers.ContainsKey(_authorizationHeaderName))
            {
                context.Request.Headers.Append(_authorizationHeaderName, "Bearer " + authorizationQueryStringValue);
            }

            return this._next(context);
        }
    }
}
