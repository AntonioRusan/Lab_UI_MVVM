using ClassLibraryModel;
using Moq;
using System.Windows.Controls;
using UIView;
using ViewModel;
using Xunit;

namespace ViewModelTests
{
    public class MainViewModelTests
    {
        private string testFilesDirectory = @"..\\..\\..\\..\\test_files\\";
        //Load from invalid file
        [Fact]
        public void LoadFromFileError()
        {
            var errorSender = new Mock<IErrorSender>();
            var fileDialog = new Mock<IFileDialog>();
            fileDialog.Setup(m => m.OpenFileDialog()).Returns($"{testFilesDirectory}incorrect_raw_data.txt");
            var testViewModel = new MainViewModel(errorSender.Object, fileDialog.Object);
            Assert.True(testViewModel.LoadFromFileCommand.CanExecute(null));
            testViewModel.LoadFromFileCommand.Execute(null);
            errorSender.Verify(r => r.SendError(It.IsAny<string>()), Times.Once);
        }
        //Load from correct file
        [Fact]
        public void LoadFromFileCorrect()
        {
            var errorSender = new Mock<IErrorSender>();
            var fileDialog = new Mock<IFileDialog>();
            fileDialog.Setup(m => m.OpenFileDialog()).Returns($"{testFilesDirectory}correct_uniform_raw_data.txt");
            var testViewModel = new MainViewModel(errorSender.Object, fileDialog.Object);

            Assert.True(testViewModel.LoadFromFileCommand.CanExecute(null));
            testViewModel.LoadFromFileCommand.Execute(null);
            errorSender.Verify(r => r.SendError(It.IsAny<string>()), Times.Never);
            Assert.Equal("12,5", testViewModel.splineIntegral);
            Assert.NotNull(testViewModel.splineData);
            Assert.NotNull(testViewModel.splinePlot);
        }
        //Load from correct controls
        [Fact]
        public void LoadFromControlsCorrect()
        {
            var errorSender = new Mock<IErrorSender>();
            var fileDialog = new Mock<IFileDialog>();
            var testViewModel = new MainViewModel(errorSender.Object, fileDialog.Object);
            testViewModel.leftBound = -1.5;
            testViewModel.rightBound = 0.5;
            testViewModel.rawNumOfNodes = 2;
            testViewModel.isUniformGrid = true;
            testViewModel.fRawEnum = ViewModel.FRawEnum.LinearFunc;
            testViewModel.SplineNumOfNodes = 5;
            testViewModel.LeftFirstDerivative = 1;
            testViewModel.RightFirstDerivative = 1;
            Assert.True(testViewModel.LoadFromControlsCommand.CanExecute(null));
            testViewModel.LoadFromControlsCommand.Execute(null);
            errorSender.Verify(r => r.SendError(It.IsAny<string>()), Times.Never);
            Assert.Equal("-1", testViewModel.splineIntegral);
            Assert.NotNull(testViewModel.splineData);
            Assert.NotNull(testViewModel.splinePlot);
        }
        //Load from invalid controls
        [Fact]
        public void LoadFromControlsError()
        {
            var errorSender = new Mock<IErrorSender>();
            var fileDialog = new Mock<IFileDialog>();
            var testViewModel = new MainViewModel(errorSender.Object, fileDialog.Object);
            testViewModel.leftBound = 5;
            testViewModel.rightBound = 0;
            testViewModel.rawNumOfNodes = 1;
            testViewModel.isUniformGrid = true;
            testViewModel.fRawEnum = ViewModel.FRawEnum.LinearFunc;
            testViewModel.SplineNumOfNodes = 5;
            testViewModel.LeftFirstDerivative = 1;
            testViewModel.RightFirstDerivative = 1;
            Assert.False(testViewModel.LoadFromControlsCommand.CanExecute(null));
        }
        //Save RawData to file and the Load from that file
        [Fact]
        public void SaveFileCommandCorrect()
        {
            var errorSender = new Mock<IErrorSender>();
            var fileDialog = new Mock<IFileDialog>();
            fileDialog.Setup(m => m.OpenFileDialog()).Returns($"{testFilesDirectory}test_save_raw_data.txt");
            fileDialog.Setup(m => m.SaveFileDialog()).Returns($"{testFilesDirectory}test_save_raw_data.txt");
            var testViewModel = new MainViewModel(errorSender.Object, fileDialog.Object);
            testViewModel.leftBound = -1.5;
            testViewModel.rightBound = 0.5;
            testViewModel.rawNumOfNodes = 5;
            testViewModel.isUniformGrid = true;
            testViewModel.fRawEnum = ViewModel.FRawEnum.LinearFunc;
            testViewModel.SplineNumOfNodes = 10;
            testViewModel.LeftFirstDerivative = 1;
            testViewModel.RightFirstDerivative = 1;

            Assert.True(testViewModel.SaveFileCommand.CanExecute(null));
            testViewModel.SaveFileCommand.Execute(null);

            Assert.True(testViewModel.LoadFromFileCommand.CanExecute(null));
            testViewModel.LoadFromFileCommand.Execute(null);
            Assert.Equal("-1", testViewModel.splineIntegral);
            errorSender.Verify(r => r.SendError(It.IsAny<string>()), Times.Never);
            Assert.NotNull(testViewModel.splineData);
            Assert.NotNull(testViewModel.splinePlot);
        }
        [Fact]
        public void SaveFileCommandIncorrect()
        {
            var errorSender = new Mock<IErrorSender>();
            var fileDialog = new Mock<IFileDialog>();
            fileDialog.Setup(m => m.OpenFileDialog()).Returns($"{testFilesDirectory}test_incorrect_save_raw_data.txt");
            var testViewModel = new MainViewModel(errorSender.Object, fileDialog.Object);
            testViewModel.leftBound = 10;
            testViewModel.rightBound = 0.5;
            testViewModel.rawNumOfNodes = -100;
            testViewModel.isUniformGrid = true;
            testViewModel.fRawEnum = ViewModel.FRawEnum.LinearFunc;
            testViewModel.SplineNumOfNodes = 10;
            testViewModel.LeftFirstDerivative = 1;
            testViewModel.RightFirstDerivative = 1;

            Assert.False(testViewModel.SaveFileCommand.CanExecute(null));
            
        }
    }
}