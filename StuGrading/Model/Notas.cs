using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace StuGrading.Model
{
    [FirestoreData]
    internal class Notas
    {
        public int IdNotas { get; set; }
        [FirestoreProperty]
        public String DniProfe { get; set; }
        [FirestoreProperty]
        public String DniAlu { get; set; }
        [FirestoreProperty]
        public int IdCurso { get; set; }
        [FirestoreProperty]
        public Double Nota { get; set; }
    }
}
