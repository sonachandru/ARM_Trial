using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.Fluent;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;

namespace Arm_api.Controllers
{
    public class ARMController : ApiController
    {
        [HttpPost]
        [Route("api/check")]
        public void armcheck(pro res)
        {
            

            var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(
                 "a325d85b-705c-4c2d-b5a9-f35c08ec9cbc" // enter clientId here, this is the ApplicationID
                 , "a==wI0nwLvsm5?hA?YdiDhaYvThp0V6e" // this is the Application secret key
                 , "fa3c421e-2fde-4cf9-945d-62735cedcad2" // this is the tenant id 
                 , AzureEnvironment.AzureGlobalCloud);

            var azure = Microsoft.Azure.Management.Fluent.Azure
                .Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(credentials)
                .WithDefaultSubscription();
            var groupName = "Interns";
            var location = Region.EuropeNorth;


            Console.WriteLine("Deploying the template...");

           
            string tjson = File.ReadAllText("C:/Users/SonaChandrasekaran/source/repos/ARM_Trial/Arm_api/template.json");
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(tjson);
            jsonObj["variables"]["functionStorageAccountName"] = res.StorageAccount;
            jsonObj["parameters"]["sonadbName"]["defaultValue"] = res.SQLDatabase;
            jsonObj["variables"]["sonawebappName"] =res.Webapp;
            jsonObj["variables"]["sitesFunctionAppName"] = res.FunctionApp;
            
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            var x = JObject.Parse(output);
          
       
                azure.Deployments.Define("SonaDeployment")
                .WithExistingResourceGroup(groupName)
                .WithTemplate(x.ToString())
                .WithParameters("{}")
                .WithMode(Microsoft.Azure.Management.ResourceManager.Fluent.Models.DeploymentMode.Incremental)
                .Create();

        }

        [HttpGet]
        [Route("api/status")]

        public void Status()
        {
            SqlConnection conn = new SqlConnection(@"Data Source=KOVLTP169\SQLEXPRESS;Initial Catalog = fnapp; Integrated security = True");

            DataTable dt = new DataTable();
            string query = @"insert into appstatus values('Starting')";
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
