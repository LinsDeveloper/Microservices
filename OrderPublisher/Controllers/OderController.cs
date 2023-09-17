using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderPublisher.Entities;

namespace OrderPublisher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OderController : ControllerBase
    {

        private readonly IBus _bus;

        public OderController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<ActionResult> CreateTicket(Ticket ticket)
        {
            if(ticket != null)
            {
                ticket.Booked = DateTime.Now;
                Uri uri = new Uri("rabbitmq://localhost/orderTicketQueue");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(ticket);
                return Ok();
            }

            return BadRequest();
        }




    }
}
