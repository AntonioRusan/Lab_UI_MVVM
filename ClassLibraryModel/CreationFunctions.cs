using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryModel
{
    public static class CreationFunctions
    {
        public static double LinearFunc(double x)
        {
            return x;
        }
        public static double ThreePolynomFunc(double x)
        {
            return x * x * x;
        }

        public static double RandomValueFunc(double x)
        {
            Random rand = new Random();
            double max = 1;
            double min = -1;
            double val = rand.NextDouble() * (max - min) + min;
            return x + val;
        }
    }
}
