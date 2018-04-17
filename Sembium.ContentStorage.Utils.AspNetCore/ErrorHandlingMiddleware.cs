using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Utils.AspNetCore
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;

        private readonly Type _userExceptionType;
        private readonly Type _authenticationExceptionType;
        private readonly Type _authorizationExceptionType;

        public ErrorHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory,
            Type userExceptionType = null, Type authenticationExceptionType = null, Type authorizationExceptionType = null)
        {
            _next = next;
            _loggerFactory = loggerFactory;
            _userExceptionType = userExceptionType;
            _authenticationExceptionType = authenticationExceptionType;
            _authorizationExceptionType = authorizationExceptionType;
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _loggerFactory.CreateLogger("Errors").LogError(0, ex, ex.GetAggregateMessages());

                var exceptionType = ex.GetType();

                if ((exceptionType == _userExceptionType) || (exceptionType == _authenticationExceptionType) || (exceptionType == _authorizationExceptionType))
                {
                    await HandleUserExceptionAsync(context, ex);
                }
                else
                {
                    throw;
                }
            }
        }

        private Task HandleUserExceptionAsync(HttpContext context, Exception exception)
        {
            var result = JsonConvert.SerializeObject(new { error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)GetUserExceptionHttpStatusCode(exception);
            return context.Response.WriteAsync(result);
        }

        private HttpStatusCode GetUserExceptionHttpStatusCode(Exception exception)
        {
            var exceptionType = exception.GetType();

            if (exceptionType == _authenticationExceptionType)
            {
                return HttpStatusCode.Unauthorized;
            }

            if (exceptionType == _authorizationExceptionType)
            {
                return HttpStatusCode.Forbidden;
            }

            return HttpStatusCode.InternalServerError;
        }
    }
}
