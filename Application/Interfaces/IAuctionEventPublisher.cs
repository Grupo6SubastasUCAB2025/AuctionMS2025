using MSAuction.Infraestructure.EventBus.Events;

namespace MSAuction.Application.Interfaces
{
    public interface IAuctionEventPublisher
    {
        void PublishAuctionUpdatedEvent(AuctionUpdatedEvent auctionUpdatedEvent);
    }

}
