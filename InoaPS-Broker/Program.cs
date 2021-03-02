using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace InoaPS_Broker
{
    class Program
    {
        static void Main(string[] args)
        {
            //IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();

            string path = "./appsettings.json";
            var reader = new StreamReader(path);

            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(reader.ReadToEnd());
            
            foreach(var email in result.emails){
                Console.WriteLine(email);
            }
        }
    }
}
