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

namespace BackupAndRestorePostgres
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TemporalStorageBD TS;
        public MainWindow()
        {
            InitializeComponent();
            this.TS = new TemporalStorageBD();
            TB_BaseDatos.Text= this.TS.getStr("bd","cinema");
            TB_BinPostgres.Text = this.TS.getStr("binPostgres", @"C:\Program Files\PostgreSQL\13\bin\");
            TB_Direccion.Text = this.TS.getStr("direccion", "");
            TB_Host.Text = this.TS.getStr("host", "localhost");
            TB_Password.Text = this.TS.getStr("password", "postgres");
            TB_Puerto.Text = this.TS.getStr("puerto", "5432");
            TB_Usuario.Text = this.TS.getStr("usuario", "postgres");
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
            FileInfo f = new FileInfo(TB_Direccion.Text);

            UtilesParaBackup.BackupDatabase(
                server: TB_Host.Text,
             port: TB_Puerto.Text,
             user: TB_Usuario.Text,
             password: TB_Password.Text,
             dbname: TB_BaseDatos.Text,
             backupdir: UtilesParaBackup.getStrCarpetaCorrecta(f.Directory.ToString()),
             backupFileName: f.Name,
             backupCommandDir: UtilesParaBackup.getStrCarpetaCorrecta(TB_BinPostgres.Text)
                );
        }

        private void alApretarBoton_Restore(object sender, RoutedEventArgs e)
        {
            FileInfo f = new FileInfo(TB_Direccion.Text);

            UtilesParaBackup.RestoreDatabase(
                server: TB_Host.Text,
             port: TB_Puerto.Text,
             user: TB_Usuario.Text,
             password: TB_Password.Text,
             dbname: TB_BaseDatos.Text,
             backupdir: UtilesParaBackup.getStrCarpetaCorrecta(f.Directory.ToString()),
             backupFileName: f.Name,
             backupCommandDir: UtilesParaBackup.getStrCarpetaCorrecta(TB_BinPostgres.Text)
                );
        }
    }
}
