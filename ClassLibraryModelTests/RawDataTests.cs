using ClassLibraryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ClassLibraryModelTests
{
    public class RawDataTests
    {
        private string testFilesDirectory = @"..\\..\\..\\..\\test_files\\";
        [Fact]
        public void CreateUniformRawDataTest()
        {
            RawData uniformRawData = new RawData(0, 10, 6, true, CreationFunctions.ThreePolynomFunc);
            double[] unGridNodes = new double[6] { 0, 2, 4, 6, 8, 10 };
            double[] unGridValues = new double[6] { 0, 8, 64, 216, 512, 1000 };
            Assert.Equal(0, uniformRawData.LeftBound, 4);
            Assert.Equal(10, uniformRawData.RightBound, 4);
            Assert.Equal(6, uniformRawData.NumOfNodes);
            Assert.True(uniformRawData.IsUniformGrid);
            Assert.Equal(unGridNodes, uniformRawData.Nodes);
            Assert.Equal(unGridValues, uniformRawData.Values);
        }
        [Fact]
        public void CreateNonUniformRawDataTest()
        {
            RawData uniformRawData = new RawData(0, 10, 6, false, CreationFunctions.ThreePolynomFunc);
            Assert.Equal(0, uniformRawData.LeftBound, 4);
            Assert.Equal(10, uniformRawData.RightBound, 4);
            Assert.Equal(6, uniformRawData.NumOfNodes);
            Assert.False(uniformRawData.IsUniformGrid);
            Assert.True(uniformRawData.Nodes.All(a => a >= 0 && a <= 10));
            Assert.True(uniformRawData.Values.All(a => a >= 0 && a <= 1000));
        }
        [Fact]
        public void LoadRawDataFromCorrectFileTest()
        {
            string filename = testFilesDirectory + "correct_raw_data.txt";
            RawData uniformRawData = new RawData(filename);
            Assert.Equal(0, uniformRawData.LeftBound, 4);
            Assert.Equal(5, uniformRawData.RightBound, 4);
            Assert.Equal(5, uniformRawData.NumOfNodes);
            Assert.False(uniformRawData.IsUniformGrid);
            Assert.True(uniformRawData.Nodes.All(a => a >= 0 && a <= 5));
            Assert.True(uniformRawData.Values.All(a => a >= 0 && a <= 5));
        }
        [Fact]
        public void LoadRawDataFromInCorrectFileTest()
        {
            string filename = testFilesDirectory + "incorrect_raw_data.txt";
            var ex = Assert.Throws<Exception>(() => new RawData(filename));
            Assert.Equal("Неправильный формат файла!", ex.Message);
        }
    }
}
