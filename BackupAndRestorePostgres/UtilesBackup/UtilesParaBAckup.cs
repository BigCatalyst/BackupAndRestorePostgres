using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static string BackupDatabase(
            string server,
            string port,
            string user,
            string password,
            string dbname,
            string backupdir,
            string backupFileName,
            string backupCommandDir)
        {
            try
            {

                Environment.SetEnvironmentVariable("PGPASSWORD", password);

                string backupFile = backupdir + backupFileName + "_" + DateTime.Now.ToString("yyyy") + "_" + DateTime.Now.ToString("MM") + "_" + DateTime.Now.ToString("dd") + ".backup";

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
                    "dropdb "+ conexion+" --if-exists -f",
                    "createdb "+ conexion+" --owner="+user,
                     "pg_restore "+ conexion+"-d cinema -v \""+direccion+"\""
                };

                string BackupString = ReneUtiles.Utiles.join(comandos,"&&");
                
                
                  //  " -f \"" + backupFile + "\" -F c" +
                  //" -h " + server + " -U " + user + " -p " + port + " -d " + dbname;


                Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = backupCommandDir + "\\pg_restore.exe";

                proc.StartInfo.Arguments = BackupString;

                proc.StartInfo.RedirectStandardOutput = false;//for error checks BackupString
                proc.StartInfo.RedirectStandardError = false;


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
    }
}
