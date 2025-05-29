using MSAuction.Application.Interfaces;

namespace MSAuction.Application.Services
{
    public class AuctionFinalizer : IAuctionFinalizer
    {
        private readonly IAuctionRepository _repository;

        public AuctionFinalizer(IAuctionRepository repository)
        {
            _repository = repository;
        }

        public async Task FinalizeAuctionAsync(int auctionId)
        {
            var auction = await _repository.GetByIdAsync(auctionId);
            if (auction is null || auction.Status != "pending") return;

            auction.MarkAsEnded(); // Método de dominio que cambia el estado
            await _repository.UpdateAsync(auction);

            // Aquí también podrías publicar un evento a RabbitMQ
        }
    }
}
