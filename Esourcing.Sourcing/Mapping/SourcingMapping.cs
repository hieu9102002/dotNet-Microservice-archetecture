using AutoMapper;
using Esourcing.Sourcing.Entities;
using EventBusRabbitMQ.Events;

namespace Esourcing.Sourcing.Mapping
{
    public class SourcingMapping : Profile
    {
        public SourcingMapping()
        {
            CreateMap<OrderCreateEvent, Bid>().ReverseMap();
        }
    }
}
