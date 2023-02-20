using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteCNPQ.Models
{
    [Table("Local")]
    public class Local
    {
        [Column("Id")]
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        [Column("Nome")]
        [Display(Name = "Nome")]
        public string? Nome { get; set; }

        [Column("Endereço")]
        [Display(Name = "Endereço")]
        public string? Endereço { get; set; }

        [Column("CapacidadeMax")]
        [Display(Name = "CapacidadeMax")]
        public int CapacidadeMax { get; set; }
    }
}
