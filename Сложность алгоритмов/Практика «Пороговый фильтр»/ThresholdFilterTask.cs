using System;

namespace Recognizer
{
	public static class ThresholdFilterTask
    {
        public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
        {
            var lengthI = original.GetLength(0);
            var lengthJ = original.GetLength(1);
            var filterMass = new double[lengthI, lengthJ];

            var T = GetT(original, whitePixelsFraction);
            var oneDimMass = ConvertMassToOneDim(original);
            var borderValue = GetBorderValue(oneDimMass, T);
            
            for (var i = 0; i < lengthI; i++)
                for (var j = 0; j < lengthJ; j++)
                {
                    if ((original[i, j] >= borderValue&& whitePixelsFraction!=0)) filterMass[i, j] = 1.0;
                    else filterMass[i, j] = 0.0;
                }
            return filterMass;
        }
		
        public static double[] ConvertMassToOneDim(double[,] original)
        {
            var lengthI = original.GetLength(0);
            var lengthJ = original.GetLength(1);
            var oneDimArray = new double[lengthI * lengthJ];
            var count = 0;
            for (var i = 0; i < lengthI; i++)
                for (var j = 0; j < lengthJ; j++)
                {
                    oneDimArray[count] = original[i, j];
                    count++;
                }
            return oneDimArray;
        }

        public static double GetBorderValue(double[] convertedOneDimMass, int T)
        {            
            Array.Sort(convertedOneDimMass);
            Array.Reverse(convertedOneDimMass);
            if (T == 0) return double.MaxValue;
            return convertedOneDimMass[T-1];
        }

        public static int GetT(double[,] original, double whitePixelsFraction)
        {
            var lengthI = original.GetLength(0);
            var lengthJ = original.GetLength(1);
            var N = lengthI * lengthJ;
            return (int)(whitePixelsFraction * N);
        }
    }
}
