using System;
using System.IO;

namespace ewv.server.adapter
{
    public static class LogAdapter
    {
        private const string CRASHDUMP_FILENAME = "emailwiedervorlage.crashdump.txt";
        private const string LOG_FILENAME = "emailwiedervorlage.log.txt";


        public static void Log(Exception ex)
        {
            Log("*** Fehler (Details s. crashdump): {0}", ex.Message);

            File.AppendAllText(CRASHDUMP_FILENAME, string.Format("*** {0} ***\n", DateTime.Now));
            File.AppendAllText(CRASHDUMP_FILENAME, ex.ToString() + "\n");
        }


        public static void Log(string format, params object[] args)
        {
            File.AppendAllText(LOG_FILENAME, string.Format("{0}: {1}\n", DateTime.Now, string.Format(format, args)));
        }
    }
}