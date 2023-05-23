#include "pch.h"
#include "mkl.h"

extern "C"  _declspec(dllexport)
void CubicSplineInterpoalte(MKL_INT nx, double* x, MKL_INT ny, double* y, bool isUniform, double* bc, double* scoeff, MKL_INT nsite, double* site,
	MKL_INT ndorder, MKL_INT * dorder, double* resultDeriv, MKL_INT nlim, double* llim, double* rlim, double* resultInteg, int& ret)
{
	DFTaskPtr task;
	int dfStatus;
	try
	{
		dfStatus = dfdNewTask1D(&task, nx, x, isUniform ? DF_UNIFORM_PARTITION : DF_NON_UNIFORM_PARTITION, ny, y, DF_MATRIX_STORAGE_ROWS);
		if (dfStatus != DF_STATUS_OK)
		{
			ret = 1;
			return;
		}
		dfStatus = dfdEditPPSpline1D(task, DF_PP_CUBIC, DF_PP_NATURAL, DF_BC_1ST_LEFT_DER | DF_BC_1ST_RIGHT_DER, bc, DF_NO_IC, NULL, scoeff, DF_NO_HINT);
		if (dfStatus != DF_STATUS_OK)
		{
			ret = 2;
			return;
		}
		dfStatus = dfdConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD);
		if (dfStatus != DF_STATUS_OK)
		{
			ret = 3;
			return;
		}
		dfStatus = dfdInterpolate1D(task, DF_INTERP, DF_METHOD_PP, nsite, site, DF_UNIFORM_PARTITION, ndorder, dorder, NULL, resultDeriv, DF_MATRIX_STORAGE_ROWS, NULL);
		if (dfStatus != DF_STATUS_OK)
		{
			ret = 4;
			return;
		}
		dfStatus = dfdIntegrate1D(task, DF_METHOD_PP, nlim, llim, DF_UNIFORM_PARTITION, rlim, DF_UNIFORM_PARTITION, NULL, NULL, resultInteg, DF_MATRIX_STORAGE_ROWS);
		if (dfStatus != DF_STATUS_OK)
		{
			ret = 5;
			return;
		}
		dfStatus = dfDeleteTask(&task);
		if (dfStatus != DF_STATUS_OK)
		{
			ret = 6;
			return;
		}
		ret = 0;
	}
	catch (...)
	{
		ret = 7;
	}
}