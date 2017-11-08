using System;
using System.Linq;
using System.ServiceProcess;

namespace TouchpadManager
{
    /// <summary>
    /// Class holding the program entry point.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            if (args != null && args.FirstOrDefault() == "console")
            {
                var handler = new TouchpadHandler();
                handler.Start();
                Console.WriteLine("Hit enter key to exit");
                Console.ReadLine();
                handler.Stop();
                return;
            }

            var servicesToRun = new ServiceBase[]
            {
                new TouchpadService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
