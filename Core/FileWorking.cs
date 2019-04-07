using System.Windows.Forms;

namespace GenArt.Classes
{
    public static class FileWorking
    {
     
        //all format of files that can open 
        public static string imageType = "jpg files (*.jpg)|*.jpg|bmp files (*.bmp)|*.bmp|jpeg files (*.jpeg)|*.jpeg|All files (*.*)|*.*";

        //open file
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
