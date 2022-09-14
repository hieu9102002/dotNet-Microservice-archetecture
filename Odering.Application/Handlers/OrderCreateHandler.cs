using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Ordering.Application.Commands.OrderCreate;
using Ordering.Application.Responses;
using Ordering.Domain.Entities;
using Ordering.Domain.Repositories;

namespace Ordering.Application.Handlers
{
    public class OrderCreateHandler :IRequestHandler<OrderCreateCommand, OrderResponse>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public OrderCreateHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _repository = orderRepository;
            _mapper = mapper;
        }
        public async Task<OrderResponse> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            if (orderEntity == null) throw new ApplicationException("Entity could not be mapped");
            var response = await _repository.AddAsync(orderEntity);

            return _mapper.Map<OrderResponse>(response);
        }
    }
}
