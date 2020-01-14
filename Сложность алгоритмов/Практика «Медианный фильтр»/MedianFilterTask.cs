using System;
using System.Collections.Generic;

namespace Recognizer
{
	internal static class MedianFilterTask
	{
		/* 
		 * Для борьбы с пиксельным шумом, подобным тому, что на изображении,
		 * обычно применяют медианный фильтр, в котором цвет каждого пикселя, 
		 * заменяется на медиану всех цветов в некоторой окрестности пикселя.
		 * https://en.wikipedia.org/wiki/Median_filter
		 * 
		 * Используйте окно размером 3х3 для не граничных пикселей,
		 * Окно размером 2х2 для угловых и 3х2 или 2х3 для граничных.
		 */
		public static double[,] MedianFilter(double[,] original)
		{
            var lengthI = original.GetLength(0);
            var lengthJ = original.GetLength(1);
            var medianMass = new double[lengthI, lengthJ];
            for (var i = 0; i < lengthI; i++)
                for (var j = 0; j < lengthJ; j++)
                {
                    var mass = GetNeighborPix3x3Mass(original, i, j);
                    medianMass[i,j]=GetMediana(GetCleanMass(mass));
                }
            return medianMass;
		}

        public static double[,] GetNeighborPix3x3Mass(double[,] original, int currentI, int currentJ)
        {
            var lengthIDim = original.GetLength(0);
            var lengthJDim = original.GetLength(1);
            var mass = new double[3, 3];
            for (var i = 0; i < 3; i++)
                for (var j = 0; j < 3; j++)
                {
                    var indexI = i + currentI - 1;
                    var indexJ = j + currentJ - 1;
                    var outOfRange = (indexI < 0 || indexI >= lengthIDim || 
                                             indexJ < 0 || indexJ >= lengthJDim);
                    if (outOfRange) mass[i, j] = -1;
                    else mass[i, j] = original[indexI, indexJ];
                }
            return mass;
        }

        public static double[] GetCleanMass(double[,] mass)
        {
            var list = new List<double>();
            for (var i = 0; i < 3; i++)
                for (var j = 0; j < 3; j++)
                {
                    if (mass[i, j] != -1) list.Add(mass[i, j]);
                }
            return list.ToArray();
        } 

        public static double GetMediana(double[] mass)
        {
            Array.Sort(mass);
            if ((mass.Length % 2) == 1)
            {
                return mass[((mass.Length + 1) / 2) - 1];
            }
            else
            {
                return (mass[((mass.Length / 2) - 1)] + mass[(mass.Length / 2)]) / 2;
            }
        } 
    }
}
