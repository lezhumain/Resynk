using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Resynk
{
    public class Time
    {
        protected bool Equals(Time other)
        {
            return Equals(this._reg, other._reg) && this._date.Equals(other._date);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this._reg != null ? this._reg.GetHashCode() : 0)*397) ^ this._date.GetHashCode();
            }
        }

        private readonly Regex _reg = new Regex(@"([0-9]{2}):([0-9]{2}):([0-9]{2}),([0-9]{3})", RegexOptions.IgnoreCase);
        private DateTime _date;

        public static Time MinValue
        {
            get { return new Time(0, 0, 0, 0); }
        }

        public static Time MaxValue
        {
            get { return new Time(23, 59, 59, 999); }
        }

        private const int MaxHour = 23;

        public int H
        {
            get { return _date.Hour; }
            private set { _date = _date.AddHours(value - H); }
        }

        public int M
        {
            get { return _date.Minute; }
            private set { _date = _date.AddMinutes(value - M); }
        }

        public int S
        {
            get { return _date.Second; }
            private set { _date = _date.AddSeconds(value - S); }
        }

        public int Z
        {
            get { return _date.Millisecond; }
            private set { _date = _date.AddMilliseconds(value - Z); }
        }

        private void SetDateTime(int hh, int mm, int ss, int zz)
        {
            try
            {
                _date = new DateTime(1, 1, 1, hh, mm, ss, zz);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.WriteLine("erreur : " + e.Message);
            }
        }

        public Time()
        {
        }

        public Time(int hh, int mm, int ss, int zz)
        {
            SetDateTime(hh, mm, ss, zz);
        }

        public bool Parse(string t)
        {
            var m = this._reg.Match(t);
            //si ligne de temps
            if (!m.Success)
                return false;

            var tps1 = m.Groups[1].ToString();
            var tps2 = m.Groups[2].ToString();
            var tps3 = m.Groups[3].ToString();
            var tps4 = m.Groups[4].ToString();

            try
            {
                this.SetDateTime(int.Parse(tps1),
                    int.Parse(tps2),
                    int.Parse(tps3),
                    int.Parse(tps4));
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void AddMil(int mil)
        {
            var newMil = mil + _date.Millisecond;
            TotalMilliseconds = newMil < 0
                ? 0
                : newMil;
        }

        public void AddSec(int sec)
        {
            _date = _date.AddSeconds(sec);
        }

        public void AddMin(int min)
        {
            _date = _date.AddMinutes(min);
        }

        public void AddHeu(int heu)
        {
            var newHeur = heu + _date.Hour;
            H = newHeur > MaxHour
                ? MaxHour
                : newHeur;
        }

        public override string ToString()
        {
            var s = "";

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

        // overload operator ==
        public static bool operator ==(Time a, Time b)
        {
            if (a == null && b == null)
                return true;

            return a != null && b != null && a.ToString() == b.ToString();
        }

        // overload operator !=
        public static bool operator !=(Time a, Time b)
        {
            return !(a == b);
        }

        // overload operator >
        public static bool operator >(Time a, Time b)
        {
            return a._date > b._date;
        }

        // overload operator <
        public static bool operator <(Time a, Time b)
        {
            return !(a > b || a == b);
        }

        // overload operator >=
        public static bool operator >=(Time a, Time b)
        {
            return a > b || a == b;
        }

        // overload operator <=
        public static bool operator <=(Time a, Time b)
        {
            return a < b && a == b;
        }

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj)
                   && ReferenceEquals(this, obj)
                   && !(obj.GetType() != this.GetType())
                   && Equals((Time) obj);
        }

        private TimeSpan TStamp
        {
            get { return (_date - new DateTime(_date.Year, _date.Month, _date.Day)); }
            set { _date = new DateTime(1, 1, 1, value.Hours, value.Minutes, value.Seconds, value.Milliseconds); }
        }

        private double TotalMilliseconds
        {
            get { return TStamp.TotalMilliseconds; }
            set { TStamp = new TimeSpan((long)value); }
        }
    }
}