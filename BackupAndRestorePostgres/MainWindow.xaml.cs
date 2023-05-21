using System;
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

using Delimon.Win32.IO;
using BackupAndRestorePostgres.UtilesBackup;
using ReneUtiles.Clases.BD;
using Microsoft.Win32;
using ReneUtiles;
using ReneUtiles.Clases.WPF;

namespace BackupAndRestorePostgres
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TemporalStorageBD TS;
        public EjecutorDeSubprosesosWPF ejc;
        public MainWindow()
        {//and Roilan Lauzardo Sotolongo
            InitializeComponent();
            this.TS = new TemporalStorageBD();
            TB_BaseDatos.Text= this.TS.getStr("bd","cinema");
            TB_BinPostgres.Text = this.TS.getStr("binPostgres", @"C:\Program Files\PostgreSQL\13\bin\");
            TB_Direccion.Text = this.TS.getStr("direccion", "");
            TB_Host.Text = this.TS.getStr("host", "localhost");
            TB_Password.Text = this.TS.getStr("password", "postgres");
            TB_Puerto.Text = this.TS.getStr("puerto", "5432");
            TB_Usuario.Text = this.TS.getStr("usuario", "postgres");

            this.ejc = new EjecutorDeSubprosesosWPF();
        }

        public void guardarEA() {
            this.TS.put("bd", TB_BaseDatos.Text);
            this.TS.put("binPostgres", TB_BinPostgres.Text);
            this.TS.put("direccion", TB_Direccion.Text);
            this.TS.put("host", TB_Host.Text);
            this.TS.put("password", TB_Password.Text);
            this.TS.put("puerto", TB_Puerto.Text);
            this.TS.put("usuario", TB_Usuario.Text);
        }
        private void AlCerrarLaVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            guardarEA();

        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }


        private void btnMinimizeLogin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnCloseLogin_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void alApretarBoton_Backup(object sender, RoutedEventArgs e)
        {


            var mens = System.Windows.MessageBox.Show("Está seguro que desea realizar una copia de la base de datos?", "Confirmación", MessageBoxButton.OKCancel);
            if (mens == MessageBoxResult.OK)
            {
                if (Validar(true))
                {
                    string server = TB_Host.Text;
                    string port = TB_Puerto.Text;
                    string user = TB_Usuario.Text;
                    string password = TB_Password.Text;
                    string dbname = TB_BaseDatos.Text;
                    string direccion = TB_Direccion.Text;

                    string backupCommandDir = UtilesParaBackup.getStrCarpetaCorrecta(TB_BinPostgres.Text);

                    string res = "";
                    DlgEspere dlg = new DlgEspere();
                    this.ejc.ejecutar(new EventosEnSubprocesoWPF(
                        antesDeComenzar: () => {
                            UtilesSubprocesos.subp(()=> {
                                UtilesWPF.subpVisual(() => {
                                    dlg.ShowDialog();
                                });
                            });
                            
                        }
                        , alTerminar: () => {
                            TB_Direccion.Text = res;
                            dlg.Hide();
                            System.Windows.MessageBox.Show("Proceso terminado");
                        }
                        , siDaError: (ex) => {
                            dlg.Hide();
                            System.Windows.MessageBox.Show("Error en el proceso:\n"+ex.ToString());
                        }
                        , alConcluirSiempre: () => {
                            //System.Windows.MessageBox.Show("Proceso terminado");
                        }
                        )
                        ,()=> {
                             res = UtilesParaBackup.BackupDatabase(
                                               server: server,
                                            port: port,
                                            user: user,
                                            password: password,
                                            dbname: dbname,
                                            direccion: direccion,
                                           
                                            backupCommandDir: backupCommandDir
                               );
                        });

                    
                    //FileInfo f = new FileInfo(TB_Direccion.Text);



                }
            }
            
        }

        private void alApretarBoton_Restore(object sender, RoutedEventArgs e)
        {
            var mens = System.Windows.MessageBox.Show("Está seguro que desea restaurar la base de datos?", "Confirmación", MessageBoxButton.OKCancel);
            if (mens == MessageBoxResult.OK)
            {
                if (Validar(false))
                {
                    string server = TB_Host.Text;
                    string port = TB_Puerto.Text;
                    string user = TB_Usuario.Text;
                    string password = TB_Password.Text;
                    string dbname = TB_BaseDatos.Text;
                    string direccion = TB_Direccion.Text;

                    string backupCommandDir = UtilesParaBackup.getStrCarpetaCorrecta(TB_BinPostgres.Text);

                    
                    DlgEspere dlg = new DlgEspere();


                    this.ejc.ejecutar(new EventosEnSubprocesoWPF(
                        antesDeComenzar: () => {
                            UtilesSubprocesos.subp(() => {
                                UtilesWPF.subpVisual(() => {
                                    dlg.ShowDialog();
                                });
                            });

                        }
                        , alTerminar: () => {
                            
                            dlg.Hide();
                            System.Windows.MessageBox.Show("Proceso terminado");
                        }
                        , siDaError: (ex) => {
                            dlg.Hide();
                            System.Windows.MessageBox.Show("Error en el proceso:\n" + ex.ToString());
                        }
                        , alConcluirSiempre: () => {
                            //System.Windows.MessageBox.Show("Proceso terminado");
                        }
                        )
                        , () => {
                             UtilesParaBackup.RestoreDatabase(
                                              server: server,
                                           port: port,
                                           user: user,
                                           password: password,
                                           dbname: dbname,
                                           direccion: direccion,

                                           backupCommandDir: backupCommandDir
                              );
                        });

                    //FileInfo f = new FileInfo(TB_Direccion.Text);
                    //UtilesParaBackup.RestoreDatabase(
                    //                server: TB_Host.Text,
                    //             port: TB_Puerto.Text,
                    //             user: TB_Usuario.Text,
                    //             password: TB_Password.Text,
                    //             dbname: TB_BaseDatos.Text,
                    //             direccion: TB_Direccion.Text,
                    //             //backupdir: UtilesParaBackup.getStrCarpetaCorrecta(f.Directory.ToString()),
                    //             //backupFileName: f.Name,
                    //             backupCommandDir: UtilesParaBackup.getStrCarpetaCorrecta(TB_BinPostgres.Text)
                    //);
                }
            }
                
            
        }

        private bool Validar(bool realizarBakup)
        {
            DirectoryInfo dir = new DirectoryInfo(TB_BinPostgres.Text);
            FileInfo fi = new FileInfo(TB_Direccion.Text);

            bool es_valido = dir.Exists ;
            if (!es_valido)
            {
                MessageBox.Show("La dirección del directorio bin de postgres es invalida", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            es_valido = realizarBakup?fi.Directory!=null&&fi.Directory.Exists:fi.Exists;
            if (!es_valido)
            {
                MessageBox.Show("La dirección del backup es invalida", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            es_valido = TB_BaseDatos.Text.Trim().Length > 0;
            if (!es_valido)
            {
                MessageBox.Show("La base de datos no puede estar vacia", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;

        }

        private void alApretar_B_BinPosgres(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            string dirAnterior = this.TS.getStr("ultimoDir", "");
            if (dirAnterior.Length > 0)
            {
                dialog.SelectedPath = dirAnterior;
            }
            bool? success = dialog.ShowDialog();
            if (success == true)
            {

                string dir = dialog.SelectedPath;
                this.TS.put("ultimoDir", dir);

                TB_BinPostgres.Text = dir;

            }
        }

        private void alApretar_B_ArchivoDeBackup(object sender, RoutedEventArgs e)
        {
            string dir = "";

            SaveFileDialog openfile = new SaveFileDialog();

            //openfile.Filter = "SQLite (*.acconf)|*.*";

            string dirAnterior = this.TS.getStr("ultimoDir_TXT", "");
            if (dirAnterior.Length > 0)
            {
                openfile.InitialDirectory = dirAnterior;
            }
            var result = openfile.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                //direccionFile_TXT.Text = openfile.FileName;


                dir = openfile.FileName;
                //dir = UtilesParaBackup.adaptarExtencioBackup(dir);
                
                this.TS.put("ultimoDir_TXT", dir);

                TB_Direccion.Text = dir;
                    
               





            }
        }
    }
}
