using ClassLibraryModel;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using UIView;

namespace ViewModel
{
    public class MainViewModel: BaseViewModel, IDataErrorInfo
    {
        private readonly IErrorSender errorSender;
        private readonly IFileDialog fileDialog;
        public double leftBound { get; set; } = 0;
        public double rightBound { get; set; } = 1;
        public int rawNumOfNodes { get; set; } = 2;
        public bool isUniformGrid { get; set; } = true;
        public FRawEnum fRawEnum { get; set; } = FRawEnum.LinearFunc;
        public int SplineNumOfNodes { get; set; } = 2;
        public double LeftFirstDerivative { get; set; }
        public double RightFirstDerivative { get; set; }
        public RawData? rawData { get; set; }
        public SplineData? splineData { get; set; }
        public PlotModel? splinePlot { get; set; }
        public string splineIntegral { get; set; } = "";
        public MainViewModel(IErrorSender errorSender, IFileDialog fileDialog)
        {
            this.errorSender = errorSender;
            this.fileDialog = fileDialog;
            SaveFileCommand = new RelayCommand(o => { SaveFileCommandHandler(); }, o => CanSaveFileCommandHandler());
            LoadFromControlsCommand = new RelayCommand(o => { LoadFromControlsCommandHandler(); }, o => CanLoadFromControlsCommandHandler());
            LoadFromFileCommand = new RelayCommand(o => { LoadFromFileCommandHandler(); }, o => CanLoadFromFileCommandHandler());
            ComputeSplineCommand = new RelayCommand(o => { ComputeSplineCommandHandler(); }, o => CanComputeSplineCommandHandler());
        }
        public ICommand SaveFileCommand { get; private set; }
        public ICommand LoadFromControlsCommand { get; private set; }
        public ICommand LoadFromFileCommand { get; private set; }
        public ICommand ComputeSplineCommand { get; private set; }
        private void SaveFileCommandHandler()
        {
            try
            {
                loadFromControls();
                string filename = fileDialog.SaveFileDialog();
                if (!string.IsNullOrEmpty(filename))
                {
                    if (rawData != null)
                        rawData.Save(filename);
                }
            }
            catch (Exception ex)
            {
                errorSender.SendError("Ошибка:" + ex.Message);
            }
        }
        private bool CanSaveFileCommandHandler()
        {
            return string.IsNullOrEmpty(this["leftBound"]) && string.IsNullOrEmpty(this["rightBound"]) && string.IsNullOrEmpty(this["rawNumOfNodes"]);
        }
        private void LoadFromControlsCommandHandler ()
        {
            try
            {
                loadFromControls();
                int result = computeSpline();
                splineIntegral = splineData.Integral.ToString();
                drawSpline();
                RaisePropertyChanged("rawData");
                RaisePropertyChanged("splineData");
                RaisePropertyChanged("splineIntegral");
                RaisePropertyChanged("splinePlot");
            }
            catch (Exception ex)
            {
                errorSender.SendError("Ошибка:" + ex.Message);
            }
        }
        private bool CanLoadFromControlsCommandHandler()
        {
            return string.IsNullOrEmpty(this["leftBound"]) && string.IsNullOrEmpty(this["rightBound"]) && string.IsNullOrEmpty(this["rawNumOfNodes"]) && string.IsNullOrEmpty(this["SplineNumOfNodes"]);
        }
        private void LoadFromFileCommandHandler()
        {
            try
            {
                string filename = fileDialog.OpenFileDialog();
                if (!string.IsNullOrEmpty(filename))
                {
                    loadFromFile(filename);
                    RaisePropertyChanged("leftBound");
                    RaisePropertyChanged("rightBound");
                    RaisePropertyChanged("rawNumOfNodes");
                    RaisePropertyChanged("isUniformGrid");
                    RaisePropertyChanged("fRawEnum");
                    RaisePropertyChanged("rawData");
                }
                if (ComputeSplineCommand.CanExecute(this))
                    ComputeSplineCommand.Execute(this);
            }
            catch (Exception ex)
            {
                errorSender.SendError("Ошибка:" + ex.Message);
            }
        }
        private bool CanLoadFromFileCommandHandler()
        {
            return string.IsNullOrEmpty(this["SplineNumOfNodes"]);
        }

        private void ComputeSplineCommandHandler()
        {
            int result = computeSpline();
            splineIntegral = splineData.Integral.ToString();
            drawSpline();
            RaisePropertyChanged("rawData");
            RaisePropertyChanged("splineData");
            RaisePropertyChanged("splineIntegral");
            RaisePropertyChanged("splinePlot");
        }
        private bool CanComputeSplineCommandHandler()
        {
            return (string.IsNullOrEmpty(this["leftBound"]) && string.IsNullOrEmpty(this["rightBound"]) && string.IsNullOrEmpty(this["rawNumOfNodes"]) && string.IsNullOrEmpty(this["SplineNumOfNodes"])) && (rawData != null);
        }
        private void drawSpline()
        {
            try
            {
                var oxyPlotMod = new OxyPlotModel(splineData);
                this.splinePlot = oxyPlotMod.plotModel;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка отрисовки сплайна\n" + ex.Message);
            }
        }
        private void loadFromControls()
        {
            try
            {
                FRaw fRaw = enumToFunc(fRawEnum);
                rawData = new RawData(leftBound, rightBound, rawNumOfNodes, isUniformGrid, fRaw);
            }
            catch (Exception)
            {
                throw new Exception("Неправильный формат ввода!");
            }
        }
        private void loadFromFile(string filename)
        {
            try
            {
                rawData = new RawData(filename);
                leftBound = rawData.LeftBound;
                rightBound = rawData.RightBound;
                rawNumOfNodes = rawData.NumOfNodes;
                isUniformGrid = rawData.IsUniformGrid;
                fRawEnum = funcToEnum(rawData.fRaw);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private int computeSpline()
        {
            try
            {
                splineData = new SplineData(rawData, LeftFirstDerivative, RightFirstDerivative, SplineNumOfNodes);
                return splineData.CreateSpline();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private FRaw enumToFunc(FRawEnum fRawEnum)
        {
            FRaw fRaw = fRawEnum switch
            {
                FRawEnum.LinearFunc => CreationFunctions.LinearFunc,
                FRawEnum.ThreePolynomFunc => CreationFunctions.ThreePolynomFunc,
                FRawEnum.RandomValueFunc => CreationFunctions.RandomValueFunc,
                _ => CreationFunctions.LinearFunc
            };
            return fRaw;
        }
        private FRawEnum funcToEnum(FRaw fRaw)
        {
            string frawName = fRaw.Method.Name;
            FRawEnum fRawEnum = frawName switch
            {
                "LinearFunc" => FRawEnum.LinearFunc,
                "ThreePolynomFunc" => FRawEnum.ThreePolynomFunc,
                "RandomValueFunc" => FRawEnum.RandomValueFunc,
                _ => FRawEnum.LinearFunc
            };
            return fRawEnum;
        }
        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case "rawNumOfNodes":
                        if (rawNumOfNodes < 2)
                        {
                            error = "Число узлов сплайна должно быть больше или равно 2!";
                        }
                        break;
                    case "SplineNumOfNodes":
                        if (SplineNumOfNodes < 2)
                        {
                            error = "Число узлов равномерной сетки для значений сплайна должно быть больше или равно 2!";
                        }
                        break;
                    case "leftBound":
                        if (leftBound > rightBound)
                        {
                            error = "Левый конец отрезка интерполяции должен быть меньше, чем правый!";
                        }
                        break;
                    case "rightBound":
                        if (leftBound > rightBound)
                        {
                            error = "Правый конец отрезка интерполяции должен быть меньше, чем левый!";
                        }
                        break;
                }
                return error;
            }
        }
        public string Error { get; }
    }
}
