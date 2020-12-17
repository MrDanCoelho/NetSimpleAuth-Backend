using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using NetPOC.Backend.Domain.Interfaces;
using NetPOC.Backend.Domain.Models;

namespace NetPOC.Backend.Infra
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) 
            : base(options) {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        
        // Entities
        public DbSet<UsuarioModel> Usuario { get; set; }
    }
}