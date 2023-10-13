using server.Data;
using server.Models;

namespace server.Repositories
{
    public class TownNumberRepository : Repository<TownNumber>, ITownNumberRepository
    {
        private readonly AppDbContext _context;

        public TownNumberRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<TownNumber> Update(TownNumber townNumber)
        {
            townNumber.UpdateDate = DateTime.Now;

            _context.TownsNumber.Update(townNumber);

            await _context.SaveChangesAsync();

            return townNumber;
        }
    }
}