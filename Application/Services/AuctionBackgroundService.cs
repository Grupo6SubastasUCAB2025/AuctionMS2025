using MSAuction.Application.Interfaces;

namespace MSAuction.Application.Services
{
    public class AuctionBackgroundService
    {
        private readonly IAuctionRepository _repository;

        public AuctionBackgroundService(IAuctionRepository repository)
        {
            _repository = repository;
        }

        public async Task FinalizeAuction(int auctionId)
        {
            var auction = await _repository.GetByIdAsync(auctionId);
            if (auction == null || auction.Status == "finalizada")
                return;

            if (auction.EndDate <= DateTime.UtcNow)
            {
                auction.MarkAsEnded();
                await _repository.UpdateAsync(auction);
            }
        }
    }
}
