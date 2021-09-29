using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Rest.Azure.Authentication;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Arm_api
{
    public class pro
    {
        public string Webapp { get; set; }
        public string StorageAccount { get; set; }
        public string FunctionApp { get; set; }
        public string SQLDatabase { get; set; }

    }
}
