using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Data.Context.Interfaces;

namespace Data.Context
{
    public class ContextManager<TContext> : IContextManager<TContext>
       where TContext : IDbContext, new()
    {
        private static string ContextKey = "ContextManager.Context";

        public ContextManager()
        {
            ContextKey = "ContextKey." + typeof(TContext).Name;
        }

        public IDbContext GetContext()
        {
            if (HttpContext.Current.Items[ContextKey] == null)
                HttpContext.Current.Items[ContextKey] = new TContext();
            return HttpContext.Current.Items[ContextKey] as IDbContext;
        }

        public void Finish()
        {
            if (HttpContext.Current.Items[ContextKey] != null)
            {
                var dbContext = HttpContext.Current.Items[ContextKey] as IDbContext;
                if (dbContext != null)
                    dbContext.Dispose();
            }
        }
    }
}
