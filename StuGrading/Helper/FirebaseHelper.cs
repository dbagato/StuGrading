using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using StuGrading.Model;
//export GOOGLE_APPLICATION_CREDENTIALS = "./stugrading-firebase-adminsdk-74xdc-0930fb672b.json";


namespace StuGrading.Helper
{
    //Clase para la gestion de conexion y metodos con la bd de FireStore
    
    internal class FirebaseHelper
    {

        //Directivas de Firestore para poder generar la conexion
        private string dir = "URL DE SU ARCHIVO DE CLAVES FIREBSE";
        private string projectId;
        private FirestoreDb _firestoreDb;
        

        //Conexion a la BD al instanciar el objeto
        public FirebaseHelper() {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", dir);
            projectId = "stugrading";
            _firestoreDb = FirestoreDb.Create(projectId);
        }

        

        //Usuario
        public async Task<List<Usuario>> getUsuarios() {
            Query UsuariosQuery = _firestoreDb.Collection("Usuarios");
            QuerySnapshot UsuariosQuerySnapshot = await UsuariosQuery.GetSnapshotAsync();
            List<Usuario> listaUsuarios = new List<Usuario>();

            foreach (DocumentSnapshot documentSnapshot in UsuariosQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> Usuario = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(Usuario);
                    Usuario user = JsonConvert.DeserializeObject<Usuario>(json);
                    user.Dni = documentSnapshot.Id;
                    listaUsuarios.Add(user);
                }
            }
           return listaUsuarios;
        }

        public async Task<List<Usuario>> getUsuariosProf()
        {
            Query UsuariosProfQuery = _firestoreDb.Collection("Usuarios").WhereEqualTo("Rol","Prof");
            QuerySnapshot UsuariosProfQuerySnapshot = await UsuariosProfQuery.GetSnapshotAsync();
            List<Usuario> listaProfes = new List<Usuario>();

            foreach (DocumentSnapshot documentSnapshot in UsuariosProfQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> Usuario = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(Usuario);
                    Usuario user = JsonConvert.DeserializeObject<Usuario>(json);
                    user.Dni = documentSnapshot.Id;
                    listaProfes.Add(user);
                }
            }
            return listaProfes;
        }

        public async Task<List<Usuario>> getUsuariosAlum()
        {
            Query UsuariosProfQuery = _firestoreDb.Collection("Usuarios").WhereEqualTo("Rol", "Alum");
            QuerySnapshot UsuariosProfQuerySnapshot = await UsuariosProfQuery.GetSnapshotAsync();
            List<Usuario> listaProfes = new List<Usuario>();

            foreach (DocumentSnapshot documentSnapshot in UsuariosProfQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> Usuario = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(Usuario);
                    Usuario user = JsonConvert.DeserializeObject<Usuario>(json);
                    user.Dni = documentSnapshot.Id;
                    listaProfes.Add(user);
                }
            }
            return listaProfes;
        }
        public async Task<Usuario> buscarUsuario(String User)
        {
            Query BuscarUsuarioQuery = _firestoreDb.Collection("Usuarios").WhereEqualTo("User", User);
            QuerySnapshot UsuariosQuerySnapshot = await BuscarUsuarioQuery.GetSnapshotAsync();
            Usuario user=new Usuario();
            foreach (DocumentSnapshot documentSnapshot in UsuariosQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> Usuario = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(Usuario);
                    user = JsonConvert.DeserializeObject<Usuario>(json);
                    user.Dni = documentSnapshot.Id;
                }
            }

            return user;
        }
        public async void actualizarInsertarUsuario(Usuario usuario)
        {
            DocumentReference documentReference = _firestoreDb.Collection("Usuarios").Document(usuario.Dni);
            await documentReference.SetAsync(usuario);
        }

        public async void borrarUsuario(Usuario usuario) {
            DocumentReference documentReference = _firestoreDb.Collection("Usuarios").Document(usuario.Dni);
            await documentReference.DeleteAsync();
        }

        //Curso
        public async Task<List<Curso>> getCursos()
        {
            Query CursosQuery = _firestoreDb.Collection("Curso");
            QuerySnapshot CursosQuerySnapshot = await CursosQuery.GetSnapshotAsync();
            List<Curso> listaCursos = new List<Curso>();

            foreach (DocumentSnapshot documentSnapshot in CursosQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> Curso = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(Curso);
                    Curso curso = JsonConvert.DeserializeObject<Curso>(json);
                    curso.IdCurso = int.Parse(documentSnapshot.Id);
                    listaCursos.Add(curso);
                }
            }
            return listaCursos;
        }

        public async Task<Curso> getCurso(String nombre)
        {
            Query CursosQuery = _firestoreDb.Collection("Curso").WhereEqualTo("Nombre",nombre);
            QuerySnapshot CursosQuerySnapshot = await CursosQuery.GetSnapshotAsync();
            Curso curso = new Curso();

            foreach (DocumentSnapshot documentSnapshot in CursosQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                        Dictionary<string, object> Curso = documentSnapshot.ToDictionary();
                        string json = JsonConvert.SerializeObject(Curso);
                        curso = JsonConvert.DeserializeObject<Curso>(json);
                        curso.IdCurso = int.Parse(documentSnapshot.Id);
                    
                   
                }
            }
            return curso;
        }

        public async Task<List<Curso>> getCursosAlu(String dni)
        {
            Query CursosAluQuery = _firestoreDb.Collection("Curso").WhereArrayContains("Alumnos", dni);
            QuerySnapshot CursosAluQuerySnapshot = await CursosAluQuery.GetSnapshotAsync();
            List<Curso> listaCursosAlu = new List<Curso>();

            foreach (DocumentSnapshot documentSnapshot in CursosAluQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    
                    Dictionary<string, object> Curso = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(Curso);
                    Curso curso = JsonConvert.DeserializeObject<Curso>(json);
                    curso.IdCurso = int.Parse(documentSnapshot.Id);
                    listaCursosAlu.Add(curso);
                }
            }
            return listaCursosAlu;
        }

        public async Task<List<Curso>> getCursosProf(String dni)
        {
            Query CursosProfQuery = _firestoreDb.Collection("Curso").WhereEqualTo("DniProf", dni);
            QuerySnapshot CursosProfQuerySnapshot = await CursosProfQuery.GetSnapshotAsync();
            List<Curso> listaCursosProf = new List<Curso>();

            foreach (DocumentSnapshot documentSnapshot in CursosProfQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> Curso = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(Curso);
                    Curso curso = JsonConvert.DeserializeObject<Curso>(json);
                    curso.IdCurso = int.Parse(documentSnapshot.Id);
                    listaCursosProf.Add(curso);
                }
            }
            return listaCursosProf;
        }

        public async void actualizarInsertarCurso(Curso curso)
        {
            DocumentReference documentReference = _firestoreDb.Collection("Curso").Document(curso.IdCurso.ToString());
            await documentReference.SetAsync(curso);
        }

        public async void borrarCurso(int idCurso)
        {
            DocumentReference documentReference = _firestoreDb.Collection("Curso").Document(idCurso.ToString());
            await documentReference.DeleteAsync();
        }

        //Notas

        public async Task<List<Notas>> getNotas()
        {
            Query NotasQuery = _firestoreDb.Collection("Notas");
            QuerySnapshot NotasQuerySnapshot = await NotasQuery.GetSnapshotAsync();
            List<Notas> listaNotas = new List<Notas>();

            foreach (DocumentSnapshot documentSnapshot in NotasQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> Notas = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(Notas);
                    Notas notas = JsonConvert.DeserializeObject<Notas>(json);
                    notas.IdNotas = int.Parse(documentSnapshot.Id);
                    listaNotas.Add(notas);
                }
            }
            return listaNotas;
        }

        public async Task<List<Notas>> getNotasAlumno(String dni)
        {
            Query NotasAluQuery = _firestoreDb.Collection("Notas").WhereEqualTo("DniAlu",dni);
            QuerySnapshot NotasAluQuerySnapshot = await NotasAluQuery.GetSnapshotAsync();
            List<Notas> listaNotasAlu = new List<Notas>();

            foreach (DocumentSnapshot documentSnapshot in NotasAluQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> Notas = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(Notas);
                    Notas notas = JsonConvert.DeserializeObject<Notas>(json);
                    notas.IdNotas = int.Parse(documentSnapshot.Id);
                    listaNotasAlu.Add(notas);
                    
                }
            }
            return listaNotasAlu;
        }
        
        public async Task<Notas> getNotasAlumnoCurso(String dni,int idCurso)
        {
            Query NotasAluCursoQuery = _firestoreDb.Collection("Notas").WhereEqualTo("DniAlu", dni).WhereEqualTo("IdCurso",idCurso);
            QuerySnapshot NotasAluCursoQuerySnapshot = await NotasAluCursoQuery.GetSnapshotAsync();
            Notas notasAluCurso = new Notas();

            foreach (DocumentSnapshot documentSnapshot in NotasAluCursoQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> Notas = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(Notas);
                    notasAluCurso = JsonConvert.DeserializeObject<Notas>(json);
                    notasAluCurso.IdNotas = int.Parse(documentSnapshot.Id);

                }
            }
            return notasAluCurso;
        }

        public async void actualizarInsertarNotas(Notas notas)
        {
            DocumentReference documentReference = _firestoreDb.Collection("Notas").Document(notas.IdNotas.ToString());
            await documentReference.SetAsync(notas);
        }

        public async void borrarNotas(int idNotas)
        {
            DocumentReference documentReference = _firestoreDb.Collection("Notas").Document(idNotas.ToString());
            await documentReference.DeleteAsync();
        }
    }
}
