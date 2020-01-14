using System;

namespace Recognizer
{
    internal static class SobelFilterTask
    {
        public static double[,] SobelFilter(double[,] g, double[,] sx)
        {
            var width = g.GetLength(0);
            var height = g.GetLength(1);
            var result = new double[width, height];

            var n = sx.GetLength(0);
            var k = (int) n / 2;

            var sy = GetTransMatrix(sx);

            for (int x = k; x < width - k; x++)
                for (int y = k; y < height - k; y++)
                {
                    var gx = GetG(g,sx,x,y,k);
                    var gy = GetG(g, sy, x, y,k);
                    result[x, y] = Math.Sqrt(gx * gx + gy * gy);
                }    
            return result;
        }

        public static double[,] GetTransMatrix(double[,] matrix)
        {
            var width = matrix.GetLength(0);
            var height = matrix.GetLength(1);
            var transMatrix = new double[width, height];
            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                {
                    transMatrix[i, j] = matrix[j, i];
                }
            return transMatrix;
        }

        public static double[,] GetNeighborMatrix(int k,double[,] g, int x, int y)
        {            
            var neighborhoodMatrix = new double[k * 2 + 1, k * 2 + 1]; 
            for (var i = 0; i < k*2+1; i++)
                for (var j = 0; j < k * 2 + 1; j++)
                {
                    neighborhoodMatrix[i, j] = g[x + i - k, y + j - k];
                }
            return neighborhoodMatrix;
        }

        public static double[,] GetMultiplydMatrix(double[,] m1, double[,] m2)
        {
            var width = m1.GetLength(0);
            var height = m1.GetLength(1);
            var multiplyMatrix = new double[width, height];
            
            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                {
                    multiplyMatrix[i, j] = m1[i, j] * m2[i, j];
                }
            return multiplyMatrix;
        }

        public static double GetSumMatrix(double[,] m)
        {
            var width = m.GetLength(0);
            var height = m.GetLength(1);
            var sum = 0.0;
            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                {
                    sum = sum + m[i, j];
                }
            return sum;
        }

        public static double GetG(double[,] g, double[,] s,int x, int y,int k)
        {            
            var neighborMatrix = GetNeighborMatrix(k, g, x, y);
            var multiplydMatrix = GetMultiplydMatrix(neighborMatrix,s);
            return GetSumMatrix(multiplydMatrix);
        }
    }
}
