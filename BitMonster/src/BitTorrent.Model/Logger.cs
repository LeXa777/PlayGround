using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace BitTorrent.Model
{
   public static class Logger
   {
      private enum LoggerType
      {
         Error,
         Warning,
         Info
      }

      public static readonly string File;
      private static object _lock = new object();
      static Logger()
      {
         var now = DateTime.Now;
         var appdata = Environment.ExpandEnvironmentVariables("%appdata%");
         var appName = "BitMonster";
         var folder = "logs";
         var day = now.ToString("MMMM");
         var time = string.Format("{0} {1} - {2}.{3}.{4}.log", now.DayOfWeek, now.ToLongDateString(), now.ToString("HH"), now.ToString("mm"), now.Second.ToString("00"));

         File = Path.Combine(appdata, appName, folder, day, time);
         var di = new FileInfo(File).Directory;

         if (di != null && !di.Exists)
         {
            di.Create();
         }
//         _file = new FileInfo(path);
      }

      # region Public Methods

      public static void Error(string msg, Exception e)
      {

      }

      public static void Error(Exception e)
      {
         var className = e.TargetSite.DeclaringType != null ? e.TargetSite.DeclaringType.ToString() : "unknown";
         var method = e.TargetSite.Name;
         var msg = string.Format("{0} - [{1}]", e.Message, e.StackTrace);
         Write(LoggerType.Error, className, method, msg);
      }

//      public static void Error(string msg)
//      {
//
//      }

      public static void Warning(string msg, string className, string method)
      {
         Write(LoggerType.Warning, className, method, msg);
      }

      public static void Info(string msg, string className, string method)
      {
         Write(LoggerType.Info, className, method, msg);
      }

      # endregion

      private static void Write(LoggerType type, string className, string method, string msg)
      {
         lock (_lock)
         {
            try
            {
               var finalMsg = string.Format("{0}\t***{1,-12}\t{2,-20} {3,-35} {4}", DateTime.Now.ToString("HH:mm:ss"), 
                  type.ToString().ToLower(), className, method + "()", msg);
               System.IO.File.AppendAllText(File, finalMsg.Replace("\n", "").Replace("\r", "") + "\n");
            }
            catch (Exception ex)
            {
               Debug.WriteLine(ex);
            }
         }
      }
   }
}
