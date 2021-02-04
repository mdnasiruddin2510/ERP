using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using Data.Context;
using WebMatrix.WebData;
using PoIpaWeb.Models;

namespace PoIpaWeb.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            //LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<UsersContext>(null);

                try
                {
                    using (var context = new UsersContext())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }
                      if (!WebMatrix.WebData.WebSecurity.Initialized)
                {
                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                }
                   
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
                //Database.SetInitializer(new MigrateDatabaseToLatestVersion<SCAppContext, System.Data.Entity.Migrations.DbMigrationsConfiguration<SCAppContext>>("DefaultConnection"));
                //try
                //{
                //    using (var context = new SCAppContext())
                //    {
                //        if (!context.Database.Exists())
                //        {
                //            // Create the SimpleMembership database without Entity Framework migration schema
                //            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();





                //        }
                //        context.Database.Initialize(true);
                //    }


                //}
                //catch (Exception ex)
                //{
                //    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                //}
            }


        }
    }
}
