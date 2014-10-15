using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Resynk
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension

            dlg.DefaultExt = ".srt";

            dlg.Filter = "Subtitles (.srt)|*.srt";



            // Display OpenFileDialog by calling ShowDialog method

            Nullable<bool> result = dlg.ShowDialog();



            // Get the selected file name and display in a TextBox

            if (result == true)
            {

                // Open document

                string filename = dlg.FileName;
                string name = System.IO.Path.GetFileName(filename);

                Window1 w = new Window1(filename, name);
                w.Show();
                Application.Current.MainWindow = w;
                this.Close();
            }
            
        }
    }
}
