using Core.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Implementations
{
    public class FlowerRepository:Repository<Flower>, IFlowerRepository
    {
        private readonly AppDbContext _context;

        public FlowerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Flower>> GetExpiringFlowers(DateTime expiryDate)
        {
            return await _context.Flowers
                .Where(f => f.ExpiryDate <= expiryDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<AppUser>> GetUsers()
        {
            return await _context.AppUsers.ToListAsync();
        }
    }
}
