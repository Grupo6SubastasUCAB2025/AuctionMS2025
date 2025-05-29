﻿using Microsoft.EntityFrameworkCore;
using MSAuction.Application.Interfaces;
using MSAuction.Domain.Entities;

namespace MSAuction.Infraestructure.Persistence.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly AppDbContext _context;

        public AuctionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Auction auction)
        {
            await _context.Auctions.AddAsync(auction);
            await _context.SaveChangesAsync();
            return auction.Id; // ID generado por PostgreSQL (serial)
        }

        public async Task<Auction?> GetByIdAsync(int id)
        {
            // Obtén una subasta por su ID
            return await _context.Auctions
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateAsync(Auction auction)
        {
            // Actualiza la subasta en la base de datos
            _context.Auctions.Update(auction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            // Obtén la subasta por su ID de forma asíncrona
            var auction = await GetByIdAsync(id);

            // Verifica si la subasta existe
            if (auction == null)
            {
                throw new InvalidOperationException("Subasta no encontrada.");
            }

            // Eliminar la subasta
            _context.Auctions.Remove(auction);
            await _context.SaveChangesAsync();
        }

    }
}
