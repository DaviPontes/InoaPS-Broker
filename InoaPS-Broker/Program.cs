using System;
using System.IO;
using Newtonsoft.Json;

namespace InoaPS_Broker
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "./appsettings.json";
            dynamic result = loadJson(path);
            
            foreach(var email in result.emails){
                Console.WriteLine(email);
            }

            Stock.test();
            Sender.test();
        }
        static private Newtonsoft.Json.Linq.JObject loadJson(string path){
            var reader = new StreamReader(path);
            dynamic result = JsonConvert.DeserializeObject(reader.ReadToEnd());
            return result;
        }
    }
}
