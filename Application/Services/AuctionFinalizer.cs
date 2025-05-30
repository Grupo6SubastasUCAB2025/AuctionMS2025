﻿using MassTransit;
using MSAuction.Application.Interfaces;
using MSAuction.Infraestructure.EventBus.Events;

namespace MSAuction.Application.Services
{
    public class AuctionFinalizer : IAuctionFinalizer
    {
        private readonly IAuctionRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;

        public AuctionFinalizer(IAuctionRepository repository, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task FinalizeAuctionAsync(int auctionId)
        {
            var auction = await _repository.GetByIdAsync(auctionId);
            if (auction is null || auction.Status != "pending") return;

            auction.MarkAsEnded(); // Método de dominio que cambia el estado
            await _repository.UpdateAsync(auction);

            await _publishEndpoint.Publish(new AuctionEndedEvent(auctionId));
        }
    }
}
