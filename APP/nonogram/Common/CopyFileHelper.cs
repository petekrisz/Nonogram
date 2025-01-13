using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace nonogram.Common
{
    public static class CopyFileHelper
    {
        public static void CopyCsvFilesOnExit()
        {
            string sourceDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DB");
            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            string targetDirectory = Path.Combine(projectDirectory, "DB");

            foreach (var filePath in Directory.GetFiles(sourceDirectory, "*.csv"))
            {
                string fileName = Path.GetFileName(filePath);
                string targetPath = Path.Combine(targetDirectory, fileName);
                try
                {
                    File.Copy(filePath, targetPath, overwrite: true);
                    //Console.WriteLine($"Copied {fileName} to {targetPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error copying {fileName}: {ex.Message}");
                }
            }
        }
    }
}
