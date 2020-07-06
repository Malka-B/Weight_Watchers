using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Weight_Watchers.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    throw new UnauthorizedAccessException ("Password or Email are not correct. try again!");
                }
                if (context.Response.StatusCode == StatusCodes.Status400BadRequest)
                {
                    throw new UnauthorizedAccessException("Action Failed. Please try again!");
                }


            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            //var code = HttpStatusCode.Unauthorized;
            //Log.Error("Can't save patient without id");
            // 500 if unexpected           
            //if (ex is BadRequestException) code = HttpStatusCode.InternalServerError;
            //else if (ex is MyUnauthorizedException) code = HttpStatusCode.Unauthorized;
            //else if (ex is MyException) code = HttpStatusCode.BadRequest;
            //context.Response.StatusCode = HttpStatusCode.InternalServerError;
            //context.Response.StatusCode = (int)code;

            var result = JsonConvert.SerializeObject(new { error = ex.Message });
            //context.Response.ContentType = "application/json";            
            return context.Response.WriteAsync(result);
        }
    }
}
