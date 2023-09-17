using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderPublisher.Entities
{
    public class Ticket
    {
        public string UserName { get; set; }
        public DateTime Booked { get; set; }

        public string Location { get; set; }
    }
}
