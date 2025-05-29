namespace MSAuction.Application.Interfaces
{
    public interface IAuctionFinalizer
    {
        Task FinalizeAuctionAsync(int auctionId);
    }
}
