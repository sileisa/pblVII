using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PBLVII.Models
{
    public class Tarefa
    {
        public int TarefaId { get; set; }
        public String Nome { get; set; }
        public String Status { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        [ForeignKey("Programador")]
        public int ProgramadorId { get; set; }
        public Programador Programador { get; set; }
    }
}