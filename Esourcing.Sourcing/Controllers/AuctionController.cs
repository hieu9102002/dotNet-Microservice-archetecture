using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DnsClient.Internal;
using Esourcing.Sourcing.Entities;
using Esourcing.Sourcing.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Esourcing.Sourcing.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionRepository _repository;
        private readonly ILogger<AuctionController> _logger;
        public AuctionController(IAuctionRepository repository, ILogger<AuctionController> logger)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Auction>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Auction>>> GetAuctions()
        {
            var auctions = await _repository.GetAuctions();
            return Ok(auctions);
        }

        [HttpGet("{id:length(24)}")]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Auction>> GetAuction(string id)
        {
            var auction = await _repository.GetAuction(id);
            if (auction == null)
            {
                _logger.LogError($"Auction with id: {id} is not found");
                return NotFound();
            }
            return Ok(auction);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Auction),(int)HttpStatusCode.Created)]
        public async Task<ActionResult<Auction>> CreateAuction([FromBody] Auction auction)
        {
            await _repository.Create(auction);
            return CreatedAtRoute("GetAuction", new {id=auction.Id});
        }
        
        [HttpPut]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Auction>> UpdateAuction([FromBody] Auction auction)
        {
            return Ok(await _repository.Update(auction));
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Auction>> DeleteAuction(string id)
        {
            return Ok(await _repository.Delete(id));
        }
    }
}
