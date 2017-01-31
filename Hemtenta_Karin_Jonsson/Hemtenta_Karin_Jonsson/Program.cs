using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017
{
    class Program
    {
        static void Main(string[] args)
        {
            double max = double.MaxValue;
            double tiny = 0.1;

            double number = max + tiny;

            Console.WriteLine("{0}+{1} gives the result {2}", max, tiny, number);

            number = max + max;
            Console.WriteLine("{0}+{0} gives the result {1}", max, number);

            Console.ReadLine();
        }
    }
}
