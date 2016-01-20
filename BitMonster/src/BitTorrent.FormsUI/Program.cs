using System;
using System.IO;
using System.Windows.Forms;
using BitTorrent.Model;
using System.Diagnostics;

namespace BitTorrent.FormsUI
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);

         if (CheckAnotherProcessRunning())
         {
            MessageBox.Show("Another Process already running...");
            try
            {
               Application.Exit();
            }
            catch (Exception ex)
            {
               Logger.Error(ex);
            }
            return;
         }

         var cmd = string.Empty;
         var commands = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData;

         try
         {
            if (commands != null && commands.Length > 0 && !string.IsNullOrEmpty(commands[0]) && 
               Uri.IsWellFormedUriString(commands[0], UriKind.RelativeOrAbsolute))
            {
               var fi = new FileInfo(new Uri(commands[0]).LocalPath);

               if (fi.Exists)
               {
                  cmd = fi.FullName;
               }
            }

            Application.Run(new BitMonster(cmd));
         }
         catch (Exception ex)
         {
            Logger.Error(ex);
         }
      }

      private static bool CheckAnotherProcessRunning()
      {
         var thisProcess = Process.GetCurrentProcess();

         foreach (var p in Process.GetProcesses())
         {
            if (p.Id != thisProcess.Id && p.ProcessName == thisProcess.ProcessName)
            {
               if (p.MainWindowHandle == (IntPtr)0)
               {
                  p.Kill();
                  continue;
               }
               return true;
            }
         }
         return false;
      }
   }
}
