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
    public class RolesMiddleware
    {
        private readonly RequestDelegate _next;

        public RolesMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            if (!(context.Session.Keys.Contains("starting")))
            {
                RolesInitializer.Initialize(context).Wait();
                context.Session.SetString("starting", "Yes");
            }

            // Вызов следующего делегата / компонента middleware в конвейере
            return _next.Invoke(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RolesMiddlewareExtensions
    {
        public static IApplicationBuilder UseRolesMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RolesMiddleware>();
        }
    }
}
