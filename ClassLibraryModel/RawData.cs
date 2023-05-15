using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryModel
{
    public delegate double FRaw(double x);
    public class RawData: IEnumerable<string>
    {
        public double LeftBound { get; set; }
        public double RightBound { get; set; }
        public int NumOfNodes { get; set; }
        public bool IsUniformGrid { get; set; }
        public FRaw fRaw { get; set; }
        public double[] Nodes { get; set; }
        public double[] Values { get; set; }

        IEnumerable <string> zipNodeValue;

        public RawData(double left, double right, int num, bool isUniform, FRaw fraw)
        {
            LeftBound = left;
            RightBound = right;
            NumOfNodes = num;
            IsUniformGrid = isUniform;
            fRaw = fraw;
            Nodes = new double[num];
            Values = new double[num];
            if (isUniform)
            {
                double step = (right - left) / (num - 1);
                int ind = 0;
                for (double i = left; ind < num - 1; i += step, ind++)
                {
                    Nodes[ind] = i;
                    Values[ind] = fraw(i);
                }
                Nodes[ind] = right;
                Values[ind] = fraw(right);
            }
            else
            {
                Random rand = new Random();
                double max = (right - left);
                double min = 0.01;
                Nodes[0] = left;
                Values[0] = fraw(left);
                for (int i = 1; i < num - 1; i++)
                {
                    double val = rand.NextDouble() * (max - min) + min;
                    Nodes[i] = left + val;
                }
                Nodes[num - 1] = right;
                Array.Sort(Nodes);
                for (int i = 0; i < num; i++)
                {
                    Values[i] = fraw(Nodes[i]);
                }
            }
            string format = "{0:0.000}";
            zipNodeValue = Nodes.Zip(Values, (node, value) => $"Coordinate: {String.Format(format, node)}\tValue: {String.Format(format, value)}");
        }

        public RawData(string filename)
        {
            StreamReader? reader = null;
            try
            {
                reader = new StreamReader(filename);
                string? str;
                str = reader.ReadLine();
                string[] fileItems = str.Split(' ');
                double left = Convert.ToDouble(fileItems[0]);
                double right = Convert.ToDouble(fileItems[1]);
                int numOfNodes = Convert.ToInt32(fileItems[2]);
                bool isUniform = Convert.ToBoolean(fileItems[3]);
                FRaw fraw = fileItems[4] switch
                {
                    "LinearFunc" => CreationFunctions.LinearFunc,
                    "ThreePolynomFunc" => CreationFunctions.ThreePolynomFunc,
                    "RandomValueFunc" => CreationFunctions.RandomValueFunc,
                    _ => CreationFunctions.LinearFunc
                };
                LeftBound = left;
                RightBound = right;
                NumOfNodes = numOfNodes;
                IsUniformGrid = isUniform;
                fRaw = fraw;
                Nodes = new double[numOfNodes];
                Values = new double[numOfNodes];
                int i = 0;
                while ((str = reader.ReadLine()) != null)
                {
                    string[] fileValues = str.Split(' ');
                    Nodes[i] = Convert.ToDouble(fileValues[0]);
                    Values[i] = Convert.ToDouble(fileValues[1]);
                    i++;
                }
                string format = "{0:0.000}";
                zipNodeValue = Nodes.Zip(Values, (node, value) => $"Coordinate: {String.Format(format, node)}\tValue: {String.Format(format, value)}");
            }
            catch (Exception ex)
            {
                throw new Exception("Неправильный формат файла!");
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
            }
        }

        public void Save(string filename)
        {
            StreamWriter? writer = null;
            try
            {
                writer = new StreamWriter(filename, false);
                string fRawEnumStr = fRaw.Method.Name;
                writer.WriteLine(LeftBound.ToString() + ' ' + RightBound.ToString() + ' ' + NumOfNodes.ToString() + ' ' + IsUniformGrid.ToString() + ' ' + fRawEnumStr);
                for (int i = 0; i < NumOfNodes; i++)
                {
                    writer.WriteLine(Nodes[i].ToString() + ' ' + Values[i].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Неправильный формат файла!");
            }
            finally
            {
                if (writer != null)
                    writer.Dispose();
            }
        }

        public static void Load(string filename, out RawData? rawData)
        {
            rawData = new RawData(filename);
        }
        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < NumOfNodes; i++)
            {
                result += $"Coordinate: {Nodes[i]}\tValue: {Values[i]}";
            }
            return result;
        }
        public IEnumerator<string> GetEnumerator()
        {
            foreach (var item in zipNodeValue)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)zipNodeValue).GetEnumerator();
        }
    }

    
}
