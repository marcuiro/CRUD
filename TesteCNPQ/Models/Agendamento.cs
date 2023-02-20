using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TesteCNPQ.Models
{
    [Table("Agendamento")]
    public class Agendamento
    {
        [Column("Id")]
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        [Column("IdLocal")]
        [Display(Name = "IdLocal")]
        public Guid IdLocal { get; set; }

        [Column("NomeResponsavel")]
        [Display(Name = "NomeResponsavel")]
        public string? NomeResponsavel { get; set; }

        [Column("DataInicio")]
        [Display(Name = "DataInicio")]
        public DateTime DataInicio { get; set; }

        [Column("DataTermino")]
        [Display(Name = "DataTermino")]
        public DateTime DataTermino { get; set; }

        public virtual Local Local { get; set; }

    }

   
}
