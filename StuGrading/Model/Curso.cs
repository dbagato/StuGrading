using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace StuGrading.Model
{
    [FirestoreData]
    internal class Curso
    {
        
        public int IdCurso { get; set;}
        [FirestoreProperty]
        public String Nombre { get; set; }
        [FirestoreProperty]
        public String DniProf { get; set; }
        [FirestoreProperty]
        public ArrayList Alumnos { get; set; }
    }
}
