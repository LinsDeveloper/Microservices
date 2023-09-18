using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using RabbitMQ.Client;
using System.Net.Mail;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {

        private readonly IDistributedCache _redisCache;


        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }

        public async Task<ShoppingCart?> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            if (String.IsNullOrEmpty(basket))
            {
                return null;
            }


            return JsonSerializer.Deserialize<ShoppingCart>(basket);

        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {

            
            await _redisCache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));

            return await GetBasket(basket.UserName);
        }

        public async Task<BasketCheckout> FinalCheckout(BasketCheckout checkout)
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

                string message = JsonSerializer.Serialize(checkout);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "orderQueue",
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine($" [x] Sent {message}");


            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao tentar publicar na fila", ex);

            }


            await _redisCache.RemoveAsync(checkout.UserName);

            return new BasketCheckout (checkout) { 
                UserName = checkout.UserName, 
                TotalPrice = checkout.TotalPrice, 
                FirstName = checkout.FirstName,
                LastName = checkout.LastName,
                EmailAddress = checkout.EmailAddress,
                AddressLine = checkout.AddressLine,
                Country = checkout.Country,
                State = checkout.State,
                ZipCode = checkout.ZipCode,
                CardName = checkout.CardName,
                CardNumber = checkout.CardNumber,
                Expiration = checkout.Expiration,
                CVV = checkout.CVV,
                PaymentMethod = checkout.PaymentMethod
            };


        }

    }
}
