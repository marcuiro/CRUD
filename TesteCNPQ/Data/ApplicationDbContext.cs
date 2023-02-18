using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TesteCNPQ.Models;

namespace TesteCNPQ.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Local> Local { get; set; }
        public DbSet<LogAuditoria> LogAuditoria { get; set; }
        public DbSet<Agendamento> Agendamento { get; set; }
    }
}