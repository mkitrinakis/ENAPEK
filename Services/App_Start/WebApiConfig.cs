using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Services
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
              name: "DefaultApiP2",
              routeTemplate: "api/{controller}/{id1}/{id2}",
              defaults: new { id1 = RouteParameter.Optional, id2 = RouteParameter.Optional }
          );
        }
    }
}
