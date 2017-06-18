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
        private static Dictionary<string, TextBox> controls = new Dictionary<string, TextBox>();
        private static Dictionary<string, double> doubleMin = new Dictionary<string, double>();
        private static Dictionary<string, double> doubleMax = new Dictionary<string, double>();
        private static Dictionary<string, int> intMin = new Dictionary<string, int>();
        private static Dictionary<string, int> intMax = new Dictionary<string, int>();

        public SettingsForm()
        {
            InitializeComponent();
            Screensaver.SetFields();
            foreach (string name in controls.Keys)
            {
                Label label = new Label();
                label.Text = name;
                flowLayoutPanel1.Controls.Add(label);
                flowLayoutPanel1.Controls.Add(controls[name]);
            }
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
        /// If you don't want a min or max just use double.MinValue or double.MaxValue.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public static void AddDoubleField(string name, double min, double max)
        {
            controls.Add(name, new TextBox());
            doubleMin.Add(name, min);
            doubleMax.Add(name, max);
        }

        /// <summary>
        /// Adds a integer field to the settings form.
        /// This will be saved in registry for use when the program starts.
        /// If you don't want a min or max just use int.MinValue or int.MaxValue.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public static void AddIntegerField(string name, int min, int max)
        {
            controls.Add(name, new TextBox());
            intMin.Add(name, min);
            intMax.Add(name, max);
        }

        /// <summary>
        /// Adds a string field to the settings form.
        /// This will be saved in registry for use when the program starts.
        /// </summary>
        /// <param name="name"></param>
        public static void AddStringField(string name)
        {
            controls.Add(name, new TextBox());
        }

        public static string GetStringField(string name)
        {
            return (string)registryKey.GetValue(name);
        }

        public static double GetDoubleField(string name)
        {
            return double.Parse((string)registryKey.GetValue(name));
        }

        public static int GetIntegerField(string name)
        {
            return int.Parse((string)registryKey.GetValue(name));
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            foreach (string name in controls.Keys)
            {
                string contents = controls[name].Text;
                if (doubleMin.ContainsKey(name))
                {
                    double result;
                    if (!double.TryParse(contents, out result))
                    {
                        MessageBox.Show(name + " must be a double!");
                        return;
                    }
                    if (doubleMin[name] > result || doubleMax[name] < result)
                    {
                        MessageBox.Show(name + " must be within " + doubleMin[name] + " and " + doubleMax[name] + "!");
                        return;
                    }
                    registryKey.SetValue(name, controls[name]);
                }
                else if (intMin.ContainsKey(name))
                {
                    int result;
                    if (!int.TryParse(contents, out result))
                    {
                        MessageBox.Show(name + " must be a integer!");
                        return;
                    }
                    if (doubleMin[name] > result || doubleMax[name] < result)
                    {
                        MessageBox.Show(name + " must be within " + doubleMin[name] + " and " + doubleMax[name] + "!");
                        return;
                    }
                    registryKey.SetValue(name, controls[name]);
                }
                else
                {
                    registryKey.SetValue(name, controls[name]);
                }
            }
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
