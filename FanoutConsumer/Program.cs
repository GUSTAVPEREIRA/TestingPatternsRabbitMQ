// See https://aka.ms/new-console-template for more information


using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


public class Program
{
    public static IConnection conn { get; set; }
    public static IModel channel { get; set; }

    public static void Main()
    {
        var factory = new ConnectionFactory();
        factory.HostName = "localhost";
        factory.VirtualHost = "/";
        factory.Port = 5672;
        factory.UserName = "guest";
        factory.Password = "guest";

        conn = factory.CreateConnection();
        channel = conn.CreateModel();

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += ConsumerReceiver;
        var consumerTag = channel.BasicConsume("my.queue2", true, consumer);
        Console.WriteLine("Waiting messages. Press any key!");
        Console.ReadKey();
    }

    static void ConsumerReceiver(object sender, BasicDeliverEventArgs e)
    {
        string message = Encoding.UTF8.GetString(e.Body.ToArray());
        Console.WriteLine($"Message: {message}");
        channel.BasicAck(e.DeliveryTag, false);
    }
}