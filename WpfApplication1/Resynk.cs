using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Resynk
{
    class Resynk
    {
        private Window1 _window = null;
        private string path = "";
        private string ifile = "";
        private string ofile = "";
        private int milToAdd = -1;
        private Time tapartirde = null;
        private Encoding enc = null;



        public Resynk(Window1 win)
        {
            _window = win;
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

        public void resynk(string path, string ifile, int milToAdd, Time tapartirde, Encoding enc)
        {
            this.path = path;
            this.ifile = ifile;
            this.milToAdd = milToAdd;
            this.tapartirde = tapartirde;
            this.enc = enc;


            //string ofile = "synk_" + ifile;
            this.ofile = ifile.Substring(0, ifile.Length - 5) + "_synk.srt";
            

            //Encoding enc = ObtientENcoding(path + "\\" + ifile);

            string[] lines = System.IO.File.ReadAllLines(@path + "\\" + ifile, enc);

            resynk(lines);
            

            // MoveFocus path + "\\" + ifile to old
            //String newifile = ifile.Substring(0, ifile.Length - 5) + "_old.srt";
            //String newofile = ifile;

            //System.Threading.Thread.Sleep(2000);

            //System.IO.File.Move(path + "\\" + ifile, path + "\\" + newifile);
            //System.IO.File.Move(path + "\\" + ofile, path + "\\" + newofile);
        }

        public void resynk(string[] lines)
        {
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

            // output file
            //System.IO.StreamWriter file = new System.IO.StreamWriter(path + "\\" + ofile);
            FileStream fs = File.Create(path + "\\" + ofile);
            StreamWriter file = new StreamWriter(fs, enc);

            total = getNb(lines, chi);
            _window.pb.Visibility = System.Windows.Visibility.Visible;

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
                    // binding???
                    _window.Percent = (float)cpt * 100.0 / (float)total;
                    _window.pb.Value = _window.Percent;

                    /*
                    if (cpt % 50 == 0)
                    {
                        lol = pb.Value;
                        Alert("" + Percent + "% \n val: " + lol);
                    }
                    */

                }

                var m = tps.Match(line);
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
            fs.Close();
        }
    }
}
