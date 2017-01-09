using System;
using System.Text;
using RabbitMQ.Client;
using EmailRabbitMq.Shared;
using Newtonsoft.Json;

namespace EmailRabbitMq.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
        
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "email_queue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                EmailQueueMessage message = new EmailQueueMessage
                {
                    RecipientEmail = "test@test-email.com",
                    RecipientName = "Test",
                    SenderName = "Rocky Balboa",
                    SenderEmail = "rocky.balboa@test-email.com",
                    Subject = "Test"
                };

                string json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "",
                                     routingKey: "email_queue",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine("Sent email: {0}", json);
            }
        }
    }
}
