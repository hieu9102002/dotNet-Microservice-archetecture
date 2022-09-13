using System.Collections.Generic;
using System.Threading.Tasks;
using Esourcing.Sourcing.Data;
using Esourcing.Sourcing.Data.Interface;
using Esourcing.Sourcing.Entities;
using Esourcing.Sourcing.Repositories.Interfaces;
using MongoDB.Driver;

namespace Esourcing.Sourcing.Repositories
{
    public class AuctionRepository:IAuctionRepository
    {
        private readonly ISourcingContext _context;
        public AuctionRepository(ISourcingContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Auction>> GetAuctions()
        {
            return await _context.Auctions.Find(p => true).ToListAsync();
        }

        public async Task<Auction> GetAuction(string id)
        {
            return await _context.Auctions.Find(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Auction> GetAuctionByName(string name)
        {
            var filter = Builders<Auction>.Filter.Eq(a => a.Name, name);
            return await _context.Auctions.Find(filter).FirstOrDefaultAsync();
        }

        public async Task Create(Auction auction)
        {
            await _context.Auctions.InsertOneAsync(auction);
        }

        public async Task<bool> Update(Auction auction)
        {
            var filter = Builders<Auction>.Filter.Eq(a => a.Id, auction.Id);
            var updateResult = await _context.Auctions.ReplaceOneAsync(filter, auction);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            DeleteResult deleteResult = await _context.Auctions.DeleteOneAsync(a => a.Id == id);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
