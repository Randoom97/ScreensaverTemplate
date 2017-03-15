using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class SettingsForm : Form
    {
        private static RegistryKey registryKey;
        private static Dictionary<string, Control> control = new Dictionary<string, Control>();
        private static Dictionary<string, double> doubleMin = new Dictionary<string, double>();
        private static Dictionary<string, double> doubleMax = new Dictionary<string, double>();
        private static Dictionary<string, int> intMin = new Dictionary<string, int>();
        private static Dictionary<string, int> intMax = new Dictionary<string, int>();

        public SettingsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the registry subfolder to save the values in.
        /// </summary>
        /// <param name="key"></param>
        public static void SetRegistryKey(string key)
        {
            registryKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\" + key);
        }

        /// <summary>
        /// Adds a double field to the settings form.
        /// This will be saved in registry for use when the program starts.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public static void AddDoubleField(string name, double min, double max)
        {
            control.Add(name, new TextBox());
            doubleMin.Add(name, min);
            doubleMin.Add(name, max);
        }

        /// <summary>
        /// Adds a integer field to the settings form.
        /// This will be saved in registry for use when the program starts.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public static void AddIntegerField(string name, int min, int max)
        {
            control.Add(name, new TextBox());
            intMin.Add(name, min);
            intMin.Add(name, max);
        }

        /// <summary>
        /// Adds a string field to the settings form.
        /// This will be saved in registry for use when the program starts.
        /// </summary>
        /// <param name="name"></param>
        public static void AddStringField(string name)
        {
            control.Add(name, new TextBox());
        }

        public static string GetStringField(string name)
        {
            return control[name].Text;
        }

        public static double GetDoubleField(string name)
        {
            return double.Parse(control[name].Text);
        }

        public static int GetIntegerField(string name)
        {
            return int.Parse(control[name].Text);
        }

    }
}
