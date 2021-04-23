using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace ScreenShotBot
{
    partial class DialogAbout : Form
    {
        private int _dance;

        public DialogAbout()
        {
            InitializeComponent();
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
        }

        #region Assembly Attribute Accessors

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        #endregion

        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
            }
        }

        private void danceTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                _dance += 1;

                if ((_dance + 10) % 20 == 0)
                {
                    textBoxDescription.Text = textBoxDescription.Text.Replace('\\', '/');
                }
                else if (_dance % 20 == 0)
                {
                    textBoxDescription.Text = textBoxDescription.Text.Replace('/', '\\');
                }

                if (_dance % 81 == 0)
                {
                    textBoxDescription.Text = textBoxDescription.Text.Replace("^_", "-_");
                }
                else if (_dance % 81 - 2 == 0)
                {
                    textBoxDescription.Text = textBoxDescription.Text.Replace("-_", "^_");
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
            }
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                // https://stackoverflow.com/a/61035650/998987
                Process.Start(
                    new ProcessStartInfo("https://www.zubspace.com")
                    {
                        UseShellExecute = true
                    });
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
            }
        }
    }
}
