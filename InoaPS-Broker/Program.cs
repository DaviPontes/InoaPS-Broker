using System;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace InoaPS_Broker
{
    class Program
    {
        static int interval = 5; //Interval in minutes
        static Stock stock;
        static Sender sender;
        static string emailSent = null;
        static async Task Main(string[] args)
        {
            try{
                if(args.Length != 3){
                    Console.WriteLine("Please enter correct parameteres! (stock_symbol min_value  max_value)");
                }else{
                    string path = "./appsettings.json";
                    dynamic config = loadJson(path);
                    double min_value = Double.Parse(args[1].Replace(",","."));
                    double max_value = Double.Parse(args[2].Replace(",","."));

                    stock = new Stock(args[0], (string)config.API_Key);
                    sender = new Sender(config.SMTP, config.Emails);
                    
                    while(true){
                        var response = await checkStock(min_value, max_value);

                        if(response != 0){
                            break;
                        }

                        Console.WriteLine("Waiting...");
                        await Task.Delay(interval*60000);
                    }
                }
            }catch{
                Console.WriteLine("Something went wrong! Check your parameters.");
            }
        }
        static private Newtonsoft.Json.Linq.JObject loadJson(string path){
            var reader = new StreamReader(path);
            dynamic result = JsonConvert.DeserializeObject(reader.ReadToEnd());
            return result;
        }

        static async private Task<int> checkStock(double min_value, double max_value){
            Console.WriteLine("* Checking Stock");

            double stockPrice = await stock.getQuote();

            if(stockPrice < 0){
                return 1;
            }

            if(stockPrice < min_value){
                Console.WriteLine(
                    "=> Buy - Stock below goal price! Goal: R$ {goal} | Current: R$ {current}"
                    .Replace("{goal}", min_value.ToString())
                    .Replace("{current}", stockPrice.ToString())
                );
                if(emailSent != "buy"){
                    sender.sendAll("buy", stock.getSymbol(), min_value, stockPrice);
                    emailSent = "buy";
                }
            }else if(stockPrice > max_value){
                Console.WriteLine(
                    "=> Sell - Stock above goal price! Goal: R$ {goal} | Current: R$ {current}"
                    .Replace("{goal}", max_value.ToString())
                    .Replace("{current}", stockPrice.ToString())
                );
                if(emailSent != "sell"){
                    sender.sendAll("sell", stock.getSymbol(), max_value, stockPrice);
                    emailSent = "sell";
                }
            }else{
                emailSent = null;
            }

            return 0;
        }
    }
}
