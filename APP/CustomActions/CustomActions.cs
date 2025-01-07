using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Configuration.Install;
using System.Collections;
using System.ComponentModel;

namespace CustomActions
{
    [RunInstaller(true)]
    public class CustomActions : Installer
    {
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            string targetDir = Context.Parameters["targetdir"];
            string readMePath = Path.Combine(targetDir, "ReadMe.txt");

            if (File.Exists(readMePath))
            {
                Process.Start("notepad.exe", readMePath);
            }
            else
            {
                MessageBox.Show("ReadMe.txt file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
