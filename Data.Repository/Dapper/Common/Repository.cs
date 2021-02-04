using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace Data.Repository.Dapper.Common
{
    public class Repository : IDisposable
    {
        //public IDbConnection MusicStoreConnection
        //{
        //   // get { return new SqlCeConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString); }
        //}

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
