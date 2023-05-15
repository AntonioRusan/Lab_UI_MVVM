using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryModel
{
    public class SplineData
    {
        public RawData InputRawData { get; set; }
        public int NumOfNodes { get; set; }
        public double LeftDerivative { get; set; }
        public double RightDerivative { get; set; }
        public List<SplineDataItem> SplineItemList { get; set; }
        public double Integral { get; set; }
        public SplineData(RawData rawData, double leftDeriv, double rightDeriv, int numOfNodes)
        {
            InputRawData = rawData;
            NumOfNodes = numOfNodes;
            LeftDerivative = leftDeriv;
            RightDerivative = rightDeriv;
            SplineItemList = new List<SplineDataItem>();
        }
        public int CreateSpline()
        {
            int nx = InputRawData.NumOfNodes;
            double[] x = Array.Empty<double>();
            if (InputRawData.IsUniformGrid)
                x = new double[2] { InputRawData.LeftBound, InputRawData.RightBound };
            else
                x = InputRawData.Nodes;
            int ny = 1;
            double[] y = InputRawData.Values;
            bool isUniform = InputRawData.IsUniformGrid;
            double[] scoeff = new double[ny * 4 * (nx - 1)];
            double[] bc = new double[2] { LeftDerivative, RightDerivative };

            int nsite = NumOfNodes;
            double[] site = new double[2] { InputRawData.LeftBound, InputRawData.RightBound };
            int ndorder = 3;
            int[] dorder = new int[3] { 1, 1, 1 };
            int nder = 3;
            double[] resultDeriv = new double[ny * nder * nsite];

            int nlim = 1;
            double[] llim = new double[1] { InputRawData.LeftBound };
            double[] rlim = new double[1] { InputRawData.RightBound };
            double[] resultInteg = new double[ny * nlim];

            int ret = -1;
            try
            {
                CubicSplineInterpoalte(nx, x, ny, y, isUniform, bc, scoeff, nsite, site, ndorder, dorder, resultDeriv, nlim, llim, rlim, resultInteg, ref ret);

                if (ret != 0)
                {
                    return ret;
                }
                double[] newGrid = new double[nsite];
                double step = (InputRawData.RightBound - InputRawData.LeftBound) / (NumOfNodes - 1);
                int k = 0;
                for (double i = InputRawData.LeftBound; k < NumOfNodes - 1; i += step, k++)
                {
                    newGrid[k] = i;
                }
                newGrid[k] = InputRawData.RightBound;
                for (int i = 0, ind = 0; i < nder * nsite - 2; i += 3, ind++)
                {
                    double value = resultDeriv[i];
                    double firstDeriv = resultDeriv[i + 1];
                    double secondDeriv = resultDeriv[i + 2];
                    SplineDataItem tmpSplineDataItem = new SplineDataItem(newGrid[ind], value, firstDeriv, secondDeriv);
                    SplineItemList.Add(tmpSplineDataItem);
                }

                Integral = resultInteg[0];

                return ret;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [DllImport("..\\..\\..\\..\\x64\\Debug\\CPP_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CubicSplineInterpoalte(int nx, double[] x, int ny, double[] y, bool isUniform, double[] bc, double[] scoeff, int nsite, double[] site,
                int ndorder, int[] dorder, double[] resultDeriv, int nlim, double[] llim, double[] rlim, double[] resultInteg, ref int ret);

    }
}
