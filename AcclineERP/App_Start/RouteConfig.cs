using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PoIpaWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Accounts", action = "LogIn", id = UrlParameter.Optional }
            //);
            routes.MapRoute(
                            name: "Default",
                            url: "{controller}/{action}/{id}",
                            defaults: new { controller = "SecUserLogin", action = "SecUserLogin", id = UrlParameter.Optional }
                        );

            routes.MapRoute(
                "AccountActivation", // Route name
        "{controller}/{action}/{confirmId}",
        new { controller = "Account", action = "ConfirmEmail", confirmId = UrlParameter.Optional }
            );


            routes.MapRoute(
                "RecoverPass", // Route name
        "{controller}/{action}/{token}",
        new { controller = "Account", action = "RecoverPass", token = UrlParameter.Optional }
            );


        }
    }
}