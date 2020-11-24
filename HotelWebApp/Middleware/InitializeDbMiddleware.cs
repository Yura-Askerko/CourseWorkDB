using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelWebApp.Data;

namespace HotelWebApp.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class InitializeDbMiddleware
    {
        private readonly RequestDelegate _next;

        public InitializeDbMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext, HotelContext db)
        {
            if (!(httpContext.Session.Keys.Contains("starting")))
            {
                Initializer.Initialize(db);
                httpContext.Session.SetString("starting", "Yes");
            }
            return _next.Invoke(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class InitializeDbMiddlewareExtensions
    {
        public static IApplicationBuilder UseInitializeDbMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<InitializeDbMiddleware>();
        }
    }
}
