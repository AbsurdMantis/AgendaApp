using AgendaApp.Server.Models;

namespace AgendaApp.Server.Repositories
{

    public interface IContatoRepository
    {
        Task<IEnumerable<Contato>> GetAllAsync();
        Task<Contato?> GetByIdAsync(int id);

        Task AddAsync(Contato contato);

        Task UpdateAsync(Contato contato);

        Task DeleteAsync(Contato contato);

        Task<bool> SaveChangesAsync();

    }

}
