using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Esourcing.Sourcing.Entities;

namespace Esourcing.Sourcing.Repositories.Interfaces
{
    public interface IBidRepository
    {
        public Task<IEnumerable<Bid>> GetBidByAuctionId(string id);
        public Task<Bid> GetWinnerBid(string id);
        public Task SendBid(Bid bid);
    }
}
