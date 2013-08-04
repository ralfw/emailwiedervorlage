using System;
using System.IO;

namespace ewv.server.adapter
{
    public static class LogAdapter
    {
        public static void Log(Exception ex)
        {
            Log("*** Fehler (Details s. crashdump): {0}", ex.Message);

            File.AppendAllText("crashdump.txt", string.Format("=== {0} ===\n", DateTime.Now));
            File.AppendAllText("crashdump.txt", ex.ToString());
            File.AppendAllText("crashdump.txt", ex.StackTrace + "\n");
        }

        public static void Log(string format, params object[] args)
        {
            File.AppendAllText("log.txt", string.Format("{0}: {1}\n", DateTime.Now, string.Format(format, args)));
        }
    }
}