using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DnsClient.Internal;
using Esourcing.Sourcing.Entities;
using Esourcing.Sourcing.Repositories.Interfaces;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Esourcing.Sourcing.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AuctionController> _logger;
        private readonly EventBusRabbitMQProducer _eventBus;
        public AuctionController(
            IAuctionRepository auctionRepository,
            IBidRepository bidRepository,
            IMapper mapper, 
            ILogger<AuctionController> logger,
            EventBusRabbitMQProducer eventBus)
        {
            _mapper = mapper;
            _logger = logger;
            _eventBus = eventBus;
            _bidRepository = bidRepository;
            _auctionRepository = auctionRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Auction>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Auction>>> GetAuctions()
        {
            var auctions = await _auctionRepository.GetAuctions();
            return Ok(auctions);
        }

        [HttpGet("{id:length(24)}")]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Auction>> GetAuction(string id)
        {
            var auction = await _auctionRepository.GetAuction(id);
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
            await _auctionRepository.Create(auction);
            return CreatedAtRoute("GetAuction", new {id=auction.Id});
        }
        
        [HttpPut]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Auction>> UpdateAuction([FromBody] Auction auction)
        {
            return Ok(await _auctionRepository.Update(auction));
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Auction>> DeleteAuction(string id)
        {
            return Ok(await _auctionRepository.Delete(id));
        }

        [HttpPost("CompleteAuction/{id:length(24)}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(OrderCreateEvent), (int)HttpStatusCode.Accepted)]
        public async Task<ActionResult<OrderCreateEvent>> CompleteAuction(string id)
        {
            var auction = await _auctionRepository.GetAuction(id);
            if (auction == null) return NotFound();

            if (auction.Status != (int) Status.Active)
            {
                _logger.LogError("Auction cannot be completed");
                return BadRequest();
            }

            var winnerBid = await _bidRepository.GetWinnerBid(id);
            if (winnerBid == null) return NotFound();

            var eventMessage = _mapper.Map<OrderCreateEvent>(winnerBid);
            eventMessage.Quantity = auction.Quantity;

            auction.Status = (int) Status.Closed;
            var updateResponse = await _auctionRepository.Update(auction);
            if (!updateResponse)
            {
                _logger.LogError("Auction cannot be updated");
                return BadRequest();
            }

            try
            {
                _eventBus.Publish(EventBusConstants.OrderCreateQueue, eventMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR Publishing integration event: {eventMessage.Id}, from Sourcing");
                throw;
            }

            return Accepted(eventMessage);
        }

        [HttpPost("TestEvent")]
        public ActionResult<OrderCreateEvent> TestEvent()
        {
            var eventMessage = new OrderCreateEvent()
            {
                AuctionId = "Dummy1",
                ProductId = "Dummy_Product_1",
                Price = 10,
                Quantity = 100,
                SellerUserName = "test@test.com"
            };

            try
            {
                _eventBus.Publish(EventBusConstants.OrderCreateQueue, eventMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR Publishing integration event: {eventMessage.Id}, from Sourcing");
                throw;
            }

            return Accepted(eventMessage);
        }
    }
}
