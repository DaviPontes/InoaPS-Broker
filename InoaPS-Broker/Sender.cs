using System;
using System.Net;
using System.Net.Mail;
using Newtonsoft.Json.Linq;

namespace InoaPS_Broker
{
    class Sender
    {
        private SmtpClient smtpClient;
        private JArray emailList;
        private string senderEmail;

        private JObject msgBuy = JObject.FromObject( 
            new {
                subject = "Sugestão de compra",
                body = "Sugerimos a compra da seguinte ação: {symbol}.\nPreço Alvo: R$ {goal} | Preço Atual: R$ {current}"
            }
        );
        private JObject msgSell = JObject.FromObject( 
            new {
                subject = "Sugestão de venda",
                body = "Sugerimos a venda da seguinte ação: {symbol}.\nPreço Alvo: R$ {goal} | Preço Atual: R$ {current}"
            }
        );

        public Sender(JObject SMTPConfig, JArray Emails){
            Console.WriteLine("-- New Sender Object");

            config(SMTPConfig, Emails);
        }

        public void config(JObject SMTPConfig, JArray Emails){
            Console.WriteLine("-- Setting up SMTP client");
            smtpClient = new SmtpClient((string)SMTPConfig["host"])
            {
                Port = (int)SMTPConfig["port"],
                Credentials = new NetworkCredential((string)SMTPConfig["email"], (string)SMTPConfig["password"]),
                EnableSsl = (bool)SMTPConfig["enable_ssl"]
            };

            emailList = Emails;

            senderEmail = (string)SMTPConfig["email"];
        }
        public void sendAll(string type, string symbol, double goalQuote, double currentQuote){
            string list = "";
            foreach(var email in emailList){
                list+=email;
                list+=", ";
            }
            list = list.Remove(list.Length-2);
            send(list, type, symbol, goalQuote, currentQuote);
        }

        public void send(string email, string type, string symbol, double goalQuote, double currentQuote){
            if(smtpClient != null){
                JObject message = type=="buy" ? msgBuy : msgSell;
                smtpClient.Send(
                    senderEmail, 
                    email, 
                    (string)message["subject"], 
                    ((string)message["body"])
                        .Replace("{symbol}", symbol)
                        .Replace("{goal}", goalQuote.ToString("N2"))
                        .Replace("{current}", currentQuote.ToString("N2"))
                );
            }else{
                Console.WriteLine("SMTP Client is null!");
            }
        }
    }
}