using Microsoft.EntityFrameworkCore;

namespace AgendaApp.Server.Models
{
    public class AgendaDBContext : DbContext
    {
        public AgendaDBContext(DbContextOptions<AgendaDBContext> options) : base(options) { }

        public DbSet<Contato> Contatos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Contato>().Property(c => c.Nome).IsRequired().HasMaxLength(150);
            modelBuilder.Entity<Contato>().Property(c => c.Email).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Contato>().Property(c => c.Telefone).IsRequired().HasMaxLength(100);
        }
    }
}
