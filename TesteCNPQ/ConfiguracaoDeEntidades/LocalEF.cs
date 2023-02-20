using System.Data.Entity.ModelConfiguration;
using TesteCNPQ.Models;

namespace TesteCNPQ.ConfiguracaoDeEntidades
{
    public class LocalEF : EntityTypeConfiguration<Local>
    {
        public LocalEF()
        {
            HasKey(i => i.Id);
        }
    }
}
