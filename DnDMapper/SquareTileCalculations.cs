using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDMapper
{
    class SquareTileCalculations
    {

        public SquareTileCalculations()
        {

        }//SquareTileCalculations()

        public Point[,] calculateSquTilePoints(Point[] cnrPnt, int height, int width)
        {
            Point[,] pointMatrix = new Point[width, height];
            calculateTileCoords(cnrPnt, width, height, ref pointMatrix);
            return pointMatrix;
        }//calculateSquTilePoints()

        private void calculateTileCoords(Point[] cnrPnt, int width, int height, ref Point[,] pointMatrix)
        {
            for (int y=0; y < height; y++)
            {
                double percentY = calcPercentage(height, y);
                for(int x = 0; x < width; x++)
                {
                    double percentX = calcPercentage(width, x);
                    int xCo = calcXCoord(cnrPnt, percentX, percentY);
                    int yCo = calcYCoord(cnrPnt, percentX, percentY);
                    Point p = new Point(xCo, yCo);
                    pointMatrix[x, y] = p;
                }//for - X loop
            }//for - Y loop
        }//calculateTileCoords()

    }
}
