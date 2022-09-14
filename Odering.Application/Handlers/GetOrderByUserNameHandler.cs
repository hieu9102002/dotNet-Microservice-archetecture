using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Domain.Repositories;

namespace Ordering.Application.Handlers
{
    public class GetOrderByUserNameHandler : IRequestHandler<GetOrdersByUsernameQuery, IEnumerable<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public GetOrderByUserNameHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<OrderResponse>> Handle(GetOrdersByUsernameQuery request, CancellationToken cancellationToken)
        {
            var orderList = await _orderRepository.GetOrdersBySellerUserName(request.UserName);
            var responses = _mapper.Map<IEnumerable<OrderResponse>>(orderList);
            return responses;
        }
    }
}
