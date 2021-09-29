using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.Fluent;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
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
            var groupName = "interns";
            var location = Region.EuropeNorth;
            // create the resource group
            //Console.WriteLine("Creating the resource group...");
            /*var resourceGroup = azure.ResourceGroups.Define(groupName)
                .WithRegion(location)
                .Create();*/
            // deploy the template
            Console.WriteLine("Deploying the template...");

            //var templatePath = "C:/Users/SonaChandrasekaran/source/repos/ARM_Trial/AzureResourceGroup1/azuredeploy.json";
            //var paramPath = "C:/Users/SonaChandrasekaran/source/repos/ARM_Trial/AzureResourceGroup1/azuredeploy.parameters.json";
            var templatePath = "https://blobsona.blob.core.windows.net/scont/template.json";
            var paramPath = "https://blobsona.blob.core.windows.net/scont/param.json";

            //var templatePath = "C:/Users/SonaChandrasekaran/Desktop/template.json";
            //var paramPath = "‪C:/Users/SonaChandrasekaran/Desktop/param.json";
            var deployment = azure.Deployments.Define("sonaDeployment")
                .WithExistingResourceGroup(groupName)
                .WithTemplateLink(templatePath, "1.0.0.0")
                .WithParametersLink(paramPath, "1.0.0.0")
                .WithMode(Microsoft.Azure.Management.ResourceManager.Fluent.Models.DeploymentMode.Incremental)
                .Create();

            // write machine info to log
            /* var vm = azure.VirtualMachines.GetByResourceGroup(groupName, "myVM");
             Console.WriteLine("Querying the VM...");
             Console.WriteLine(vm.Size.ToString());
             Console.WriteLine(vm.OSProfile.ComputerName.ToString());

             // delete the resource group
             Console.WriteLine("Deleting the resource group...");
             Console.WriteLine("Press enter to delete the resource group...");
             Console.ReadLine();
             azure.ResourceGroups.DeleteByName(groupName);*/
        }
    }
}
