using MSAuction.Infraestructure.Database;
//using System.Threading.Tasks;

namespace MSAuction.Application.Interfaces
{
    public interface IMongoAuctionRepository
    {
        Task AddAsync(MongoAuction auction);           // Crear subasta en Mongo
        Task UpdateAsync(MongoAuction auction);         // Actualizar subasta en Mongo
        Task DeleteAsync(int auctionId);                // Eliminar subasta de Mongo
        Task<MongoAuction> GetByIdAsync(int auctionId); // Obtener subasta por ID en Mongo
    }
}
