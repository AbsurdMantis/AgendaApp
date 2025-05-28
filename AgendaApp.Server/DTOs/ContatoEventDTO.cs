using AgendaApp.Server.Models;

namespace AgendaApp.Server.DTOs
{
    public class ContatoEventDTO
    {
        public required int Id { get; init; }
        public required string Nome { get; init; }
        public required string Email { get; init; }
        public required string Telefone { get; init; }
        public required ContatoEventType Evento { get; init; }
        public required DateTime Timestamp { get; init; }
    }
}
