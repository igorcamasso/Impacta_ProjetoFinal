using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjetoFinal.Models;

namespace ProjetoFinal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ProjetoFinal.Models.Produto> Produto { get; set; }
        public DbSet<ProjetoFinal.Models.Venda> Venda { get; set; }
        public DbSet<ProjetoFinal.Models.Estoque> Estoque { get; set; }
    }
}