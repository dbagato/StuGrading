using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuGrading.Model
{
    internal class Notas
    { 
        public int IdNotas { get; set; }
        public String DniProfe { get; set; }
        public String DniAlu { get; set; }
        public String IdCurso { get; set; }
        public Double Nota { get; set; }
    }
}
