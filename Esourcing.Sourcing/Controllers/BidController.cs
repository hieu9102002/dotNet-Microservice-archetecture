using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Esourcing.Sourcing.Entities;
using Esourcing.Sourcing.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Esourcing.Sourcing.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBidRepository _repository;
        public BidController(IBidRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("GetBidByAuctionId/{auctionId:length(24)}")]
        [ProducesResponseType(typeof(IEnumerable<Bid>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Bid>>> GetBidsByAuctionId(string auctionId)
        {
            var bids = await _repository.GetBidByAuctionId(auctionId);
            return Ok(bids);
        }

        [HttpGet("GetWinnerBid/{auctionId:length(24)}")]
        [ProducesResponseType(typeof(Bid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Bid>> GetWinnerBid(string auctionId)
        {
            return Ok(await _repository.GetWinnerBid(auctionId));
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> SendBid([FromBody] Bid bid)
        {
            await _repository.SendBid(bid);
            return Ok();
        }
    }
}