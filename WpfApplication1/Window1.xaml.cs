using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Resynk
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private string Filename { get; set; }
        //public int cpt { get; set; }


        #region INotifyPropertyChanged
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private double _percent;
        public double Percent
        {
            get { return this._percent; }
            set
            {
                this._percent = value;
                NotifyPropertyChange("Percent");
            }
        }

        /// <summary>
        /// Permet de tester l'encodage utilisé pour le fichier texte dont le chemin est fourni
        /// </summary>
        /// <param name="cheminFichier">Chemin du fichier</param>
        /// <returns>Encodage du fichier Texte</returns>
        private static Encoding ObtientENcoding(string cheminFichier)
        {
            /*
             * Source:
             * http://codes-sources.commentcamarche.net/source/35661-c-fonction-permettant-d-obtenir-l-encodage-d-un-fichier-texte
             */
            Encoding enc = null;
            FileStream file = new FileStream(cheminFichier, FileMode.Open, FileAccess.Read, FileShare.Read);
	        if (file.CanSeek)
	        {
		        byte[] bom = new byte[4]; // Get the byte-order mark, if there is one
		        file.Read(bom, 0, 4);
		        if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) // utf-8
		        {
			        enc = Encoding.UTF8;
		        }
		        else if ((bom[0] == 0xff && bom[1] == 0xfe) || // ucs-2le, ucs-4le, and ucs-16le
			        (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff)) // ucs-4
		        {
			        enc = Encoding.Unicode;
		        }
		        else if (bom[0] == 0xfe && bom[1] == 0xff) // utf-16 and ucs-2
		        {
			        enc = Encoding.BigEndianUnicode;
		        }
		        else // ANSI, Default
		        {
			        enc = Encoding.Default;
		        }
		        // Now reposition the file cursor back to the start of the file
		        file.Seek(0, SeekOrigin.Begin);
	        }
	        else
	        {
		        // The file cannot be randomly accessed, so you need to decide what to set the default to
		        // based on the data provided. If you're expecting data from a lot of older applications,
		        // default your encoding to Encoding.ASCII. If you're expecting data from a lot of newer
		        // applications, default your encoding to Encoding.Unicode. Also, since binary files are
		        // single byte-based, so you will want to use Encoding.ASCII, even though you'll probably
		        // never need to use the encoding then since the Encoding classes are really meant to get
		        // strings from the byte array that is the file.

		        enc = Encoding.Default;
	        }
            return enc;
        }

        public Window1()
        {
            this.Init();

            this.TbFic.Text = "lolilol";
            this.Filename = "";
        }

        private void Init()
        {
            InitializeComponent();
            DataContext = this;

            Percent = 50;
        }

        public Window1(string name, string filename)
        {
            this.Init();

            this.TbFic.Text = name;
            this.Filename = filename;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            // si tout n'est pas vide
            if (this.Coutn(this.SpTemps) != 4)
                this.BResynk.IsEnabled = true;
        }

        /// <summary>
        /// Teste si les input sont tous vides
        /// </summary>
        /// <param name="sp">StackPanel contenant les input</param>
        /// <returns>Le nombre d'input vides</returns>
        private int Coutn(StackPanel sp)
        {
            int cpt = 0;
            foreach (var elem in sp.Children)
            {
                if (elem.GetType().Name == "TextBox")
                    if ((elem as TextBox).Text == "0")
                        ++cpt;
            }
            return cpt;
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            // si tout n'est pas vide
            if ( this.Coutn(this.SpTemps) != 4 )
                this.BResynk.IsEnabled = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow w = new MainWindow();
            w.Show();
            Application.Current.MainWindow = w;
            this.Close();
        }

        private void bResynk_Click(object sender, RoutedEventArgs e)
        {
            string path = "";
            int totalMil = 0;
            StackPanel sp = this.SpTemps;
            int mili = 0;
            int sec = 0;
            int min = 0;
            int heu = 0;

            if(this.Filename == "")
                return;

            path = this.TbFic.Text.Substring(0, this.TbFic.Text.Count() - 1 - this.Filename.Count()); 

            try
            {
                mili = int.Parse(this.Z.Text);
                sec = int.Parse(this.S.Text);
                min = int.Parse(this.M.Text);
                heu = int.Parse(this.H.Text);
            }
            catch (FormatException fe)
            {
                Alert("Seulement des nombres");
                return;
            }
            
            totalMil += mili + (sec + 60 * min + 3600 * heu) * 1000;
            
            // Gere le signe
            if (this.RbMoins.IsChecked == true)
            {
                if (totalMil > 0)
                    totalMil *= -1;
            }
            else if (this.RbPlus.IsChecked == true)
            {
                if (totalMil < 0)
                    totalMil *= -1;
            }
            //---------------


            //string apartir = "" + exh.ToString()
            try
            {
                mili = int.Parse(this.Exz.Text);
                sec = int.Parse(this.Exs.Text);
                min = int.Parse(this.Exm.Text);
                heu = int.Parse(this.Exh.Text);
            }
            catch (FormatException fe)
            {
                Alert("Seulement des nombres");
                return;
            }
            Time t = new Time(heu, min, sec, mili);
            //------------------------
            this.Resynk(path, this.Filename, totalMil, t);
            
            Alert("Syncro terminée.");

            this.Button_Click_1(new object(), new RoutedEventArgs());
        }


        private void Resynk(string path, string ifile, int milToAdd, Time tapartirde)
        {
            var resynk = new SrtResynk(this);

            resynk.Resynk(path, ifile, milToAdd, tapartirde,
                        ObtientENcoding(path + "\\" + ifile));

            this.Pb.Visibility = Visibility.Visible;
        }

        private void Alert(string msg)
        {
            MessageBox.Show(msg);
        }
    }
}
