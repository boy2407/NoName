using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Orders.Events
{
    public record OrderCreatedEvent : INotification
    {
        public int OrderId { get; set; }
        public string CustomerEmail { get; set; }
        public  string ProductName { get; set; }


    }
}
