using System.Text.RegularExpressions;

namespace Resynk
{
    public class Time
    {
        protected bool Equals(Time other)
        {
            return Equals(this._reg, other._reg) && this.H == other.H && this.M == other.M && this.S == other.S && this.Z == other.Z && this.H == other.H;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Time)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (this._reg != null ? this._reg.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.H;
                hashCode = (hashCode * 397) ^ this.M;
                hashCode = (hashCode * 397) ^ this.S;
                hashCode = (hashCode * 397) ^ this.Z;
                hashCode = (hashCode * 397) ^ this.H;
                return hashCode;
            }
        }

        private Regex _reg;

        private int _h;
        public int H
        {
            get
            {
                return _h;
            }
            set
            {
                _h = value;

                if (_h < 0)
                {
                    _h = 0;
                    _m = 0;
                    _s = 0;
                    _z = 0;
                }
                else
                {
                    if (_h > 23)
                    {
                        _h = 23;
                        _m = 59;
                        _s = 59;
                        _z = 999;
                    }
                }
            }
        }

        private int _m;
        public int M
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
                    this.H += value / 60;
                }
                else if (value < 0)
                {
                    _m = 60 + value % 60;
                    this.H += value / 60 - 1;
                }
            }
        }

        private int _s;
        public int S
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
                    this.M += value / 60;
                }
                else if (value < 0)
                {
                    _s = 60 + value % 60;
                    this.M += value / 60 - 1;
                }
            }
        }

        private int _z;
        public int Z
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
                    this.S += value / 1000;
                }
                else if (value < 0)
                {
                    _z = 1000 + value % 1000;
                    this.S = this.S + value / 1000 - 1;
                }
            }
        }

        public Time()
        {
            this._reg = new Regex(@"([0-9]{2}):([0-9]{2}):([0-9]{2}),([0-9]{3})", RegexOptions.IgnoreCase);
        }

        public Time(int hh, int mm, int ss, int zz)
        {
            this._reg = new Regex(@"([0-9]{2}):([0-9]{2}):([0-9]{2}),([0-9]{3})", RegexOptions.IgnoreCase);

            this.H = hh;
            this.M = mm;
            this.S = ss;
            this.Z = zz;
        }

        public bool Parse(string t)
        {
            Match m = this._reg.Match(t);
            //si ligne de temps
            if (m.Success)
            {
                string tps1 = m.Groups[1].ToString();
                string tps2 = m.Groups[2].ToString();
                string tps3 = m.Groups[3].ToString();
                string tps4 = m.Groups[4].ToString();

                try
                {
                    this.H = int.Parse(tps1);
                    this.M = int.Parse(tps2);
                    this.S = int.Parse(tps3);
                    this.Z = int.Parse(tps4);
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

            this.Z = (this.Z + r);
            if (this.Z < 0)
                this.Z = 0;

            AddSec(v);
        }

        public void AddSec(int sec)
        {
            if (sec == 0)
                return;

            int r = sec % 60;
            int v = sec / 60;

            this.S = this.S + r;
            //this.s = (this.s + r) % 60;
            if (this.S < 0)
                this.S = 0;

            AddMin(v);
        }

        //public static cTime MinValue
        //{
        //    get { return new cTime(0, 0, 0, 0); }
        //}

        //public static cTime MaxValue
        //{
        //    get { return new cTime(23, 59, 59, 999); }
        //}

        public void AddMin(int min)
        {
            if (min == 0)
                return;

            int r = min % 60;
            int v = min / 60;

            this.M = (this.M + r);
            if (this.M < 0)
                this.M = 0;

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
            this.H += heu;
        }

        public override string ToString()
        {
            string s = "";

            if (this.H < 10)
                s += "0";

            s += "" + this.H + ":";

            if (this.M < 10)
                s += "0";

            s += "" + this.M + ":";

            if (this.S < 10)
                s += "0";

            s += "" + this.S + ",";

            if (this.Z < 100)
                s += "0";
            if (this.Z < 10)
                s += "0";

            s += "" + this.Z;

            return s;
        }

        public static Time MinValue => new Time(0, 0, 0, 0);

        public static Time MaxValue => new Time(23, 59, 59, 999);

        public static bool operator ==(Time a, Time b)
        {
            return (a == null && b == null) || (a != null && b!= null && a.H == b.H && a.M == b.M && a.S == b.S && a.Z == b.Z);
        }

        // overload operator !=
        public static bool operator !=(Time a, Time b)
        {
            return !(a == b);
        }

        // overload operator >
        public static bool operator >(Time a, Time b)
        {
            if (a.H > b.H || a.M > b.M || a.S > b.S || a.Z > b.Z)
                return true;
            else
                return false;
        }

        // overload operator <
        public static bool operator <(Time a, Time b)
        {
            if (a.H < b.H || a.M < b.M || a.S < b.S || a.Z < b.Z)
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

        // TODO: GetHashCode(), GetEquals()
    }
}
