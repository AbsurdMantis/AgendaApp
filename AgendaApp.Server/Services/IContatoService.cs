using AgendaApp.Server.DTOs;

namespace AgendaApp.Server.Services
{
    public interface IContatoService
    {
        Task<IEnumerable<ContatoDTO>> GetContatosAsync();

        Task<ContatoDTO?> GetContatoAsync(int id);

        Task<ContatoDTO> AddContatoAsync(ContatoDTO contatoDTO);

        Task<ContatoDTO?> UpdateContatoAsync(int id, ContatoDTO contatoDTO);

        Task<bool> RemoveContatoAsync(int id);
    }
}
