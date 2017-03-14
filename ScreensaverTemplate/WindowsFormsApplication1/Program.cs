using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
            {
                string firstArgument = args[0].ToLower().Trim();
                string secondArgument = null;

                //Handle cases where arguments are separated by colon.
                //Examples: /c:1234567 or /P:1234567
                if (firstArgument.Length > 2)
                {
                    secondArgument = firstArgument.Substring(3).Trim();
                    firstArgument = firstArgument.Substring(0, 2);
                }
                else if (args.Length > 1)
                {
                    secondArgument = args[1];
                }
                if (firstArgument == "/c")           // Configuration Mode
                {
                    Application.Run(new SettingsForm());
                }
                else if (firstArgument == "/p")           // Preview Mode
                {
                    if (secondArgument == null)
                    {
                        MessageBox.Show("Sorry, but the expected window handle was not provided.", "ScreenSaver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    IntPtr previewWndHandle = new IntPtr(long.Parse(secondArgument));
                    Application.Run(new Screensaver(previewWndHandle));
                }
                else if (firstArgument == "/s")           // Full-screen mode
                {
                    ShowScreenSaver();
                    Application.Run();
                }
                else
                {
                    MessageBox.Show("Sorry, but the command line argument \"" + firstArgument + "\" is not valid.", "ScreenSaver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Application.Exit();
                }
            }
            else // No arguments - treat like /c
            {
                Application.Run(new SettingsForm());
            }
        }

        static void ShowScreenSaver()
        {
            Rectangle bounds = new Rectangle();
            foreach (Screen s in Screen.AllScreens)
            {
                bounds = Rectangle.Union(bounds, s.Bounds);
            }

            Screensaver screenForm = new Screensaver(bounds);
            screenForm.Show();
        }
    }
}
