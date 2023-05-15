using ClassLibraryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ClassLibraryModelTests
{
    public class SplineDataTests
    {
        [Fact]
        public void CreateSplineDataFromRawData()
        {
            RawData uniformRawData = new RawData(0, 10, 6, true, CreationFunctions.ThreePolynomFunc);
            SplineData testSpline = new SplineData(uniformRawData, 0, 300, 15);
            Assert.Equal(0, testSpline.LeftDerivative, 4);
            Assert.Equal(300, testSpline.RightDerivative, 4);
            Assert.Equal(15, testSpline.NumOfNodes);
            Assert.Equal(uniformRawData, testSpline.InputRawData);
            Assert.True(!testSpline.SplineItemList.Any());
        }

        [Theory]
        [InlineData(0, 5, 5, true, FRawEnum.ThreePolynomFunc, 0, 75, 10, 156.25)]
        [InlineData(-5, 10, 10, true, FRawEnum.ThreePolynomFunc, 75, 300, 25, 2343.75)]
        [InlineData(-1.5, 0.5, 2, false, FRawEnum.LinearFunc, 1, 1, 5, -1)]
        public void CalculateSplineData(double leftBound, double rightBound, int rawNumOfNodes, bool isUniform, FRawEnum funcEnum, double leftDer, double rihtDer, int numOfNodes, double resultIntegral)
        {
            RawData testRawData = new RawData(leftBound, rightBound, rawNumOfNodes, isUniform, enumToFunc(funcEnum));
            SplineData testSpline = new SplineData(testRawData, leftDer, rihtDer, numOfNodes);
            int computeResult = testSpline.CreateSpline();

            Assert.Equal(0, computeResult);
            Assert.Equal(numOfNodes, testSpline.SplineItemList.Count); 
            Assert.Equal(leftDer, testSpline.SplineItemList[0].FirstDerivative, 4);
            Assert.Equal(rihtDer, testSpline.SplineItemList[numOfNodes-1].FirstDerivative, 4);
            Assert.Equal(resultIntegral, testSpline.Integral, 4);
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
    }
}
