using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Ordering.Application.Responses;

namespace Ordering.Application.Queries
{
    public class GetOrdersByUsernameQuery : IRequest<IEnumerable<OrderResponse>>
    {
        public string UserName { get; set; }

        public GetOrdersByUsernameQuery(string userName)
        {
            UserName = userName;
        }
    }
}
