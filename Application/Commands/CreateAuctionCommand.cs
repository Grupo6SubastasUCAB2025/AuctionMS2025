using MSAuction.Application.DTOs;
using MediatR;

namespace MSAuction.Application.Commands
{
    public class CreateAuctionCommand : IRequest<int> // Devuelve el ID int (serial en PostgreSQL)
    {
        public AuctionDto Auction { get; set; }
        public int UserId { get; set; } // Usuario creador

        public CreateAuctionCommand(AuctionDto auction, int userId)
        {
            Auction = auction;
            UserId = userId;
        }
    }
}
