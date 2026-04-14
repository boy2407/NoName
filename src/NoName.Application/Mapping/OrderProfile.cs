using AutoMapper;
using NoName.Domain.Entities;
using NoName.Application.Features.Orders.Commands.CreateOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoName.Application.Features.Orders.Commands.UpdateOrder;

namespace NoName.Application.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile() {

            CreateMap<CreateOrderCommand,Order> ();
            CreateMap<UpdateOrderCommand,Order>();
            

        }
    }
}
