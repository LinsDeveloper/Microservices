using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderPublisherRMQ.Domain;
using System.Text;
using RabbitMQ.Client;
using System.Text.Json;

namespace OrderPublisherRMQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private ILogger<OrderController> _logger;


        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }


        public IActionResult InsertOrder(Order order)
        {
            try
            {

                var factory = new ConnectionFactory { HostName = "localhost" };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();


                channel.QueueDeclare(queue: "orderQueue",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

                string message = JsonSerializer.Serialize(order);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "orderQueue",
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine($" [x] Sent {message}");

                

                return Accepted(order);
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao tentar publicar na fila", ex);

                return new StatusCodeResult(500);
            }
        }


    }
}
