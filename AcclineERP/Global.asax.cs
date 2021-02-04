using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Data.Context;
using WebMatrix.WebData;
using System.Data.Entity;

namespace PoIpaWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        //protected void Application_Start()
        //{
           
        //    AreaRegistration.RegisterAllAreas();

        //    WebApiConfig.Register(GlobalConfiguration.Configuration);
        //    FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        //    RouteConfig.RegisterRoutes(RouteTable.Routes);
        //    BundleConfig.RegisterBundles(BundleTable.Bundles);
        //    AuthConfig.RegisterAuth();
            
        //}
        protected void Application_Start()
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<SCAppContext, System.Data.Entity.Migrations.DbMigrationsConfiguration<SCAppContext>>("DefaultConnection"));
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            using (var context = new AcclineERPContext())
            {
                if (!context.Database.Exists())
                {
                    // Register the SimpleMembership database without Entity Framework migration schema
                    //((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                    context.Database.Create();
                }
                context.Database.Initialize(true);
                //context.Database.Delete();
                //context.Database.Create();

            }
            string url = ""; //HttpContext.Current.Request.Url.AbsoluteUri;
            //string url = HttpContext.Current.Request.Url.Authority;
            //if (url == "ntcgarden.accline.com")
            //{
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "Username", autoCreateTables: true);
            //}
            //else if (url == "ntctest.accline.com")
            //{
            //    WebSecurity.InitializeDatabaseConnection("testConnection", "UserProfile", "UserId", "Username", autoCreateTables: true);
            //}
            //else
            //{
            //    WebSecurity.InitializeDatabaseConnection("testConnection", "UserProfile", "UserId", "Username", autoCreateTables: true);
            //}
        }
    }
}