using System;
using System.Windows;

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