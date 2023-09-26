namespace HW2_Avito
{
    public class Logger
    {
        private static StreamWriter? logStream;
        private Logger()
        {
        }

        public static StreamWriter GetLogger()
        {
            if (logStream == null) 
                logStream = new StreamWriter("C:\\Users\\Valeriya\\source\\repos\\HW2_Avito\\HW2_Avito\\LogFile.txt", true);
            return logStream;
        }

        public static void Dispose()
        {
            logStream?.Dispose();
            logStream = null;
        }
    }
}
