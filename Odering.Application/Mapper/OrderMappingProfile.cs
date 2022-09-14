using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ordering.Application.Commands.OrderCreate;
using Ordering.Application.Responses;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mapper
{
    public class OrderMappingProfile :Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderCreateCommand>().ReverseMap();
            CreateMap<Order, OrderResponse>().ReverseMap();
        }
    }
}
