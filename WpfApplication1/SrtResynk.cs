using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Resynk
{
    public class SrtResynk
    {
        private Window1 _window;
        private string _path = "";
        private string _ifile = "";
        private string _ofile = "";
        private int _milToAdd = -1;
        private Time _tapartirde = null;
        private Encoding _enc = null;

        private bool _isTestMode;

        public List<string> TestContent { get; private set; }


        public SrtResynk(int milToAdd)
        {
            _isTestMode = true;
            _milToAdd = milToAdd;
            _tapartirde = new Time(0,0,0,0);
            _enc = new UTF8Encoding();
            this.TestContent = new List<string>();
        }

        public SrtResynk(Window1 win)
        {
            this.TestContent = null;
            _window = win;
        }

        /// <summary>
        /// Retourne le nombre de blocs de sous-titre
        /// </summary>
        /// <param name="lines">Les lignes du fichier d'entrée</param>
        /// <param name="chi">La regex chiffre</param>
        /// <returns>Nombre de blocs de sous-titre</returns>
        private int GetNb(string[] lines, Regex chi)
        {
            return lines.Count(line => chi.Match(line).Success);
        }

        public void Resynk(string path, string ifile, int milToAdd, Time tapartirde, Encoding enc)
        {
            this._path = path;
            this._ifile = ifile;
            this._milToAdd = milToAdd;
            this._tapartirde = tapartirde;
            this._enc = enc;


            //string ofile = "synk_" + ifile;
            this._ofile = this._ifile.Substring(0, ifile.Length - 5) + "_synk.srt";
            

            //Encoding enc = ObtientENcoding(path + "\\" + ifile);

            string[] lines = File.ReadAllLines(@path + "\\" + ifile, enc);

            this.ResynkLines(lines);
            

            // MoveFocus path + "\\" + ifile to old
            //String newifile = ifile.Substring(0, ifile.Length - 5) + "_old.srt";
            //String newofile = ifile;

            //System.Threading.Thread.Sleep(2000);

            //System.IO.File.Move(path + "\\" + ifile, path + "\\" + newifile);
            //System.IO.File.Move(path + "\\" + ofile, path + "\\" + newofile);
        }

        public bool ResynkLines(string[] lines)
        {
            if ((_isTestMode && _window != null) || (!_isTestMode && _window == null))
            {
                Debug.WriteLine("\tError with window or testMode...");
                return false;
            }

            int cpt = 1;
            
            /*
            cTime tapartirde = new cTime();
            
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

            FileStream fs = null;
            StreamWriter file = StreamWriter.Null;
            if (!_isTestMode)
            {
                fs = File.Create(this._path + "\\" + this._ofile);
                file = new StreamWriter(fs, this._enc);                
            }

            int total = this.GetNb(lines, chi);

            if(!_isTestMode && _window != null)
                _window.Pb.Visibility = System.Windows.Visibility.Visible;

            foreach (string line in lines)
            {
                string ligne = line;

                //si ligne de chiffre
                if (chi.Match(line).Success)
                {
                    if (int.Parse(line) != cpt)
                        ligne = cpt.ToString();
                    ++cpt;

                    // StatusBar
                    // binding???
                    if (_window != null)
                    {
                        _window.Percent = cpt * 100.0 / total;
                        _window.Pb.Value = _window.Percent;    
                    }
                }

                Match m = tps.Match(line);

                if (m.Success) //si ligne de temps
                {
                    string tps1 = m.Groups[1].ToString();
                    string tps2 = m.Groups[2].ToString();

                    Time ti1 = new Time();
                    ti1.Parse(tps1);

                    Time ti2 = new Time();
                    ti2.Parse(tps2);

                    if (ti1 >= this._tapartirde)
                    {
                        ti1.AddMil(this._milToAdd);
                        ti2.AddMil(this._milToAdd);

                        ligne = ti1.ToString() + " --> " + ti2.ToString();
                    }
                }

                if (!_isTestMode)
                    file.WriteLine(ligne);
                else
                    TestContent.Add(ligne);
            }

            // ReSharper disable once InvertIf
            if (!_isTestMode && fs != null)
            {
                file.Close();
                fs.Close();                
            }

            return true;
        }
    }
}
