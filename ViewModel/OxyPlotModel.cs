using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Legends;
using ClassLibraryModel;

namespace ViewModel
{
    public class OxyPlotModel
    {
        private SplineData splineData;
        public PlotModel plotModel { get; private set; }

        public OxyPlotModel(SplineData splineData)
        {
            this.splineData = splineData;
            this.plotModel = new PlotModel { Title = "Результат сплайн-интерполяции" };
            this.DrawSpline();
        }

        public void DrawSpline()
        {
            if (splineData != null)
            {
                this.plotModel.Series.Clear();
                OxyColor color = OxyColors.Red;
                LineSeries lineSeries = new LineSeries();
                for (int i = 0; i < splineData.InputRawData.NumOfNodes; i++)
                {
                    lineSeries.Points.Add(new DataPoint(splineData.InputRawData.Nodes[i], splineData.InputRawData.Values[i]));
                }

                lineSeries.Title = "Изначальные данные";
                lineSeries.Color = color;
                lineSeries.LineStyle = LineStyle.None;
                lineSeries.MarkerType = MarkerType.Circle;
                lineSeries.MarkerSize = 4;
                lineSeries.MarkerStroke = color;
                lineSeries.MarkerFill = color;

                Legend leg = new Legend();
                this.plotModel.Legends.Add(leg);
                this.plotModel.Series.Add(lineSeries);

                color = OxyColors.DeepSkyBlue;
                lineSeries = new LineSeries();
                for (int i = 0; i < splineData.NumOfNodes; i++)
                {
                    lineSeries.Points.Add(new DataPoint(splineData.SplineItemList[i].Coordinate, splineData.SplineItemList[i].SplineValue));
                }

                lineSeries.Title = "Сплайн";
                lineSeries.Color = color;
                lineSeries.MarkerSize = 4;

                this.plotModel.Series.Add(lineSeries);
            }
        }
    } 
}
