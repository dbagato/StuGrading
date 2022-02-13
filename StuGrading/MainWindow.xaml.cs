using StuGrading.Helper;
using StuGrading.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StuGrading
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        FirebaseHelper firebaseHelper = new FirebaseHelper();
        public MainWindow()
        {
            InitializeComponent();

        }

       

        //Login Metodos
        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            String user = userLogin.Text;
            String pwd = pwdLogin.Text;
            if ((user != null && user.Length == 9) && pwd != null)
            {
                Usuario usuario = await firebaseHelper.buscarUsuario(user);
                if (usuario.Pwd == pwd)
                {
                    switch (usuario.Rol) {
                        case "Alum":
                            login.Visibility = Visibility.Collapsed;
                            aluView.Visibility = Visibility.Visible;
                            cargarDatosAlu(usuario);
                            break;
                        case "Prof":
                            login.Visibility = Visibility.Collapsed;
                            profView.Visibility = Visibility.Visible;
                            cargarDatosProf(usuario);
                            break;
                        case "Admin":
                            login.Visibility = Visibility.Collapsed;
                            AdminView.Visibility = Visibility.Visible;
                            cargarDatosAdmin(usuario);
                            break;
                    }
                }
                else {
                    MessageBox.Show("Contraseña incorrecta");
                }
            }
            else {
                MessageBox.Show("Usuario o contraseña no válidos y no pueden estar vacios");
            }
           
        }

        private void exitLogin_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(-1);
        }

        //Alumno Metodos

        private async void cargarDatosAlu(Usuario usuario)
        { 
            int contador = 0;
            double media = 0;
            List<Curso> cursosAlu =await firebaseHelper.getCursosAlu(usuario.Dni);
            List<Notas> notasAlumno = await firebaseHelper.getNotasAlumno(usuario.Dni);
            foreach (Curso curso in cursosAlu) {
                DockPanel item = new DockPanel();
                item.Name="itemAlu"+contador;
                Label nomCurso = new Label();
                nomCurso.Name = "cursoAluName" + contador;
                nomCurso.Content = curso.Nombre;
                nomCurso.Style= (Style)Application.Current.Resources["AluFontColor"];
                nomCurso.HorizontalAlignment = HorizontalAlignment.Left;
                item.Children.Add(nomCurso);
                foreach (Notas notas in notasAlumno) {
                    if (notas.IdCurso==curso.IdCurso) {
                        Label notaCurso = new Label();
                        notaCurso.Name = "cursoAluNota" + contador;
                        notaCurso.Content = notas.Nota;
                        notaCurso.HorizontalAlignment = HorizontalAlignment.Right;
                        notaCurso.Style = (Style)Application.Current.Resources["AluFontColor"];
                        item.Children.Add(notaCurso);
                        media += notas.Nota;
                    }
                }
                notasAlu.Children.Add(item);
                contador++;
            }
            media = media / contador;
            mediaAlu.Content = mediaAlu.Content+" "+ media;

        }

        private void salirAlu_Click(object sender, RoutedEventArgs e)
        {
            login.Visibility = Visibility.Visible;
            aluView.Visibility = Visibility.Collapsed;
            notasAlu.Children.Clear();
            mediaAlu.Content = "Media: ";
            userLogin.Text=string.Empty;
            pwdLogin.Text=string.Empty;
        }

        //Profesor Metodos
        private void salirProf_Click(object sender, RoutedEventArgs e)
        {
            login.Visibility = Visibility.Visible;
            profView.Visibility = Visibility.Collapsed;
            cursosProfTable.Children.Clear();
            userLogin.Text = string.Empty;
            pwdLogin.Text = string.Empty;
        }

        //metodos para cargar los datos de Profesor
        private void cargarDatosProf(Usuario usuario)
        {
            cargarheaderAlumnosProfesor();
            cargarDatosTabViewProf1(usuario);
            cargarDatosComboBoxProfCursos(usuario,cursoNewAlu);
            cargarDatosComboBoxProfCursos(usuario, notasProf);

        }
        //ventana mis cursos
        private async void cargarDatosTabViewProf1(Usuario usuario)
        {
            List<Curso> cursosProf = await firebaseHelper.getCursosProf(usuario.Dni);
            var contadorProf1 = 0;
            
            foreach (Curso curso in cursosProf)
            {
                //stack que contendra todos los datos de cursos y alumnos
                StackPanel itemCurso = new StackPanel();
                itemCurso.Orientation = Orientation.Vertical;
                //dock para las filas de cursos y acciones
                DockPanel itemCursoContent = new DockPanel();
                //stackPanel con toda la lista de alumnos
                StackPanel cargarAlumCurso = new StackPanel();
                //dockPanel para la gestion de cada fila de alumnos con el header
                DockPanel itemAlumCurso = new DockPanel();

                //bool verAlumnos = false;

                TextBox nomCursoProf = new TextBox();
                nomCursoProf.Text = curso.Nombre;
                nomCursoProf.Name = "nomCursoProf" + contadorProf1;
                nomCursoProf.IsEnabled = false;
                nomCursoProf.Style = (Style)Application.Current.Resources["AdminTextBox2"];
                nomCursoProf.HorizontalAlignment = HorizontalAlignment.Right;
                itemCursoContent.Children.Add(nomCursoProf);
                Label numAlumMatri = new Label();
                numAlumMatri.Name = "numAlumMatri" + contadorProf1;
                if (curso.Alumnos == null)
                {
                    numAlumMatri.Content = 0;
                }
                else {
                    numAlumMatri.Content = curso.Alumnos.Count;
                }
                
                numAlumMatri.Style = (Style)Application.Current.Resources["AdminFontColor"];
                nomCursoProf.HorizontalAlignment = HorizontalAlignment.Center;
                itemCursoContent.Children.Add(numAlumMatri);
                DockPanel accionesCurso = new DockPanel();
                accionesCurso.HorizontalAlignment = HorizontalAlignment.Right;
                //generamos los botones de acciones
                Button buttonAlumAcction = new Button();
                Button buttonEditarAcction = new Button();
                Button buttonAceptarAcction = new Button();
                Button buttonCancelarAcction = new Button();
                //boton de alumnos
                buttonAlumAcction.Content = "Alumnos";
                buttonAlumAcction.Style = (Style)Application.Current.Resources["MoradoBaseButton"];
                //buttonAlumAcction.
                
                //boton de editar
                buttonEditarAcction.Content = "Editar";
                buttonEditarAcction.Style = (Style)Application.Current.Resources["MoradoBaseButton"];
                //buttonEditarAcction.Click += new RoutedEventHandler(mostrarEditar(buttonEditarAcction,buttonAceptarAcction,buttonCancelarAcction));
                
                //boton de aceptar
                buttonAceptarAcction.Content = "Aceptar";
                buttonAceptarAcction.Style = (Style)Application.Current.Resources["TurquesaBaseButton"];
                //buttonAceptarAcction.Click += new RoutedEventHandler(actualizarNomCursoProf(nomCursoProf, curso));
               
                //boton de cancelar
                buttonCancelarAcction.Content = "Cancelar";
                buttonCancelarAcction.Style = (Style)Application.Current.Resources["TurquesaBaseButton"];
                //buttonCancelarAcction.Click += new RoutedEventHandler(limpiarNomCursoProf(nomCursoProf, curso.Nombre, buttonEditarAcction, buttonAceptarAcction, buttonCancelarAcction));
                
                //los añadimos al dockPanel de los botones de accion
                accionesCurso.Children.Add(buttonAlumAcction);
                accionesCurso.Children.Add(buttonEditarAcction);
                accionesCurso.Children.Add(buttonAceptarAcction);
                accionesCurso.Children.Add(buttonCancelarAcction);
                //añadimos el dock de acciones al item
                itemCursoContent.Children.Add(accionesCurso);
                //añadimos el dock cargado del curso con las acciones al stack Panel
                itemCurso.Children.Add(itemCursoContent);

               
                //header campo1
                Label headerAlumCurso = new Label();
                headerAlumCurso.Content = "Nombre: ";
                headerAlumCurso.Style = (Style)Application.Current.Resources["AdminFontColor"];
                headerAlumCurso.HorizontalAlignment = HorizontalAlignment.Left;
                itemAlumCurso.Children.Add(headerAlumCurso);
                //header capo2
                Label headerNotaCurso = new Label();
                headerNotaCurso.Content = "Nota: ";
                headerNotaCurso.Style = (Style)Application.Current.Resources["AdminFontColor"];
                headerNotaCurso.HorizontalAlignment = HorizontalAlignment.Left;
                itemAlumCurso.Children.Add(headerNotaCurso);
                //cargamos la lista de alumnos del curso
                if (curso.Alumnos != null)
                {
                    for (int i = 0; i < curso.Alumnos.Count; i++)
                    {
                        Notas notasAlumno = await firebaseHelper.getNotasAlumnoCurso(curso.Alumnos[i].ToString(), curso.IdCurso);
                        Label notaAlumCurso = new Label();
                        notaAlumCurso.Content = notasAlumno.Nota;
                        notaAlumCurso.HorizontalAlignment = HorizontalAlignment.Right;
                        headerNotaCurso.Style = (Style)Application.Current.Resources["AdminFontColor"];
                        itemAlumCurso.Children.Add(notaAlumCurso);

                    }
                }
                //añadimos el dock cargado con los datos de los alumnos al stackPanel
                itemCurso.Children.Add(cargarAlumCurso);
                cursosProfTable.Children.Add(itemCurso);
                contadorProf1++;
            }

        }

        private void cargarheaderAlumnosProfesor()
        {
            DockPanel header = new DockPanel();
            Label headerAsignatura = new Label();
            headerAsignatura.Content = "Asignatura:";
            headerAsignatura.Style = (Style)Application.Current.Resources["AdminFontColor"];
            headerAsignatura.HorizontalAlignment = HorizontalAlignment.Left;
            header.Children.Add(headerAsignatura);
            Label headerAlumMatricula = new Label();
            headerAlumMatricula.Content = "Alumnos Matriculados:";
            headerAlumMatricula.Style = (Style)Application.Current.Resources["AdminFontColor"];
            headerAlumMatricula.HorizontalAlignment = HorizontalAlignment.Right;
            header.Children.Add(headerAlumMatricula);
            Label headerProfAcciones = new Label();
            headerProfAcciones.Content = "Acciones:";
            headerProfAcciones.Style = (Style)Application.Current.Resources["AdminFontColor"];
            headerProfAcciones.HorizontalAlignment = HorizontalAlignment.Center;
            header.Children.Add(headerProfAcciones);
            cursosProfTable.Children.Add(header);
        }

        private void mostrarAlumnos(StackPanel cargarAlumCurso, bool verAlumnos)
        {
            if (!verAlumnos)
            {
                verAlumnos = true;
                cargarAlumCurso.Visibility = Visibility.Visible;
            }
            else {
                verAlumnos = false;
                cargarAlumCurso.Visibility = Visibility.Collapsed;
            }

        }

        //metodos comentados de asignacion de Clck con los botones

       /* private RoutedEventHandler mostrarEditar(Button buttonEditarAcction, Button buttonAceptarAcction, Button buttonCancelarAcction)
        {
            buttonEditarAcction.Visibility=Visibility.Collapsed;
            buttonAceptarAcction.Visibility= Visibility.Visible;
            buttonCancelarAcction.Visibility = Visibility.Visible;
           
        }

        private RoutedEventHandler actualizarNomCursoProf(TextBox nomCursoProf, Curso curso)
        {
            curso.Nombre = nomCursoProf.Text;
            firebaseHelper.actualizarInsertarCurso(curso);

            nomCursoProf.IsEnabled = false;

        }

        private RoutedEventHandler limpiarNomCursoProf(TextBox nomCursoProf, String nombre, Button buttonEditarAcction, Button buttonAceptarAcction, Button buttonCancelarAcction)
        {
            nomCursoProf.Text = nombre;
            nomCursoProf.IsEnabled = false;
            buttonAceptarAcction.Visibility = Visibility.Collapsed;
            buttonCancelarAcction.Visibility = Visibility.Collapsed;
            buttonEditarAcction.Visibility = Visibility.Visible;

        }

        */
       

       

        //ventana Matricular Alumno

        private async void cargarDatosComboBoxProfCursos(Usuario usuario,ComboBox comboBox) {
            List<Curso> listCursosCombo = new List<Curso>();
            listCursosCombo = await firebaseHelper.getCursosProf(usuario.Dni);
            foreach (Curso curso in listCursosCombo) {
                comboBox.Items.Add(curso.Nombre);
            }
            comboBox.SelectedIndex = 0;

        }
        private async void acepNewAlu_Click(object sender, RoutedEventArgs e)
        {
            Usuario usuario = new Usuario();
            Curso curso = new Curso();
            if (dniNewAlu != null&&dniNewAlu.Text.Length==9)
            {
                if (nameNewAlu != null && nameNewAlu.Text.Length>2)
                {
                    usuario.Dni = dniNewAlu.Text;
                    usuario.Nombre = nameNewAlu.Text;
                    usuario.User = usuario.Dni;
                    usuario.Pwd = usuario.Dni;
                    usuario.Rol = "Alum";
                    firebaseHelper.actualizarInsertarUsuario(usuario);
                    curso.Nombre = cursoNewAlu.SelectedItem.ToString();
                    curso = await firebaseHelper.getCurso(curso.Nombre);
                    if (curso.Alumnos == null)
                    {
                        curso.Alumnos = new ArrayList();
                        curso.Alumnos.Add(usuario.Dni);
                    }
                    else
                    {
                        curso.Alumnos.Add(usuario.Dni);
                    }
                    firebaseHelper.actualizarInsertarCurso(curso);
                    MessageBox.Show("Usuario insertado correctamente");
                    dniNewAlu.Text = String.Empty;
                    nameNewAlu.Text = String.Empty;
                }
                else {
                    MessageBox.Show("Inserte un nombre valido");
                }
            }
            else {
                MessageBox.Show("Inserte un dni Valido");
            }
            

        }

        private void borrNewAlu_Click(object sender, RoutedEventArgs e)
        {
            dniNewAlu.Text=string.Empty;
            nameNewAlu.Text = string.Empty;
            cursoNewAlu.SelectedIndex = 0;
        }

        private async void buscarNotasProf_Click(object sender, RoutedEventArgs e)
        {
            notasAluProf.Children.Clear();
            Curso cursoNotas = new Curso();
            cursoNotas = await firebaseHelper.getCurso(notasProf.SelectedValue.ToString());
            if (cursoNotas.Alumnos != null)
            {
                for (int i = 0; i < cursoNotas.Alumnos.Count; i++) {
                Usuario alumno = new Usuario();
                alumno=await firebaseHelper.buscarUsuario(cursoNotas.Alumnos[i].ToString());
                    //dock para las filas de cursos y acciones
                    DockPanel itemAlumNota = new DockPanel();

                    Label nomAlumNota = new Label();
                    nomAlumNota.Content = alumno.Nombre;
                    nomAlumNota.Name = "nomAluNota" + i;
                    nomAlumNota.Style = (Style)Application.Current.Resources["AdminFontColor"];
                    nomAlumNota.HorizontalAlignment = HorizontalAlignment.Left;
                    itemAlumNota.Children.Add(nomAlumNota);

                    Notas notaAluCurso = new Notas();
                    notaAluCurso = await firebaseHelper.getNotasAlumnoCurso(alumno.Dni,cursoNotas.IdCurso);
                    TextBox notaTextBox = new TextBox();
                    notaTextBox.Name = "notaTextBox" + i;
                    notaTextBox.Style = (Style)Application.Current.Resources["AdminTextBox2"];
                    notaTextBox.HorizontalAlignment = HorizontalAlignment.Center;
                    if (notaAluCurso != null)
                    {
                        notaTextBox.Text=notaAluCurso.Nota.ToString();
                    }
                    else {
                        notaTextBox.Text = "-";
                    }
                    itemAlumNota.Children.Add(notaTextBox);

                    Button buttonAceptarAcction = new Button();
                    //boton de aceptar
                    buttonAceptarAcction.Content = "Aceptar";
                    buttonAceptarAcction.Style = (Style)Application.Current.Resources["MoradoBaseButton"];
                    buttonAceptarAcction.HorizontalAlignment = HorizontalAlignment.Left;
                    //buttonAceptarAcction.Click += new RoutedEventHandler(actualizarNomCursoProf(nomCursoProf, curso));
                    itemAlumNota.Children.Add(buttonAceptarAcction);

                    notasAluProf.Children.Add(itemAlumNota);
                }
            }
            else {
                Label noAlum = new Label();
                noAlum.Content = "No hay alumnos matriculados para este curso";
                noAlum.Style = (Style)Application.Current.Resources["AdminFontColor"];
                notasAluProf.Children.Add(noAlum);
            }
        }

        //Admin Metodos

        //metodos para cargar los datos de Admin
        private void cargarDatosAdmin(Usuario usuario)
        {
            cargarDatosProfComboBoxAdminCursos();
            cargarDatosCursoComboBoxAdminCursos();
            cargarDatosTabViewAdminCursos(usuario);
            cargarDatosTabViewAdminProf(usuario);
            cargarDatosTabViewAdminAlum(usuario);
        }

        private async void cargarDatosProfComboBoxAdminCursos()
        {
            dniProfCur.Items.Clear();
            dniProfNewCur.Items.Clear();
            List<Usuario> listprofCombo = new List<Usuario>();
            var contador = 0;
            listprofCombo = await firebaseHelper.getUsuariosProf();
            foreach (Usuario profe in listprofCombo)
            {
                dniProfCur.Items.Insert(contador, profe.Dni);
                dniProfNewCur.Items.Insert(contador, profe.Dni);
                contador++;
            }
            dniProfCur.SelectedIndex = 0;
            dniProfNewCur.SelectedIndex = 0;
        }
        private async void cargarDatosCursoComboBoxAdminCursos()
        {
            curProfCur.Items.Clear();
            List<Curso> listCursoCombo = new List<Curso>();
            var contador = 0;
            listCursoCombo = await firebaseHelper.getCursos();
            foreach (Curso curso in listCursoCombo)
            {
                curProfCur.Items.Insert(contador, curso.Nombre);
                contador++;
            }
            curProfCur.SelectedIndex = 0;
        }

        private void salirAdmin_Click(object sender, RoutedEventArgs e)
        {
            login.Visibility = Visibility.Visible;
            AdminView.Visibility = Visibility.Collapsed;
            userLogin.Text = string.Empty;
            pwdLogin.Text = string.Empty;
        }
        //pantalla añadir profesor
        private void acepNewProf_Click(object sender, RoutedEventArgs e)
        {
            if (dniNewProf != null && dniNewProf.Text.Length == 9)
            {
                if (nameNewProf != null && nameNewProf.Text.Length > 2)
                {
                    Usuario newProfe = new Usuario();
                    newProfe.Dni = dniNewProf.Text;
                    newProfe.Nombre = nameNewProf.Text;
                    newProfe.User = newProfe.Dni;
                    newProfe.Pwd = newProfe.Dni;
                    newProfe.Rol = "Prof";
                    firebaseHelper.actualizarInsertarUsuario(newProfe);
                    MessageBox.Show("Usuario insertado correctamente");
                    dniNewProf.Text = String.Empty;
                    nameNewProf.Text = String.Empty;
                    cargarDatosProfComboBoxAdminCursos();
                }
                else
                {
                    MessageBox.Show("Inserte un nombre valido");
                }
            }
            else
            {
                MessageBox.Show("Inserte un dni Valido");
            }
        }

        private void borrNewProf_Click(object sender, RoutedEventArgs e)
        {
            dniNewProf.Text = String.Empty;
            nameNewProf.Text = String.Empty;
        }
        //pantalla asignar Curso
        private void acepAsiCur_Click(object sender, RoutedEventArgs e)
        {
            Curso curso = new Curso();
            curso.DniProf = dniProfCur.SelectedItem.ToString();
            curso.Nombre = curProfCur.SelectedItem.ToString();
            curso.IdCurso = curProfCur.SelectedIndex;
            firebaseHelper.actualizarInsertarCurso(curso);
            MessageBox.Show("Curso asignado correctamente");
        }

        private void borrAsiCur_Click(object sender, RoutedEventArgs e)
        {
            dniProfCur.SelectedIndex = 0;
            curProfCur.SelectedIndex = 0;
        }
        
        private async void acepNewCur_Click(object sender, RoutedEventArgs e)
        {
            if (nomNewCur!=null&&nomNewCur.Text.Length>3) {
                List<Curso> listCursoCombo = new List<Curso>();
                listCursoCombo = await firebaseHelper.getCursos();
                Curso curso = new Curso();
                curso.DniProf = dniProfNewCur.SelectedItem.ToString();
                curso.Nombre = nomNewCur.Text;
                curso.IdCurso = listCursoCombo.Count + 1;
                firebaseHelper.actualizarInsertarCurso(curso);
                MessageBox.Show("Curso creado correctamente");
                nomNewCur.Text = String.Empty;
            }
            else {
                MessageBox.Show("Inserta un nombre valido");
            }
            
        }

        private void borrNewCur_Click(object sender, RoutedEventArgs e)
        {
            nomNewCur.Text=String.Empty;
            dniProfNewCur.SelectedIndex = 0;
        }

        //pantalla Cursos

        private async void cargarDatosTabViewAdminCursos(Usuario usuario)
        {
            List<Curso> cursos = await firebaseHelper.getCursos();
            var contadorAdmin = 0;

            foreach (Curso curso in cursos)
            {
                //stack que contendra todos los datos de cursos
                StackPanel itemCurso = new StackPanel();
                itemCurso.Orientation = Orientation.Vertical;
                //dock para las filas de cursos y acciones
                DockPanel itemCursoContent = new DockPanel();

                TextBox nomCursoAdmin = new TextBox();
                nomCursoAdmin.Text = curso.Nombre;
                nomCursoAdmin.Name = "nomCursoAdmin" + contadorAdmin;
                nomCursoAdmin.IsEnabled = false;
                nomCursoAdmin.Style = (Style)Application.Current.Resources["AdminTextBox2"];
                nomCursoAdmin.HorizontalAlignment = HorizontalAlignment.Left;
                itemCursoContent.Children.Add(nomCursoAdmin);
                Label numAlumMatri = new Label();
                numAlumMatri.Name = "numAlumMatriAdmin" + contadorAdmin;
                if (curso.Alumnos == null)
                {
                    numAlumMatri.Content = 0;
                }
                else
                {
                    numAlumMatri.Content = curso.Alumnos.Count;
                }
                numAlumMatri.Style = (Style)Application.Current.Resources["AdminFontColor"];
                itemCursoContent.Children.Add(numAlumMatri);
                DockPanel accionesCurso = new DockPanel();
                accionesCurso.HorizontalAlignment = HorizontalAlignment.Right;
                //generamos los botones de acciones
                Button buttonBorrarAcction = new Button();
                Button buttonEditarAcction = new Button();
                Button buttonAceptarAcction = new Button();
                Button buttonCancelarAcction = new Button();
                //boton de alumnos
                buttonBorrarAcction.Content = "Borrar";
                buttonBorrarAcction.Style = (Style)Application.Current.Resources["MoradoBaseButton"];
                //buttonBorrarAcction.Click += new RoutedEvent(borrarCurso());
                
                //boton de editar
                buttonEditarAcction.Content = "Editar";
                buttonEditarAcction.Style = (Style)Application.Current.Resources["MoradoBaseButton"];
                //buttonEditarAcction.Click += new RoutedEventHandler(mostrarEditar(buttonEditarAcction,buttonAceptarAcction,buttonCancelarAcction));
               
                //boton de aceptar
                buttonAceptarAcction.Content = "Aceptar";
                buttonAceptarAcction.Style = (Style)Application.Current.Resources["TurquesaBaseButton"];
                //buttonAceptarAcction.Click += new RoutedEventHandler(actualizarNomCursoProf(nomCursoProf, curso));
                
                //boton de cancelar
                buttonCancelarAcction.Content = "Cancelar";
                buttonCancelarAcction.Style = (Style)Application.Current.Resources["TurquesaBaseButton"];
                //buttonCancelarAcction.Click += new RoutedEventHandler(limpiarNomCursoProf(nomCursoProf, curso.Nombre, buttonEditarAcction, buttonAceptarAcction, buttonCancelarAcction));
                
                //los añadimos al dockPanel de los botones de accion
                accionesCurso.Children.Add(buttonBorrarAcction);
                accionesCurso.Children.Add(buttonEditarAcction);
                accionesCurso.Children.Add(buttonAceptarAcction);
                accionesCurso.Children.Add(buttonCancelarAcction);
                //añadimos el dock de acciones al item
                itemCursoContent.Children.Add(accionesCurso);
                //añadimos el dock cargado del curso con las acciones al stack Panel
                itemCurso.Children.Add(itemCursoContent);

                adminListaCursos.Children.Add(itemCurso);
                contadorAdmin++;
            }

        }

        //pantalla Profesores
        private async void cargarDatosTabViewAdminProf(Usuario usuario)
        {
            List<Usuario> profes = await firebaseHelper.getUsuariosProf();
            var contadorAdmin = 0;

            foreach (Usuario profe in profes)
            {
                //dock para las filas de cursos y acciones
                DockPanel itemProfeContent = new DockPanel();

                TextBox nomProfeAdmin = new TextBox();
                nomProfeAdmin.Text = profe.Nombre;
                nomProfeAdmin.Name = "nomProfAdmin" + contadorAdmin;
                nomProfeAdmin.IsEnabled = false;
                nomProfeAdmin.Style = (Style)Application.Current.Resources["AdminTextBox2"];
                nomProfeAdmin.HorizontalAlignment = HorizontalAlignment.Left;
                itemProfeContent.Children.Add(nomProfeAdmin);
               
                DockPanel accionesCurso = new DockPanel();
                accionesCurso.HorizontalAlignment = HorizontalAlignment.Right;
                //generamos los botones de acciones
                Button buttonBorrarAcction = new Button();
                Button buttonEditarAcction = new Button();
                Button buttonAceptarAcction = new Button();
                Button buttonCancelarAcction = new Button();
                //boton de alumnos
                buttonBorrarAcction.Content = "Borrar";
                buttonBorrarAcction.Style = (Style)Application.Current.Resources["MoradoBaseButton"];
                //buttonBorrarAcction.Click += new RoutedEvent(borrarCurso());
                
                //boton de editar
                buttonEditarAcction.Content = "Editar";
                buttonEditarAcction.Style = (Style)Application.Current.Resources["MoradoBaseButton"];
                //buttonEditarAcction.Click += new RoutedEventHandler(mostrarEditar(buttonEditarAcction,buttonAceptarAcction,buttonCancelarAcction));
                
                //boton de aceptar
                buttonAceptarAcction.Content = "Aceptar";
                buttonAceptarAcction.Style = (Style)Application.Current.Resources["TurquesaBaseButton"];
                //buttonAceptarAcction.Click += new RoutedEventHandler(actualizarNomCursoProf(nomCursoProf, curso));
                
                //boton de cancelar
                buttonCancelarAcction.Content = "Cancelar";
                buttonCancelarAcction.Style = (Style)Application.Current.Resources["TurquesaBaseButton"];
                //buttonCancelarAcction.Click += new RoutedEventHandler(limpiarNomCursoProf(nomCursoProf, curso.Nombre, buttonEditarAcction, buttonAceptarAcction, buttonCancelarAcction));
                
                //los añadimos al dockPanel de los botones de accion
                accionesCurso.Children.Add(buttonBorrarAcction);
                accionesCurso.Children.Add(buttonEditarAcction);
                accionesCurso.Children.Add(buttonAceptarAcction);
                accionesCurso.Children.Add(buttonCancelarAcction);
                //añadimos el dock de acciones al item
                itemProfeContent.Children.Add(accionesCurso);
                

                adminListaProf.Children.Add(itemProfeContent);
                contadorAdmin++;
            }

        }

        //pantalla Alumnos

        private async void cargarDatosTabViewAdminAlum(Usuario usuario)
        {
            List<Usuario> alumnos = await firebaseHelper.getUsuariosAlum();
            var contadorAdmin = 0;

            foreach (Usuario alum in alumnos)
            {
                //dock para las filas de cursos y acciones
                DockPanel itemProfeContent = new DockPanel();

                TextBox nomAlumAdmin = new TextBox();
                nomAlumAdmin.Text = alum.Nombre;
                nomAlumAdmin.Name = "nomProfAdmin" + contadorAdmin;
                nomAlumAdmin.IsEnabled = false;
                nomAlumAdmin.Style = (Style)Application.Current.Resources["AdminTextBox2"];
                nomAlumAdmin.HorizontalAlignment = HorizontalAlignment.Left;
                itemProfeContent.Children.Add(nomAlumAdmin);

                DockPanel accionesCurso = new DockPanel();
                accionesCurso.HorizontalAlignment = HorizontalAlignment.Right;
                //generamos los botones de acciones
                Button buttonBorrarAcction = new Button();
                Button buttonEditarAcction = new Button();
                Button buttonAceptarAcction = new Button();
                Button buttonCancelarAcction = new Button();
                //boton de alumnos
                buttonBorrarAcction.Content = "Borrar";
                buttonBorrarAcction.Style = (Style)Application.Current.Resources["MoradoBaseButton"];
                //buttonBorrarAcction.Click += new RoutedEvent(borrarCurso());

                //boton de editar
                buttonEditarAcction.Content = "Editar";
                buttonEditarAcction.Style = (Style)Application.Current.Resources["MoradoBaseButton"];
                //buttonEditarAcction.Click += new RoutedEventHandler(mostrarEditar(buttonEditarAcction,buttonAceptarAcction,buttonCancelarAcction));

                //boton de aceptar
                buttonAceptarAcction.Content = "Aceptar";
                buttonAceptarAcction.Style = (Style)Application.Current.Resources["TurquesaBaseButton"];
                //buttonAceptarAcction.Click += new RoutedEventHandler(actualizarNomCursoProf(nomCursoProf, curso));

                //boton de cancelar
                buttonCancelarAcction.Content = "Cancelar";
                buttonCancelarAcction.Style = (Style)Application.Current.Resources["TurquesaBaseButton"];
                //buttonCancelarAcction.Click += new RoutedEventHandler(limpiarNomCursoProf(nomCursoProf, curso.Nombre, buttonEditarAcction, buttonAceptarAcction, buttonCancelarAcction));

                //los añadimos al dockPanel de los botones de accion
                accionesCurso.Children.Add(buttonBorrarAcction);
                accionesCurso.Children.Add(buttonEditarAcction);
                accionesCurso.Children.Add(buttonAceptarAcction);
                accionesCurso.Children.Add(buttonCancelarAcction);
                //añadimos el dock de acciones al item
                itemProfeContent.Children.Add(accionesCurso);


                adminListaAlu.Children.Add(itemProfeContent);
                contadorAdmin++;
            }

        }

    }
}
