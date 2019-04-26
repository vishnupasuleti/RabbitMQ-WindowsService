using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQDemo.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo.WindowsService
{
    public class RabbitMQManager
    {
        IConnection connection;
        IModel channel;
        public void Setup()
        {
            var queueName = ConfigurationManager.AppSettings["queueName"];
            var hostName = ConfigurationManager.AppSettings["rabbitMQHostName"];

            var factory = new ConnectionFactory() { HostName = hostName };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(queueName, true, false, false, null);
            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (s, e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body);
                DoTask(message);

                channel.BasicAck(e.DeliveryTag, false);
            };

            channel.BasicConsume(queueName, false, consumer);
        }



        public void Release()
        {
            if (this.channel != null && this.channel.IsOpen)
            {
                this.channel.Close();
            }

            if (this.connection != null && this.connection.IsOpen)
            {
                this.connection.Close();
            }
        }

        private void DoTask(string message)
        {
            try
            {
                var queueItem = JsonConvert.DeserializeObject<QueueItem>(message);
                if (queueItem.Category == QueueItemCategory.Sample)
                {
                    //Do your task
                }
            }
            catch (Exception)
            {
                //Log Exception
            }
        }
    }
}
