using Dapper;
using Discount.API.Entities;
using Npgsql;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {

        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        

        public async Task<bool> CreateDiscount(Coupon coupon)
        {

            NpgsqlConnection connection = GetConnectionPostgreSQL();

            var effected = await connection.ExecuteAsync("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            if (effected == 0)
            {
                return false;
            }
            return true;

            
        }

        private NpgsqlConnection GetConnectionPostgreSQL()
        {
            return new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            
        }


        public async Task<Coupon> getAllDiscount()
        {

            NpgsqlConnection connection = GetConnectionPostgreSQL();



            var coupon = await connection.QueryAsync<Coupon>("SELECT TOP 10 * FROM Coupon");
            if (coupon == null)
            {
                return new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Description Desc" };
            }
            return (Coupon)coupon;

        }
        public async Task<Coupon> getDiscount(string productName)
        {

            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connectionMQ = factory.CreateConnection();
            using var channel = connectionMQ.CreateModel();

            channel.QueueDeclare(queue: "orderQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);


            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var order = System.Text.Json.JsonSerializer.Deserialize<BascketChekount>(message);
            };
            channel.BasicConsume(queue: "orderQueue",
                                 autoAck: true,
                                 consumer: consumer);



            string nomeProduct = order;

            await CreateDiscount(coupon);




            NpgsqlConnection connection = GetConnectionPostgreSQL();



            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });
            if(coupon == null)
            {
                return new Coupon{ ProductName = "No Discount", Amount = 0, Description = "No Description Desc"};
            }

            return coupon;

        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            NpgsqlConnection connection = GetConnectionPostgreSQL();

            var effected = await connection.ExecuteAsync("UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

            if (effected == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            NpgsqlConnection connection = GetConnectionPostgreSQL();

            var effected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName});

            if (effected == 0)
            {
                return false;
            }
            return true;
        }
    }
    }

