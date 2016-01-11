using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using CefSharp;

namespace TumblrTVScreensaver
{
    static class Program
    {
        static void ShowScreensaver()
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                ScreensaverForm sf = new ScreensaverForm(screen.Bounds);
                sf.Show();
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string option = args.Length > 0 ? args[0].ToLower().Trim() : "";

            if (option == "/c")
            {
                Console.WriteLine("Launching in Configuration Mode");
                Application.Run(new SettingsForm());
            }

            Cef.Initialize(new CefSettings(), shutdownOnProcessExit: true, performDependencyCheck: true);

            // no args just means freeform
            if (args.Length == 0)
            {
                Console.WriteLine("Launching in Freeform Mode");
                Application.Run(new ScreensaverForm());
            }

            if (option == "/p")
            {
                Console.WriteLine("Launching in Preview Mode");
                if (args.Length < 2)
                {
                    Console.WriteLine("No handle provided! Exiting!");
                    Cef.Shutdown();
                    Application.Exit();
                }
                IntPtr previewWindowHandle = new IntPtr(long.Parse(args[1]));
                Application.Run(new ScreensaverForm(previewWindowHandle));
            }

            if (option == "/s")
            {
                Console.WriteLine("Launching in Fullscreen Mode");
                ShowScreensaver();
                Application.Run();
            }

        }
    }
}
