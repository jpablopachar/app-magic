using server.Data;
using server.Models;

namespace server.Repositories
{
    public class TownRepository : Repository<Town>, ITownRepository
    {
        private readonly AppDbContext _context;

        public TownRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Town> Update(Town town)
        {
            town.UpdateDate = DateTime.Now;

            _context.Towns.Update(town);

            await _context.SaveChangesAsync();

            return town;
        }
    }
}