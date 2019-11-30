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

        private double calcPercentage(int numTile, int kurPnt)
        {
            return (1 / (numTile - 1f)) * kurPnt;
        }//calcPercentage()

        private int calcXCoord(Point[] cnrPnt, double Px, double Py)
        {
            double x = (cnrPnt[0].X + (cnrPnt[1].X - cnrPnt[0].X) * Px) * (1 - Py) + (cnrPnt[2].X + (cnrPnt[3].X - cnrPnt[2].X) * Px) * Py;
            int r = Convert.ToInt32(x);
            return r;
        }//calcXCoord()

        private int calcYCoord(Point[] cnrPnt, double Px, double Py)
        {
            double y = (cnrPnt[0].Y + (cnrPnt[2].Y - cnrPnt[0].Y) * Py) * (1 - Px) + (cnrPnt[1].Y + (cnrPnt[3].Y - cnrPnt[1].Y) * Py) * Px;
            int r = Convert.ToInt32(y);
            return r;
        }//calcYCoord()

        // for reference later, SBL 11.29.19
        // cnrPnt[0] = northwest
        // cnrPnt[1] = northeast
        // cnrPnt[2] = southwest
        // cnrPnt[3] = southeast

    }//class SquareTileCalculations
}
