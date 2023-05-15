using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryModel
{
    public struct SplineDataItem
    {
        public double Coordinate { get; set; }
        public double SplineValue { get; set; }
        public double FirstDerivative { get; set; }
        public double SecondDerivative { get; set; }
        public SplineDataItem(double x, double value, double firstDeriv, double secondDeriv)
        {
            Coordinate = x;
            SplineValue = value;
            FirstDerivative = firstDeriv;
            SecondDerivative = secondDeriv;
        }
        public string ToString(string format)
        {
            return $"Coordiante: {String.Format(format, Coordinate)};\nSplineValue: {String.Format(format, SplineValue)}\n" +
                $"FirstDerivative: {String.Format(format, FirstDerivative)}\nSecondDerivative {String.Format(format, SecondDerivative)}";
            
        }
        public override string ToString()
        {
            return $"Coordiante: {Coordinate}\n\tSplineValue: {SplineValue}\n\tFirstDerivative: {FirstDerivative}\n\tSecondDerivative {SecondDerivative}";
        }
    }
}
