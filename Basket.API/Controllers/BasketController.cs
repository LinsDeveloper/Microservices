using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;

        public BasketController(IBasketRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);

            return Ok(basket ?? new ShoppingCart(userName));

        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart bascket)
        {
            return Ok(await _repository.UpdateBasket(bascket));

        }

        //[HttpPost]
        //public async Task<ActionResult<BasketCheckout>> FinalCheckout([FromBody] BasketCheckout checkout)
        //{

        //    var checkount = await _repository.FinalCheckout(checkout);
        //    return Ok(checkount ?? new BasketCheckout(checkout));

        //}

        [HttpDelete]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);

            return Ok();
        }

    }
}
