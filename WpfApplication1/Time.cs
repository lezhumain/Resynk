using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Resynk
{
    public class Time
    {
        private Regex reg;

        private int _h;
        public int h { get; set; }

        private int _m;
        public int m
        {
            get
            {
                return _m;
            }
            set
            {
                _m = value;
                if (value >= 60)
                {
                    _m = value % 60;
                    h += value / 60;
                }
                else if (value < 0)
                {
                    _m = 60 + value % 60;
                    h += value / 60;
                }
            }
        }

        private int _s;
        public int s
        {
            get
            {
                return _s;
            }
            set
            {
                _s = value;
                if (value >= 60)
                {
                    _s = value % 60;
                    m += value / 60;
                }
                else if (value < 0)
                {
                    _s = 60 + value % 60;
                    m += value / 60;
                }
            }
        }

        private int _z;
        public int z
        {
            get
            {
                return _z;
            }
            set
            {
                _z = value;
                if (value >= 1000)
                {
                    _z = value % 1000;
                    s += value / 1000;
                }
                else if (value < 0)
                {
                    _z = 1000 + value % 1000;
                    s += value / 1000;
                }
            }
        }

        public Time()
        {
            this.reg = new Regex(@"([0-9]{2}):([0-9]{2}):([0-9]{2}),([0-9]{3})", RegexOptions.IgnoreCase);
        }

        public Time(int hh, int mm, int ss, int zz)
        {
            this.reg = new Regex(@"([0-9]{2}):([0-9]{2}):([0-9]{2}),([0-9]{3})", RegexOptions.IgnoreCase);

            h = hh;
            m = mm;
            s = ss;
            z = zz;
        }

        public bool Parse(string t)
        {
            Match m = reg.Match(t);
            //si ligne de temps
            if (m.Success)
            {
                string tps1 = m.Groups[1].ToString();
                string tps2 = m.Groups[2].ToString();
                string tps3 = m.Groups[3].ToString();
                string tps4 = m.Groups[4].ToString();

                try
                {
                    h = int.Parse(tps1);
                    this.m = int.Parse(tps2);
                    s = int.Parse(tps3);
                    z = int.Parse(tps4);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            else
                return false;
        }

        public void AddMil(int mil)
        {
            if (mil == 0)
                return;

            int r = mil % 1000;
            int v = mil / 1000;

            this.z = (this.z + r);
            if (this.z < 0)
                this.z = 0;
            
            AddSec(v);
        }

        public void AddSec(int sec)
        {
            if (sec == 0)
                return;

            int r = sec % 60;
            int v = sec / 60;

            this.s = this.s + r;
            //this.s = (this.s + r) % 60;
            if (this.s < 0)
                this.s = 0;

            AddMin(v);
        }

        public void AddMin(int min)
        {
            if (min == 0)
                return;

            int r = min % 60;
            int v = min / 60;

            this.m = (this.m + r);
            if (this.m < 0)
                this.m = 0;
            
            AddHeu(v);
        }

        public void AddHeu(int heu)
        {
            /*
            if (heu == 0)
                return;
            
            int r = heu % 60;
            int v = heu / 60;

            this.h = (this.h + r) % 24;
            */
            this.h += heu;
            if (this.h < 0)
                this.h = 0;
        }

        public string ToString()
        {
            string s = "";

            if (h < 10)
                s += "0";

            s += "" + h + ":";

            if (m < 10)
                s += "0";

            s += "" + m + ":";

            if (this.s < 10)
                s += "0";

            s += "" + this.s + ",";

            if (z < 100)
                s += "0";
            if (z < 10)
                s += "0";

            s += "" + z;

            return s;
        }

        // overload operator ==
        public static bool operator ==(Time a, Time b)
        {
            if (a.h == b.h && a.m == b.m && a.s == b.s && a.z == b.z)
                return true;
            else
                return false;
        }

        // overload operator !=
        public static bool operator !=(Time a, Time b)
        {
            return !(a==b);
        }

        // overload operator >
        public static bool operator >(Time a, Time b)
        {
            if (a.h > b.h || a.m > b.m || a.s > b.s || a.z > b.z)
                return true;
            else
                return false;
        }

        // overload operator <
        public static bool operator <(Time a, Time b)
        {
            if (a.h < b.h || a.m < b.m || a.s < b.s || a.z < b.z)
                return true;
            else
                return false;
        }

        // overload operator >=
        public static bool operator >=(Time a, Time b)
        {
            if (a > b || a == b)
                return true;
            else
                return false;
        }

        // overload operator <=
        public static bool operator <=(Time a, Time b)
        {
            if (a < b || a == b)
                return true;
            else
                return false;
        }
    }
}
