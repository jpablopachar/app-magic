using server.Models;

namespace server.Repositories
{
    public interface ITownNumberRepository : IRepository<TownNumber>
    {
        Task<TownNumber> Update(TownNumber townNumber);
    }
}