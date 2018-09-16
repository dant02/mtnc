using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using FirebirdSql.Data.FirebirdClient;

namespace web.Controllers
{
    public class HomeController : Controller
    {
        private string connStr = @"Database='d:\mtnc.FDB';DataSource=localhost;User=SYSDBA;Password=masterkey;Dialect=3;Charset=UTF8;Pooling=true;MinPoolSize=0;MaxPoolSize=10;Connection lifetime=30;";

        // GET: Home
        public ActionResult Index()
        {
            using (var conn = new FbConnection(connStr))
            {
                conn.Open();

                using (var tr = conn.BeginTransaction())
                {
                    using (var cmd = new FbCommand("SELECT * FROM RDB$RELATIONS", conn, tr))
                    {
                        
                    }
                }
            }

            return View();
        }
    }
}