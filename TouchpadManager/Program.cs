using System;
using System.Linq;
using System.ServiceProcess;
using log4net;
using log4net.Config;

namespace TouchpadManager
{
    /// <summary>
    /// Class holding the program entry point.
    /// </summary>
    public static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            try
            {
                XmlConfigurator.Configure();
                if (args != null && args.FirstOrDefault() == "console")
                {
                    Log.Info("Starting in console mode.");
                    var handler = new TouchpadHandler();
                    handler.Start();
                    Console.WriteLine("Hit enter key to exit");
                    Console.ReadLine();
                    Log.Info("Stopping.");
                    handler.Stop();
                    Log.Info("Stopped.");
                    return;
                }

                var servicesToRun = new ServiceBase[]
                {
                    new TouchpadService()
                };
                Log.Info("Starting service.");
                ServiceBase.Run(servicesToRun);
                Log.Info("Service stopped.");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Unexpected failure: {e.Message}");
                Log.Error("Unexpected failure", e);
                Environment.ExitCode = 1;
            }
        }
    }
}
