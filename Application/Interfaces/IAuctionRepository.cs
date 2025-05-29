﻿using MSAuction.Domain.Entities;

namespace MSAuction.Application.Interfaces
{
    public interface IAuctionRepository
    {
        Task<int> AddAsync(Auction auction); // Devuelve el ID generado
        Task<Auction?> GetByIdAsync(int id);  // Método para obtener una subasta por ID
        Task UpdateAsync(Auction auction);      // Método para actualizar una subasta
        Task DeleteAsync(int id);
    }
}
