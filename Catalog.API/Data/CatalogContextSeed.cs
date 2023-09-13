using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {

        public static void SendData(IMongoCollection<Product> productCollection)
        {
            bool existProcut = productCollection.Find(p => true).Any();
            if (!existProcut)
            {
                productCollection.InsertManyAsync(GetMyProducts());
            }
        }


        private static IEnumerable<Product> GetMyProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f5",
                    Name = "IPhone X",
                    Description = "gnfjdksngjkf;dnsjgkfsfgsg",
                    Image = "Product-1.png",
                    Price = 950.00M,
                    Category = "Smart Phone"

                },

                new Product()
                {
                    Id = "602v2249e72332a6990b47f1",
                    Name = "IPhone MAX",
                    Description = "gnfjdksn54635436543654365dnsjgkfsfgsg",
                    Image = "Product-1.png",
                    Price = 950.00M,
                    Category = "Smart Phone"
                }
            };
        }
    }
}
