using System.Collections.Generic;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace web.Pages
{
    public class IndexModel : PageModel
    {
        private string connectionString = @"Database='d:\fbdata\mtnc.fdb';DataSource=localhost;User=sysdba;Password=masterkey;Dialect=3;Charset=UTF8;Pooling=true;MinPoolSize=0;MaxPoolSize=100;Connection lifetime=30;";

        public List<dynamic> Records { get; private set; }

        public async Task OnGet()
        {
            this.Records = new List<dynamic>();

            using (var conn = new FbConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, text, anchorUtc, duration, task, context FROM JobRecords";
                    var reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        this.Records.Add(new
                        {
                            id = reader["id"],
                            text = reader["text"],
                            anchorUtc = reader["anchorUtc"],
                            duration = reader["duration"],
                            task = reader["task"],
                            context = reader["context"]
                        });
                    }
                }
            }
        }
    }
}