using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace InoaPS_Broker
{
    class Stock
    {
        private HttpClient httpClient = new HttpClient();
        private string requestURL = "https://apidojo-yahoo-finance-v1.p.rapidapi.com/market/v2/get-quotes?region=BR&symbols={symbol}.SA";        
        private string apiKey;
        private string symbol;

        public Stock(string symbol, string apiKey){
            Console.WriteLine("-- New Stock Object");

            this.symbol = symbol;
            this.apiKey = apiKey;
        }
        public async Task<double> getQuote(){
            Console.WriteLine("* Getting stock quote");

            var request = createRequest();

            request.RequestUri = new Uri(requestURL.Replace("{symbol}", symbol));
            using (var response = await httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                try{
                    var obj = JObject.Parse(body)["quoteResponse"]["result"][0];
                    return (double)obj["regularMarketPrice"];
                }catch{
                    Console.WriteLine("Stock not found!");
                    return -1;
                }
            };
        }

        public string getSymbol(){
            return symbol;
        }

        private HttpRequestMessage createRequest(){
            return new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                Headers =
                {
                    { "x-rapidapi-host", "apidojo-yahoo-finance-v1.p.rapidapi.com" },
                    {"x-rapidapi-key", apiKey}
                }
            };
        }
    }
}