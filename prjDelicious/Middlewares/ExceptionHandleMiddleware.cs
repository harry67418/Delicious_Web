using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.Middlewares
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 任務調用
        /// </summary>
        /// <param name="context">HTTP 的上下文</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return context.Response.WriteAsync(
                $"{context.Response.StatusCode}{exception.Message} Internal Server Error from the ExceptionHandle middleware."
            );
        }
    }

    public static class ExceptionHandleMiddlewareExtensions
    {
        /// <summary>在中介程序中全域處理例外</summary>
        /// <param name="builder">中介程序建構器</param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionHandleMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandleMiddleware>();
        }
    }
}
