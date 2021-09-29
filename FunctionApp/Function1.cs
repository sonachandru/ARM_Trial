using System;
using System.Data;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            SqlConnection conn = new SqlConnection(@"Data Source=KOVLTP169\SQLEXPRESS;Initial Catalog = fnapp; Integrated security = True");

            DataTable dt = new DataTable();
            string query = @"update appstatus set fnstatus='Running' where fnstatus='Starting'";
            using (var con = new SqlConnection(@"Data Source=KOVLTP169\SQLEXPRESS;Initial Catalog = fnapp; Integrated security = True"))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(dt);
            }
        }
    }
}
