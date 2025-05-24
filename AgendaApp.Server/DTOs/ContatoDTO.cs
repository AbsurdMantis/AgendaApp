namespace AgendaApp.Server.DTOs
{
    public class ContatoDTO
    {
        public required int Id { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }

        public required string Telefone { get; set; }
    }
}
