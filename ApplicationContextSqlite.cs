using Microsoft.EntityFrameworkCore;

namespace Note
{
    public class ApplicationContextSqlite: DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Note> Notes { get; set; } = null!;
        public DbSet<Backup> Backup { get; set; } = null!;

        public ApplicationContextSqlite()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=note.db");
        }
    }
}
