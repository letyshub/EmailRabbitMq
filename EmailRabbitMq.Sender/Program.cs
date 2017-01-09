using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using EmailRabbitMq.Shared;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EmailRabbitMq.Sender
{ 
    public class Program
    {
        public static void Main(string[] args)
        {
            EmailSender sender = CreateEmailSender();

            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "email_queue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                BasicGetResult result = channel.BasicGet("email_queue", true);
                
                while (result != null)
                {
                    var json = Encoding.UTF8.GetString(result.Body);
                    var msg = JsonConvert.DeserializeObject<EmailQueueMessage>(json);

                    Console.WriteLine("Email from queue:");
                    Console.WriteLine(json);

                    sender.Send(msg);

                    result = channel.BasicGet("email_queue", true);
                }
            }
        }

        public static EmailSender CreateEmailSender()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfigurationRoot config = builder.Build();

            return new EmailSender(config["server"], config["port"], config["user"], config["password"]);
        }
    }
}
