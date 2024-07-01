using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    public interface IFlowerRepository:IRepository<Flower>
    {
        Task<IEnumerable<Flower>> GetExpiringFlowers(DateTime expiryDate);
        Task<IEnumerable<AppUser>> GetUsers();
    }
}
