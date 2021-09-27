using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ScreenShotBot.Properties;

namespace ScreenShotBot
{
    static class Program
    {
        private static string appGuid = "BDFC6AC4-1C5A-4009-982B-CE5910D6E89";        
        
        [STAThread]
        static void Main()
        {
            // https://odetocode.com/blogs/scott/archive/2004/08/20/the-misunderstood-mutex.aspx
            using (Mutex mutex = new Mutex(false, appGuid))
            {
                if(!mutex.WaitOne(0, false))
                {
                    MessageBox.Show(
                        Resources.info_ApplicationAlreadyRunning, 
                        Resources.caption_info, 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                    return;
                }
   
                GC.Collect();

                string appDataDir =
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "ScreenShotBot");

                Directory.CreateDirectory(appDataDir);

                using (ILog log = new LogFile(Path.Combine(appDataDir, "ScreenShotBot.log"), false))
                {
                    log.WriteInfo(Resources.info_ApplicationStarted.Swap(Application.ProductVersion));

                    Settings.Load(log, Path.Combine(appDataDir, "Settings.xml"));
                    log.Tracing = Settings.Instance.LogTracing;

                    Application.SetHighDpiMode(HighDpiMode.SystemAware);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FormMain(log));

                    log.WriteInfo(Resources.info_ApplicationStopped);
                }
            }
        }
    }
}
