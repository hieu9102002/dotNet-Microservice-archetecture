using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esourcing.Sourcing.Data.Interface;
using Esourcing.Sourcing.Entities;
using Esourcing.Sourcing.Repositories.Interfaces;
using MongoDB.Driver;

namespace Esourcing.Sourcing.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly ISourcingContext _context;

        public BidRepository(ISourcingContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Bid>> GetBidByAuctionId(string id)
        {
            var bids = await _context.Bids.Find(b => b.AuctionId == id).ToListAsync();
            bids = bids.OrderByDescending(b => b.CreatedAt)
                .GroupBy(b=>b.SellerUserName)
                .Select(a => new Bid
                {
                    AuctionId = a.FirstOrDefault().AuctionId,
                    Price = a.FirstOrDefault().Price,
                    CreatedAt = a.FirstOrDefault().CreatedAt,
                    SellerUserName = a.FirstOrDefault().SellerUserName,
                    ProductId = a.FirstOrDefault().ProductId,
                    Id = a.FirstOrDefault().Id
                })
                .ToList();
            return bids;
        }

        public async Task<Bid> GetWinnerBid(string id)
        {
            var bids = await GetBidByAuctionId(id);
            return bids.FirstOrDefault();
        }

        public async Task SendBid(Bid bid)
        {
            await _context.Bids.InsertOneAsync(bid);
        }
    }
}
