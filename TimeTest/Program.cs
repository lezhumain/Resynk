using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resynk;

namespace TimeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Time t1 = new Time();
            Time t2 = new Time();

            t1.Parse("00:01:45,000");
            t2.Parse("00:02:59,000");

            Console.WriteLine("t1 = " + t1.ToString() + "\n\t+1000ms");
            t1.AddMil(1000);
            Console.WriteLine("t1 = " + t1.ToString() + "\n\t+1s");
            t1.AddSec(1);
            Console.WriteLine("t1 = " + t1.ToString() + "\n\t+40s");
            t1.AddSec(40);
            Console.WriteLine("t1 = " + t1.ToString() + "\n\t+59mn");
            t1.AddMin(59);
            Console.WriteLine("t1 = " + t1.ToString() + "\n\t+3600000ms");
            t1.AddMil(3600000);
            Console.WriteLine("t1 = " + t1.ToString());
            

            Console.WriteLine("Press key ");
            Console.ReadKey();
        }
    }
}
