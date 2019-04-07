using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;

namespace GenArt.Classes
{
    public static class FileWorking
    {
        
        public static string imageType = "jpg files (*.jpg)|*.jpg|gif files (*.gif)|*.gif|bmp files (*.bmp)|*.bmp|jpeg files (*.jpeg)|*.jpeg|All files (*.*)|*.*";

        public static string saveFileName(string filter)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.Filter = filter;
            if (!d.ShowDialog().Equals(DialogResult.Cancel))
                return d.FileName;
            return null;
        }

        public static string openFileName(string filter)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = filter;
            if (!d.ShowDialog().Equals(DialogResult.Cancel))
                return d.FileName;
            return null;
        }
    }
}
