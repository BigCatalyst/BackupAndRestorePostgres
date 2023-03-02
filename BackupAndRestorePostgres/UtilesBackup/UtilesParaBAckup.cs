using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReneUtiles;
using ReneUtiles.Clases;
using Delimon.Win32.IO;


namespace BackupAndRestorePostgres.UtilesBackup
{
    public class UtilesParaBackup
    {

        public static string getStrCarpetaCorrecta(string carpeta) {
            if (carpeta.EndsWith("\\")) {
                return carpeta;
            }
            if (carpeta.EndsWith("/"))
            {
                return carpeta;
            }
            return carpeta + "\\";
        }
        public static string adaptarExtencioBackup(string direccion) {
            return Archivos.setExtencionStr(direccion, ".backup");
        }
        public static string BackupDatabase(
            string server,
            string port,
            string user,
            string password,
            string dbname,
            string direccion,
            //string backupdir,
            //string backupFileName,
            string backupCommandDir)
        {
            try
            {

                Environment.SetEnvironmentVariable("PGPASSWORD", password);
                direccion = adaptarExtencioBackup(direccion);

                string backupFile = direccion; //backupdir + backupFileName + "_" + DateTime.Now.ToString("yyyy") + "_" + DateTime.Now.ToString("MM") + "_" + DateTime.Now.ToString("dd") + ".backup";

                string BackupString = " -f \"" + backupFile + "\" -F c" +
                  " -h " + server + " -U " + user + " -p " + port + " -d " + dbname;


                Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = backupCommandDir + "\\pg_dump.exe";

                proc.StartInfo.Arguments = BackupString;

                proc.StartInfo.RedirectStandardOutput = true;//for error checks BackupString
                proc.StartInfo.RedirectStandardError = true;


                proc.StartInfo.UseShellExecute = false;//use for not opening cmd screen
                proc.StartInfo.CreateNoWindow = true;//use for not opening cmd screen


                proc.Start();
                proc.WaitForExit();
                proc.Close();

                return backupFile;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static string RestoreDatabase(
            string server,
            string port,
            string user,
            string password,
            string dbname,
            string direccion,
            //string backupdir,
            //string backupFileName,
            string backupCommandDir)
        {
            try
            {

                Environment.SetEnvironmentVariable("PGPASSWORD", password);

                string backupFile = direccion;//backupdir + backupFileName + "_" + DateTime.Now.ToString("yyyy") + "_" + DateTime.Now.ToString("MM") + "_" + DateTime.Now.ToString("dd") + ".backup";

                string conexion = "--host="+ server + " --port="+ port + " --username="+ user + " --no-password";
                string[] comandos = {
                    
                    "cd /D \""+backupCommandDir+"\"",
                    "dropdb "+ conexion+" --if-exists -f "+dbname,
                    "createdb "+ conexion+" --owner="+user+" "+dbname,
                     "pg_restore "+ conexion+" -d "+dbname+" -v \""+direccion+"\""
                };

                string BackupString = ReneUtiles.Utiles.join(comandos,"&&");

                //foreach (var item in comandos)
                //{
                //    Console.WriteLine(item);
                //}

               // Console.WriteLine(BackupString);



                FileInfo f= Archivos.crearTEXTO(new DirectoryInfo( System.IO.Directory.GetCurrentDirectory()), "ejectuar", ".bat", "@echo\n", "SET PGPASSWORD="+password,  BackupString);


                ReneUtiles.Utiles.ejecutarCMD_Visible(
                    urlExe:f.ToString() //@"C:\Windows\System32\cmd.exe"
                                       );
                //  " -f \"" + backupFile + "\" -F c" +
                //" -h " + server + " -U " + user + " -p " + port + " -d " + dbname;


                //Process proc = new System.Diagnostics.Process();
                //proc.StartInfo.FileName = "cmd.exe"; //backupCommandDir + "\\pg_restore.exe";

                //proc.StartInfo.Arguments = BackupString;

                //proc.StartInfo.RedirectStandardOutput = false;//for error checks BackupString
                //proc.StartInfo.RedirectStandardError = false;


                //proc.StartInfo.UseShellExecute = false;//use for not opening cmd screen
                //proc.StartInfo.CreateNoWindow = true;//use for not opening cmd screen


                //proc.Start();
                //proc.WaitForExit();
                //proc.Close();

                return backupFile;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
