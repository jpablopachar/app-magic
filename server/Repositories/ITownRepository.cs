using server.Models;

namespace server.Repositories
{
    public interface ITownRepository : IRepository<Town>
    {
        Task<Town> Update(Town town);
    }
}