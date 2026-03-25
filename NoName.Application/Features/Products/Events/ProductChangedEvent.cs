using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Events
{
    public record ProductChangedEvent : INotification
    {
        public int ProductId { get; init; }
        public ProductChangedEvent(int ProductId)
        {
            this.ProductId = ProductId;
        }

    }
}
