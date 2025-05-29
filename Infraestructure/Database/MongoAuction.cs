using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MSAuction.Infraestructure.Database
{
    public class MongoAuction
    {
        [BsonId]  // Marca esta propiedad como el _id de MongoDB
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }  // Necesario para que Mongo no lance error con el campo _id


        public int AuctionId { get; set; }
        public int ProductId { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public decimal InitialPrice { get; set; }
        public decimal MinIncrement { get; set; }
        public decimal? ReservePrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public required string Status { get; set; }
        public string? Conditions { get; set; }
        public required string Type { get; set; }
        public required int UserId { get; set; }

    }
}
