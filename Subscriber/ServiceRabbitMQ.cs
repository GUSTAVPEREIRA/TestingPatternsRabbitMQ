namespace Subscriber;

using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class ServiceRabbitMq : IHostedService, IDisposable
{
    private IModel channel;
    private IConnection connection;

    // Initiate RabbitMQ and start listening to an input queue
    private void Run()
    {
        // ! Fill in your data here !
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            VirtualHost = "/",
            Port = 5672,
            UserName = "guest",
            Password = "guest",
        };

        connection = factory.CreateConnection();
        channel = connection.CreateModel();
        Console.WriteLine(" [*] Waiting for messages.");

        var consumer = new EventingBasicConsumer(channel);
        var consumerTag = channel.BasicConsume("my.queue1", false, consumer);
        consumer.Received += (sender, e) =>
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());
            Console.WriteLine(" [x] Received {0}", message);
            
            
            
            channel.BasicAck(e.DeliveryTag, false);
        };
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Run();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Run();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        channel.Dispose();
        connection.Dispose();
    }
}