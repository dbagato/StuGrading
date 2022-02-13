using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace StuGrading.Model
{
    [FirestoreData]
    internal class Usuario
    {
       
        public String Dni { get; set;}
        [FirestoreProperty]
        public String Nombre { get; set; }
        [FirestoreProperty]
        public String Apellidos { get; set; }
        [FirestoreProperty]
        public String User { get; set; }
        [FirestoreProperty]
        public String Pwd { get; set; }
        [FirestoreProperty]
        public String Rol { get; set; }
    }
}
