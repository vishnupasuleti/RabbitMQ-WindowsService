using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQDemo.Common;
using System;
using System.Text;

namespace RabbitMQDemo.Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            const string queueName = "RabbitMqDemo";
            const string hostName = "localhost";

            var factory = new ConnectionFactory() { HostName = hostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                channel.QueueDeclare(queue: queueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var sampleData = new SampleData
                {
                    Message = "Hello World!"
                };

                var queueItem = new QueueItem
                {
                    Category = QueueItemCategory.Sample,
                    Data = JsonConvert.SerializeObject(sampleData)
                };
                
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(queueItem));

                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2;

                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: properties,
                                     body: body);

                Console.WriteLine(" [x] Sent");
            }

            Console.WriteLine(" Press [enter] to exit.");
        }
    }
}
