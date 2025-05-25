using AgendaApp.Server.Models;
using Microsoft.EntityFrameworkCore;


namespace AgendaApp.Server.Repositories
{
    public class ContatoRepository : IContatoRepository
    {
        private readonly AgendaDBContext _context;
        public ContatoRepository(AgendaDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Contato contato)
        {
            try
            {
                await _context.Contatos.AddAsync(contato);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Adição de Contato falhou: {e} \n Em ContatoRepository");
                throw;
            }

            return;
        }

        public Task DeleteAsync(Contato contato)
        {
            try
            {
                _context.Contatos.Remove(contato);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Remoção de Contato falhou: {e} \n Em ContatoRepository");
                throw;
            }

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Contato>> GetAllAsync()
        {
            return await _context.Contatos.ToListAsync();
        }

        public async Task<Contato?> GetByIdAsync(int id)
        {
            return await _context.Contatos.FindAsync(id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public Task UpdateAsync(Contato contato)
        {
            try
            {
                _context.Contatos.Update(contato);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Update de Contato falhou: {e} \n Em ContatoRepository");
                throw;
            }

            return Task.CompletedTask;
        }
    }
}
