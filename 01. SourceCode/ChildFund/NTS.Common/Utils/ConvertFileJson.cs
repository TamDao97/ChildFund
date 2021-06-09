using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace NTS.Common.Utils
{
   public static class ConvertFileJson<T>
    { 

        public static T ReadFile(string file)
        {
            string json = File.ReadAllText(HostingEnvironment.MapPath(file), System.Text.Encoding.UTF8);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
