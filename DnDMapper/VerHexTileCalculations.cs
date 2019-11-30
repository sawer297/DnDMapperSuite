using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDMapper
{
    class VerHexTileCalculations
    {
        private bool staggered = false, northS = false;

        public VerHexTileCalculations()
        {
            staggered = false;
            northS = false;
        }//end VerHexPntsCalculationS()

        public Point[,] calculateVerticalHexPoints(Point[] cnrPnt, ref int height, ref int width, int imgHeight, int imgWidth)
        {
            staggered = isStaggered(cnrPnt);
            if (staggered) { northS = isNorthStaggered(cnrPnt); }

            int[] intervals = preCalculation(cnrPnt, height, width, staggered, northS);
            bool[] expansion = new bool[8];

            if (northS)
            {
                //Console.WriteLine("north staggered");
                cnrPnt[0].X -= intervals[1];
                cnrPnt[0].Y += intervals[2];
                cnrPnt[1].X += intervals[1];
                cnrPnt[1].Y += intervals[2];
                width++;
                height--;
            }
            else if (staggered)
            {
                //Console.WriteLine("south staggered");
                cnrPnt[2].X -= intervals[1];
                cnrPnt[2].Y -= intervals[2];
                cnrPnt[3].X += intervals[1];
                cnrPnt[3].Y -= intervals[2];
                height--;
            }

            expansion = checkForExpansion(cnrPnt, intervals, imgHeight, imgWidth);

            if (expansion[0]) { height += 2; }
            else if (expansion[1]) { height++; }
            if (expansion[2]) { height += 2; }
            else if (expansion[3]) { height++; }
            if (expansion[4]) { width++; }
            if (expansion[6]) { width++; }
            if (!expansion[4] && !expansion[6] && expansion[5] && expansion[7]) { width++; }

            //Console.WriteLine("expansion checkpoint : ");
            //Console.Write("\t");
            //foreach(bool b in expansion) { Console.Write(" : " + b); }
            //Console.WriteLine();

            Point[,] pointMatrix = new Point[width, height];

            cnrPnt = calcNewCorner(cnrPnt, intervals[2], intervals[1], expansion);

            bool shift = isShifted(expansion);
            //Console.WriteLine("shift checkpoint : " + shift);

            bool offset = determineOffset(expansion);
            //Console.WriteLine("offset checkpoint : (t=even, f=odd) " + offset);

            calculateTileCoords(cnrPnt, width, height, shift, offset, ref pointMatrix);

            return pointMatrix;
        }//end calculateVerticalHexPoints()

        //////////////////////////////////////////////////////////////////////
        // PART 1

        private bool isStaggered(Point[] cnrPnt)
        {
            if ((cnrPnt[0].X > cnrPnt[2].X + 10) || (cnrPnt[0].X < cnrPnt[2].X - 10))
            {
                return true;
            }
            return false;

        }//end isStaggered()

        private bool isNorthStaggered(Point[] cnrPnt)
        {
            if (cnrPnt[1].X - cnrPnt[0].X < cnrPnt[3].X - cnrPnt[2].X)
            {
                return true;
            }
            return false;
        }//end isNorthStaggered()

        //////////////////////////////////////////////////////////////////////
        // PART 2

        private int[] preCalculation(Point[] cnrPnt, int height, int width, bool staggered, bool northStaggered)
        {
            int[] intervals = new int[4];
            double vstag = ((1 / (width - 1f)) * 1) / 2;
            Point p1, p2, p3;

            if (!staggered || !northStaggered)
            {
                p1 = new Point(
                    calcXCoord(cnrPnt, calcPercentage(width, 0), calcPercentage(height, 0)),
                    calcYCoord(cnrPnt, calcPercentage(width, 0), calcPercentage(height, 0))
                    );
                p2 = new Point(
                    calcXCoord(cnrPnt, calcPercentage(width, 1), calcPercentage(height, 0)),
                    calcYCoord(cnrPnt, calcPercentage(width, 1), calcPercentage(height, 0))
                    );
                p3 = new Point(
                    calcXCoord(cnrPnt, calcPercentage(width, 0) + vstag, calcPercentage(height, 1)),
                    calcYCoord(cnrPnt, calcPercentage(width, 0) + vstag, calcPercentage(height, 1))
                    );
            }
            else
            {
                p1 = new Point(
                    calcXCoord(cnrPnt, calcPercentage(width, 0) + vstag, calcPercentage(height, 0)),
                    calcYCoord(cnrPnt, calcPercentage(width, 0) + vstag, calcPercentage(height, 0))
                    );
                p2 = new Point(
                    calcXCoord(cnrPnt, calcPercentage(width, 1) + vstag, calcPercentage(height, 0)),
                    calcYCoord(cnrPnt, calcPercentage(width, 1) + vstag, calcPercentage(height, 0))
                    );
                p3 = new Point(
                    calcXCoord(cnrPnt, calcPercentage(width, 1), calcPercentage(height, 1)),
                    calcYCoord(cnrPnt, calcPercentage(width, 1), calcPercentage(height, 1))
                    );
            }

            intervals[0] = p2.X - p1.X;
            intervals[1] = intervals[0] / 2;

            intervals[2] = p3.Y - p1.Y;
            intervals[3] = intervals[2] / 2;

            return intervals;
        }//end preCalculation()

        //////////////////////////////////////////////////////////////////////
        // PART 3

        private bool[] checkForExpansion(Point[] cnrPnt, int[] intervals, int imgHeight, int imgWidth)
        {
            // expansion[0] = north2        intervals[0] = horInterval
            // expansion[1] = north         intervals[1] = horHalfInterval
            // expansion[2] = south2        intervals[2] = verInterval
            // expansion[3] = south         intervals[3] = verHalfInterval
            // expansion[4] = east2
            // expansion[5] = east
            // expansion[6] = west2
            // expansion[7] = west

            Point northwest = cnrPnt[0], southeast = cnrPnt[3];
            bool[] expansion = new bool[8];

            //northern edge
            if (northwest.Y - ((intervals[2] * 2) + intervals[3]) > 0 - 5)
            {
                expansion[0] = true;
                expansion[1] = true;
            }
            else if (northwest.Y - (intervals[2] + intervals[3]) > 0 - 5)
            {
                expansion[1] = true;
            }

            //southern edge
            if (southeast.Y + ((intervals[2] * 2) + intervals[3]) < imgHeight + 5)
            {
                expansion[2] = true;
                expansion[3] = true;
            }
            else if (southeast.Y + (intervals[2] + intervals[3]) < imgHeight + 5)
            {
                expansion[3] = true;
            }

            //eastern edge
            if (southeast.X + (intervals[0] + intervals[1]) < imgWidth + 5)
            {
                expansion[4] = true;
                expansion[5] = true;
            }
            else if (southeast.X + intervals[0] < imgWidth + 5)
            {
                expansion[5] = true;
            }

            //western edge
            if (northwest.X - (intervals[0] + intervals[1]) > 0 - 5)
            {
                expansion[6] = true;
                expansion[7] = true;
            }
            else if (northwest.X - intervals[0] > 0 - 5)
            {
                expansion[7] = true;
            }

            return expansion;
        }//end checkForExpansion()

        //////////////////////////////////////////////////////////////////////
        // PART 4

        private Point[] calcNewCorner(Point[] cnrPnt, int verInterval, int horHalfInterval, bool[] expansion)
        {
            // expansion[0] = north2        cnrPnt[0] = northwest
            // expansion[1] = north1        cnrPnt[1] = northeast
            // expansion[2] = south2        cnrPnt[2] = southwest
            // expansion[3] = south1        cnrPnt[3] = southeast
            // expansion[4] = east2
            // expansion[5] = east
            // expansion[6] = west2
            // expansion[7] = west

            //northern edge
            if (expansion[0])
            {
                cnrPnt[0].Y -= (verInterval * 2);
                cnrPnt[1].Y -= (verInterval * 2);
            }
            else if (expansion[1])
            {
                cnrPnt[0].Y -= verInterval;
                cnrPnt[1].Y -= verInterval;
            }

            //southern edge
            if (expansion[2])
            {
                cnrPnt[2].Y += (verInterval * 2);
                cnrPnt[3].Y += (verInterval * 2);
            }
            else if (expansion[3])
            {
                cnrPnt[2].Y += verInterval;
                cnrPnt[3].Y += verInterval;
            }

            //eastern edge
            if (expansion[5] && expansion[7] && !expansion[6])
            {
                cnrPnt[1].X += horHalfInterval;
                cnrPnt[3].X += horHalfInterval;
            }
            else if (expansion[4])
            {
                cnrPnt[1].X += (horHalfInterval * 2);
                cnrPnt[3].X += (horHalfInterval * 2);
            }

            //western edge
            if (expansion[7] && !expansion[6] && !expansion[5])
            {
                cnrPnt[0].X -= horHalfInterval;
                cnrPnt[1].X -= horHalfInterval;
                cnrPnt[2].X -= horHalfInterval;
                cnrPnt[3].X -= horHalfInterval;
            }
            else if (expansion[6])
            {
                cnrPnt[0].X -= (horHalfInterval * 2);
                cnrPnt[2].X -= (horHalfInterval * 2);
            }
            else if (expansion[7])
            {
                cnrPnt[0].X -= horHalfInterval;
                cnrPnt[2].X -= horHalfInterval;
            }

            return cnrPnt;
        }//end calcNewCorner()

        //////////////////////////////////////////////////////////////////////
        // PART 5

        private bool isShifted(bool[] expansion)
        {
            if ((expansion[4] && expansion[6])
                || (!expansion[4] && !expansion[6] && ((expansion[5] && expansion[7]) || (!expansion[5] && !expansion[7])))
                || (!expansion[5] && expansion[6])
                || (expansion[4] && !expansion[7]))
            {
                return false;
            }
            else
            {
                return true;
            }
        }//end isShifted()

        private bool determineOffset(bool[] expansion)
        {
            if ((!expansion[0] && expansion[1]) || (!expansion[6] && expansion[7]))
            {
                if (!expansion[0] && expansion[1] && !expansion[6] && expansion[7])
                {
                    return false;//odd
                }
                return true;//even
            }
            else
            {
                return false;//odd
            }
        }

        //////////////////////////////////////////////////////////////////////
        // PART 6

        private void calculateTileCoords(Point[] cnrPnt, int width, int height, bool shift, bool offset, ref Point[,] pointMatrix)
        {
            double vstag = ((1 / (width - 1f)) * 1) / 2;

            for (int y = 0; y < height; y++)
            {
                double percentY = calcPercentage(height, y);
                for (int x = 0; x < width; x++)
                {
                    int xCo = 0, yCo = 0;
                    double percentX = calcPercentage(width, x);
                    if ((y % 2 != 0 && !offset) || (y % 2 == 0 && offset))
                    {///if staggered row
                        if (x != width - 1 || shift)//
                        {///get every point except last in non-shifted staggered row 
                         /// or all points in shifted staggered roow 
                            xCo = calcXCoord(cnrPnt, percentX + vstag, percentY);
                            yCo = calcYCoord(cnrPnt, percentX + vstag, percentY);
                        }
                        else
                        {//if last point skip over
                            continue;
                        }
                    }
                    else
                    {///non-staggered row
                        xCo = calcXCoord(cnrPnt, percentX, percentY);
                        yCo = calcYCoord(cnrPnt, percentX, percentY);
                    }

                    Point p = new Point(xCo, yCo);
                    pointMatrix[x, y] = p;
                }
            }
        }//calculateTileCoords()

        private double calcPercentage(int numTile, int kurPnt)
        {
            return (1 / (numTile - 1f)) * kurPnt;
        }//calcpercentage()

        private int calcXCoord(Point[] cnrPnt, Double Px, Double Py)
        {
            double x = (cnrPnt[0].X + (cnrPnt[1].X - cnrPnt[0].X) * Px) * (1 - Py) + (cnrPnt[2].X + (cnrPnt[3].X - cnrPnt[2].X) * Px) * Py;
            //int r = Convert.ToInt32(Math.Ceiling(x));
            int r = Convert.ToInt32(x);
            //int r = Convert.ToInt32(Math.Floor(x));
            return r;
        }//calcXCoord()

        private int calcYCoord(Point[] cnrPnt, Double Px, Double Py)
        {
            double y = (cnrPnt[0].Y + (cnrPnt[2].Y - cnrPnt[0].Y) * Py) * (1 - Px) + (cnrPnt[0].Y + (cnrPnt[2].Y - cnrPnt[0].Y) * Py) * Px;
            // int r = Convert.ToInt32(Math.Ceiling(y));
            return Convert.ToInt32(y);
            //int r = Convert.ToInt32(Math.Floor(y));
        }//clacYCoord()

    }//class VerHexTileCalculations
}
