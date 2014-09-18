using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Resynk
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private string filename { get; set; }
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

        private double percent = 0;
        public double Percent
        {
            get { return this.percent; }
            set
            {
                this.percent = value;
                NotifyPropertyChange("Percent");
            }
        }



        public Window1()
        {
            InitializeComponent();
            tbFic.Text = "lolilol";
            filename = "";
            percent = 0;
        }

        public Window1(string name, string filename)
        {
            InitializeComponent();
            tbFic.Text = name;
            this.filename = filename;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            // si tout n'est pas vide
            if (coutn(spTemps) != 4)
                bResynk.IsEnabled = true;
        }

        /// <summary>
        /// Teste si les input sont tous vides
        /// </summary>
        /// <param name="sp">StackPanel contenant les input</param>
        /// <returns>Le nombre d'input vides</returns>
        private int coutn(StackPanel sp)
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
            if ( coutn(spTemps) != 4 )
                bResynk.IsEnabled = true;
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
            StackPanel sp = spTemps;
            int mili = 0;
            int sec = 0;
            int min = 0;
            int heu = 0;

            if(filename == "")
                return;

            path = tbFic.Text.Substring(0, tbFic.Text.Count() - 1 - filename.Count()); 

            try
            {
                mili = int.Parse(z.Text);
                sec = int.Parse(s.Text);
                min = int.Parse(m.Text);
                heu = int.Parse(h.Text);
            }
            catch (FormatException fe)
            {
                Alert("Seulement des nombres");
                return;
            }
            
            totalMil += mili + 100 * sec + 6000 * min + 360000 * heu;
            
            // Gere le signe
            if (rbMoins.IsChecked == true)
            {
                if (totalMil > 0)
                    totalMil *= -1;
            }
            else if (rbPlus.IsChecked == true)
            {
                if (totalMil < 0)
                    totalMil *= -1;
            }
            //---------------


            //string apartir = "" + exh.ToString()
            try
            {
                mili = int.Parse(exz.Text);
                sec = int.Parse(exs.Text);
                min = int.Parse(exm.Text);
                heu = int.Parse(exh.Text);
            }
            catch (FormatException fe)
            {
                Alert("Seulement des nombres");
                return;
            }
            Time t = new Time(heu, min, sec, mili);
            //------------------------
            resynk(path, filename, totalMil, t);
            
            Alert("Syncro terminée.");

            this.Button_Click_1(new object(), new RoutedEventArgs());
        }


        /// <summary>
        /// Retourne le nombre de blocs de sous-titre
        /// </summary>
        /// <param name="lines">Les lignes du fichier d'entrée</param>
        /// <param name="chi">La regex chiffre</param>
        /// <returns>Nombre de blocs de sous-titre</returns>
        private int getNb(string[] lines, Regex chi)
        {
            int cpt = 0;

            foreach (string line in lines)
            {
                string ligne = line;

                //si ligne de chiffre
                if (chi.Match(line).Success)
                    ++cpt;
            }

            return cpt;
        }

        private void resynk(string path, string ifile, int milToAdd, Time tapartirde)
        {
            string ofile = "synk_" + ifile;
            int cpt = 1;
            int total = 0;
            double lol = 0;
            /*
            Time tapartirde = new Time();
            
            if(!tapartirde.Parse(apartirde))
                return;
            */

            // Chiffre seul
            string patternC = @"^[0-9]+$";
            // Temps => 00:00:00,000 --> 00:00:00,000
            string patternT = @"^([0-9]{2}:[0-9]{2}:[0-9]{2},[0-9]{3}) --> ([0-9]{2}:[0-9]{2}:[0-9]{2},[0-9]{3})\s?$";

            // Instantiate the regular expression object.
            Regex chi = new Regex(patternC, RegexOptions.IgnoreCase);
            Regex tps = new Regex(patternT, RegexOptions.IgnoreCase);

            string[] lines = System.IO.File.ReadAllLines(@path + "\\" + ifile);

            // output file
            System.IO.StreamWriter file = new System.IO.StreamWriter(path + "\\" + ofile);

            total = getNb(lines, chi);

            foreach (string line in lines)
            {
                string ligne = line;

                //si ligne de chiffre
                if (chi.Match(line).Success)
                {
                    int test = int.Parse(line);
                    if (int.Parse(line) != cpt)
                        ligne = cpt.ToString();
                    ++cpt;
                    // StatusBar
                    //Percent = (float)cpt * 100.0 / (float)total;

                    pb.Value = (float)cpt * 100.0 / (float)total;
                    lol = pb.Value;
                }

                Match m = tps.Match(line);
                //si ligne de temps
                if (m.Success)
                {
                    string tps1 = m.Groups[1].ToString();
                    string tps2 = m.Groups[2].ToString();

                    Time ti1 = new Time();
                    ti1.Parse(tps1);

                    Time ti2 = new Time();
                    ti2.Parse(tps2);

                    /*
                     * FAIRE operateur= pour Time
                     * faire bool passer = true
                     * ti1 >= tapartirde => passer = false
                     * 
                     */
                    if (ti1 >= tapartirde)
                    {
                        ti1.AddMil(milToAdd);
                        ti2.AddMil(milToAdd);

                        ligne = ti1.ToString() + " --> " + ti2.ToString();

                        int i = 0;
                    }
                }


                // Use a tab to indent each line of the file.
                //Console.WriteLine("\t" + ligne);
                file.WriteLine(ligne);
            }

            file.Close();
        }

        private void Alert(string msg)
        {
            MessageBox.Show(msg);
        }
    }
}
