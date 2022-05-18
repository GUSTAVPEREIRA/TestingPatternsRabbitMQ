// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;

IConnection conn;
IModel channel;
var factory = new ConnectionFactory();
factory.HostName = "localhost";
factory.VirtualHost = "/";
factory.Port = 5672;
factory.UserName = "guest";
factory.Password = "guest";

IDictionary<string, object> arg = new Dictionary<string, object>
{
    { "x-delayed-type", "direct" }
};


conn = factory.CreateConnection();
channel = conn.CreateModel();
var basicProperties = channel.CreateBasicProperties();
basicProperties.Headers = new Dictionary<string, object> { { "x-delay", 8000 } };
channel.ExchangeDeclare("ex.fanout", "fanout", true, false, null);

channel.QueueDeclare("my.queue1", true, false, false, null);

channel.QueueBind("my.queue1", "ex.fanout", "delayBind");
channel.BasicPublish("ex.fanout", "delayBind", basicProperties, Encoding.UTF8.GetBytes("message1"));

 channel.QueueDelete("my.queue1");
 channel.QueueDelete("my.queue2");
//
// channel.Close();
// channel.Close();