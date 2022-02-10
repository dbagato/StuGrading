using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuGrading.Model
{
    internal class Usuario
    {
        public String Dni { get; set;}
        public String Nombre { get; set; }
        public String Apellidos { get; set; }
        public String User { get; set; }
        public String Pwd { get; set; }
        public int Rol { get; set; }
    }
}
