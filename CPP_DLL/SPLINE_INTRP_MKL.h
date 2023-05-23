#pragma once
#include "mkl.h"

extern "C"  _declspec(dllexport)
void CubicSplineInterpoalte(MKL_INT nx, double* x, MKL_INT ny, double* y, bool isUniform, double* bc, double* scoeff, MKL_INT nsite, double* site,
	MKL_INT ndorder, MKL_INT * dorder, double* resultDeriv, MKL_INT nlim, double* llim, double* rlim, double* resultInteg, int& ret);