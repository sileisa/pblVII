using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBLVII.Models
{
   public class Programador
   {
      public int ProgramadorId { get; set; }
      public String Nome { get; set; }
      public String Nivel { get; set; }
      public ICollection<Tarefa> Tarefa { get; set; }

   }
}