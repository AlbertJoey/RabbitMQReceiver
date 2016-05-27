using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;

namespace testRabbitMQReciver
{
    class Program
    {
        static void Main(string[] args)
        {
            #region "Hello World!"
            //var factory = new ConnectionFactory() { HostName = "192.168.100.37", Password = "Git123!@#", UserName = "GitAdmin", Port = 5672 };

            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            //    channel.QueueDeclare(queue: "hello",
            //                         durable: false,
            //                         exclusive: false,
            //                         autoDelete: false,
            //                         arguments: null);

            //    var consumer = new EventingBasicConsumer(channel);
            //    consumer.Received += (model, ea) =>
            //    {
            //        var body = ea.Body;
            //        var message = Encoding.UTF8.GetString(body);
            //        Console.WriteLine(" [x] Received {0}", message);
            //    };
            //    channel.BasicConsume(queue: "hello",
            //                         noAck: true,
            //                         consumer: consumer);

            //    Console.WriteLine(" Press [enter] to exit.");
            //    Console.ReadLine();

            //}
            #endregion

            #region  Work queues
            //var factory = new ConnectionFactory() { HostName = "192.168.100.37", Password = "Git123!@#", UserName = "GitAdmin", Port = 5672 };
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            //    channel.QueueDeclare(queue: "task_queue",
            //                         durable: true,
            //                         exclusive: false,
            //                         autoDelete: false,
            //                         arguments: null);

            //    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            //    Console.WriteLine(" [*] Waiting for messages.");

            //    var consumer = new EventingBasicConsumer(channel);
            //    consumer.Received += (model, ea) =>
            //    {
            //        var body = ea.Body;
            //        var message = Encoding.UTF8.GetString(body);
            //        Console.WriteLine(" [x] Received {0}", message);

            //        int dots = message.Split('.').Length - 1;
            //        Thread.Sleep(dots * 1000);

            //        Console.WriteLine(" [x] Done");

            //        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            //    };
            //    channel.BasicConsume(queue: "task_queue",
            //                         noAck: false,
            //                         consumer: consumer);

            //    Console.WriteLine(" Press [enter] to exit.");
            //    Console.ReadLine();
            //}
            #endregion

            #region Publish/Subscribe
            //var factory = new ConnectionFactory() { HostName = "192.168.100.37", Password = "Git123!@#", UserName = "GitAdmin", Port = 5672 };
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            //    channel.ExchangeDeclare(exchange: "logs", type: "fanout");

            //    var queueName = channel.QueueDeclare().QueueName;
            //    channel.QueueBind(queue: queueName,
            //                      exchange: "logs",
            //                      routingKey: "");

            //    Console.WriteLine(" [*] Waiting for logs.");

            //    var consumer = new EventingBasicConsumer(channel);
            //    consumer.Received += (model, ea) =>
            //    {
            //        var body = ea.Body;
            //        var message = Encoding.UTF8.GetString(body);
            //        Console.WriteLine(" [x] {0}", message);
            //    };
            //    channel.BasicConsume(queue: queueName,
            //                         noAck: true,
            //                         consumer: consumer);

            //    Console.WriteLine(" Press [enter] to exit.");
            //    Console.ReadLine();
            //}
            #endregion

            #region  Routing
            //var  factory = new ConnectionFactory() { HostName = "192.168.100.37", Password = "Git123!@#", UserName = "GitAdmin", Port = 5672 };
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            //    channel.ExchangeDeclare(exchange: "direct_logs",
            //                            type: "direct");
            //    var queueName = channel.QueueDeclare().QueueName;

               
            //    args = new string[] { "error", "info", "warning" };

            //    foreach (var severity in args)
            //    {
            //        channel.QueueBind(queue: queueName,
            //                          exchange: "direct_logs",
            //                          routingKey: severity);
            //    }

            //    Console.WriteLine(" [*] Waiting for messages.");

            //    var consumer = new EventingBasicConsumer(channel);
            //    consumer.Received += (model, ea) =>
            //    {
            //        var body = ea.Body;
            //        var message = Encoding.UTF8.GetString(body);
            //        var routingKey = ea.RoutingKey;
            //        Console.WriteLine(" [x] Received '{0}':'{1}'",
            //                          routingKey, message);
            //    };
            //    channel.BasicConsume(queue: queueName,
            //                         noAck: true,
            //                         consumer: consumer);

            //    Console.WriteLine(" Press [enter] to exit.");
            //    Console.ReadLine();
            //}
            #endregion

            #region Topics
            var factory = new ConnectionFactory() { HostName = "192.168.100.37", Password = "Git123!@#", UserName = "GitAdmin", Port = 5672 };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");
                var queueName = channel.QueueDeclare().QueueName;

                if (args.Length < 1)
                {
                    Console.Error.WriteLine("Usage: {0} [binding_key...]",
                                            Environment.GetCommandLineArgs()[0]);
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                    Environment.ExitCode = 1;
                    return;
                }

                foreach (var bindingKey in args)
                {
                    channel.QueueBind(queue: queueName,
                                      exchange: "topic_logs",
                                      routingKey: bindingKey);
                }

                Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;
                    Console.WriteLine(" [x] Received '{0}':'{1}'",
                                      routingKey,
                                      message);
                };
                channel.BasicConsume(queue: queueName,
                                     noAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
            #endregion
        }
    }
}
