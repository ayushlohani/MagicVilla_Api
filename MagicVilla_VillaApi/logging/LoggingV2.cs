namespace MagicVilla_VillaApi.logging
{
    public class LoggingV2 : ILogging
    {
        public void Log(string message, string type)
        {
            if (type == "error")
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Error:-" + message);
            }
            else
            {
                if (type == "warning")
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Warning:-" + message);
                }
                else
                {
                    Console.WriteLine(message);
                }
            }
        }
    }
}
