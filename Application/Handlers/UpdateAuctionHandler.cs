using MediatR;
using MSAuction.Application.Commands;
using MSAuction.Application.Interfaces;
using MSAuction.Infraestructure.EventBus.Events;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;

namespace MSAuction.Application.Handlers
{
    public class UpdateAuctionHandler : IRequestHandler<UpdateAuctionCommand, bool>
    {
        private readonly IAuctionRepository _repository;
        private readonly IAuctionEventPublisher _eventPublisher;

        public UpdateAuctionHandler(IAuctionRepository repository, IAuctionEventPublisher eventPublisher)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
        }

        public async Task<bool> Handle(UpdateAuctionCommand request, CancellationToken cancellationToken)
        {
            var auction = await _repository.GetByIdAsync(request.AuctionId);

            if (auction == null || auction.UserId != request.UserId)
            {
                return false;  // La subasta no existe o no es el propietario
            }

            // Validar que no se esté modificando una subasta ya comenzada
            if (auction.Status == "active" || auction.Status == "finalizada")
            {
                return false;
            }

            // Actualizar los campos
            auction.Title = request.AuctionDto.Title;
            auction.Description = request.AuctionDto.Description;
            auction.InitialPrice = request.AuctionDto.InitialPrice;
            auction.MinIncrement = request.AuctionDto.MinIncrement;
            auction.ReservePrice = request.AuctionDto.ReservePrice;
            auction.Conditions = request.AuctionDto.Conditions;
            auction.Type = request.AuctionDto.Type;
            auction.StartDate = request.AuctionDto.StartTime;
            auction.EndDate = request.AuctionDto.EndTime;

            // Guardar los cambios
            await _repository.UpdateAsync(auction);

            var auctionUpdatedEvent = new AuctionUpdatedEvent
            {
                AuctionId = auction.Id,
                Title = auction.Title,
                InitialPrice = auction.InitialPrice,
                EndDate = auction.EndDate,
                Status = auction.Status
            };

            _eventPublisher.PublishAuctionUpdatedEvent(auctionUpdatedEvent);

            return true;
        }
    }
}
