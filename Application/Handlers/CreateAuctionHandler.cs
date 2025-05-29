using Hangfire;
using MediatR;
using RabbitMQ.Client;
using Newtonsoft.Json;
using MSAuction.Application.Commands;
using System.Text;
using MSAuction.Domain.Entities;
using MSAuction.Application.Interfaces;
using MSAuction.Application.Services;
using MSAuction.Infraestructure.EventBus.Events;

namespace MSAuction.Application.Handlers
{
    public class CreateAuctionHandler : IRequestHandler<CreateAuctionCommand, int>
    {
        private readonly IAuctionRepository _repository;

        public CreateAuctionHandler(IAuctionRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Auction;

            var auction = new Auction
            {
                ProductId = dto.ProductId,
                UserId = request.UserId,
                Title = dto.Title,
                Description = dto.Description,
                InitialPrice = dto.InitialPrice,
                MinIncrement = dto.MinIncrement,
                ReservePrice = dto.ReservePrice,
                StartDate = dto.StartTime.ToUniversalTime(),
                EndDate = dto.EndTime.ToUniversalTime(),
                Status = "pending",
                Conditions = dto.Conditions,
                Type = "normal"
            };

            await _repository.AddAsync(auction);
            // Publicar el evento a RabbitMQ
            var factory = new ConnectionFactory() { HostName = "localhost" };
            try
            {
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "auction_created_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var auctionCreatedEvent = new AuctionCreatedEvent
                    {
                        AuctionId = auction.Id,
                        ProductId = auction.ProductId,
                        Title = auction.Title,
                        Description = auction.Description,
                        InitialPrice = auction.InitialPrice,
                        MinIncrement = auction.MinIncrement,
                        ReservePrice = auction.ReservePrice,
                        StartDate = auction.StartDate,
                        EndDate = auction.EndDate,
                        Status = auction.Status,
                        Conditions = auction.Conditions,
                        Type = auction.Type,
                        UserId = auction.UserId
                    };

                    var json = JsonConvert.SerializeObject(auctionCreatedEvent);
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "", routingKey: "auction_created_queue", basicProperties: null, body: body);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle accordingly (retries, alerting, etc.)
                Console.WriteLine($"Error while publishing to RabbitMQ: {ex.Message}");
            }

            BackgroundJob.Schedule<AuctionBackgroundService>(
                x => x.FinalizeAuction(auction.Id),
                auction.EndDate
            );


            return auction.Id; // ID generado por la base de datos (Serial)
        }


    }
}
