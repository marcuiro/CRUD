using System.Data.Entity.ModelConfiguration;
using TesteCNPQ.Models;

namespace TesteCNPQ.ConfiguracaoDeEntidades
{
    public class AgendamentoEF : EntityTypeConfiguration<Agendamento>
    {
        public AgendamentoEF()
        {
            HasKey(i => i.Id);

                
        }
    }
}
