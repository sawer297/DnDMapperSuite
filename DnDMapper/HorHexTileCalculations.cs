using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDMapper
{
    class HorHexTileCalculations
    {
        private bool staggered = false, eastS = false;

        public HorHexTileCalculations()
        {
            staggered = false;
            eastS = false;
        }

        public Point[,] calculateHorizontalHexPoints(Point[] cnrPnt, ref int height, ref int width, int imgHeight, int imgWidth)
        {
            Point[,] pointMatrix;

            //check if staggered and whether it is east or west staggered
            staggered = isStaggered(cnrPnt);
            if (staggered)
            {
                eastS = isEastStaggered(cnrPnt);
            }
            //Console.WriteLine("staggered checkpoint : "+staggered +" : "+eastS);

            //run precalculation and get hor and ver intervals
            int[] intervals = preCalculation(cnrPnt, height, width, staggered, eastS);
            bool[] expansive = new bool[8];
            //Console.Write("pre-claculate checkpoint : ");
            //foreach(int i in intervals) { Console.Write("\t" + i + ", "); }
            //Console.WriteLine();

            // cnrPnt[0] = northwest        intervals[0] = horInterval
            // cnrPnt[1] = northeast        intervals[1] = horHalfInterval
            // cnrPnt[2] = southwest        intervals[2] = verInterval
            // cnrPnt[3] = southeast        intervals[3] = verHalfInterval

            if (eastS)
            {
                cnrPnt[1].X -= intervals[0];
                cnrPnt[1].Y -= intervals[3];
                cnrPnt[3].X -= intervals[0];
                cnrPnt[3].Y += intervals[3];
                width--;
            }
            else if (staggered)
            {
                cnrPnt[0].X += intervals[0];
                cnrPnt[0].Y -= intervals[3];
                cnrPnt[2].X += intervals[0];
                cnrPnt[2].Y += intervals[3];
                width--;
                height++;
            }

            //check edges for extra hex tiles
            expansive = checkForExpansion(cnrPnt, intervals, imgHeight, imgWidth);

            //Console.WriteLine("edges checkpoint : ");
            //foreach (bool b in expansive) { Console.Write("\t" + b + ", "); }
            //Console.WriteLine();

            //if found edges, add to height and width
            if (expansive[0]) { height++; }
            if (expansive[2]) { height++; }
            if (!expansive[0] && !expansive[2] && expansive[1] && expansive[3]) { height++; }
            if (expansive[4]) { width += 2; }
            else if (expansive[5]) { width++; }
            if (expansive[6]) { width += 2; }
            else if (expansive[7]) { width++; }
            //Console.WriteLine("h&w checkpoint > new h : " + height + ", new w : " + width);

            pointMatrix = new Point[width, height];

            //calculate new corners
            cnrPnt = calcNewCorner(cnrPnt, intervals[3], intervals[0], expansive);

            //Console.WriteLine("new corners checkpoint");

            //check for shift
            bool shift = isShifted(expansive);
            //Console.WriteLine("shift checkpoint : " + shift);
            //claculate offset
            bool offset = determineOffset(expansive);
            //Console.WriteLine("offset checkpoint > (t=even, f=odd) : " + offset);

            calculateTileCoords(cnrPnt, width, height, shift, offset, ref pointMatrix);
            //Console.WriteLine("final checkpoint");

            return pointMatrix;
        }//end calculateHorizontalHexPoints()

        //////////////////////////////////////////////////////////////////////
        // PART 1

        private bool isStaggered(Point[] cnrPnt)
        {
            if ((cnrPnt[0].Y > (cnrPnt[1].Y + 10)) || (cnrPnt[0].Y < (cnrPnt[1].Y - 10)))
            {
                return true;
            }
            return false;
        }//end isStaggered()

        private bool isEastStaggered(Point[] cnrPnt)
        {
            if (cnrPnt[2].Y - cnrPnt[0].Y > cnrPnt[3].Y - cnrPnt[1].Y)
            {
                return true;
            }
            return false;
        }//end isEastStaggered()

        //////////////////////////////////////////////////////////////////////
        // PART 2

        private int[] preCalculation(Point[] cnrPnt, int height, int width, bool staggered, bool easts)
        {
            //Console.Write("\t\t");
            //foreach (Point p in cnrPnt) { Console.Write("(" + p.X + "," + p.Y + "), "); }
            //Console.WriteLine();
            //Console.WriteLine("\t\tH : " + height + ", W : " + width);
            int[] intervals = new int[4];
            double hstag = ((1 / (height - 1f)) * 1) / 2;
            Point p1, p2, p3;

            if (!staggered || easts)
            {
                //Console.WriteLine("\t\tnon-staggered or east-staggered");
                p1 = new Point(
                    calcXCoord(cnrPnt, calcPercentage(width, 0), calcPercentage(height, 0)),
                    calcYCoord(cnrPnt, calcPercentage(width, 0), calcPercentage(height, 0))
                    );
                p2 = new Point(
                    calcXCoord(cnrPnt, calcPercentage(width, 1), calcPercentage(height, 0) + hstag),
                    calcYCoord(cnrPnt, calcPercentage(width, 1), calcPercentage(height, 0) + hstag)
                    );
                p3 = new Point(
                    calcXCoord(cnrPnt, calcPercentage(width, 0), calcPercentage(height, 1)),
                    calcYCoord(cnrPnt, calcPercentage(width, 0), calcPercentage(height, 1))
                    );
            }
            else
            {
                //Console.WriteLine("\t\twest-staggered");
                p1 = new Point(
                    calcXCoord(cnrPnt, calcPercentage(width, 1), calcPercentage(height, 0)),
                    calcYCoord(cnrPnt, calcPercentage(width, 1), calcPercentage(height, 0))
                    );
                p2 = new Point(
                    calcXCoord(cnrPnt, calcPercentage(width, 2), calcPercentage(height, 0) + hstag),
                    calcYCoord(cnrPnt, calcPercentage(width, 2), calcPercentage(height, 0) + hstag)
                    );
                p3 = new Point(
                    calcXCoord(cnrPnt, calcPercentage(width, 1), calcPercentage(height, 1)),
                    calcYCoord(cnrPnt, calcPercentage(width, 1), calcPercentage(height, 1))
                    );
            }

            //Console.WriteLine("\t\tP1(" + p1.X + "," + p1.Y + ")");
            //Console.WriteLine("\t\tP2(" + p2.X + "," + p2.Y + ")");
            //Console.WriteLine("\t\tP3(" + p3.X + "," + p3.Y + ")");
            intervals[0] = p2.X - p1.X; //horInterval
            intervals[1] = intervals[0] / 2; //horHalfInterval

            intervals[2] = p3.Y - p1.Y; //verInterval
            intervals[3] = intervals[2] / 2; //verHalfInterval

            return intervals;

        }//end preCalculation()

        //////////////////////////////////////////////////////////////////////
        // PART 3

        private bool[] checkForExpansion(Point[] cnrPnt, int[] intervals, int imgHeight, int imgWide)
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
            //Console.WriteLine("imgH : " + imgHeight + ", imgW : " + imgWide);

            //northern edge
            //Console.WriteLine("\t\tnorth2 > nw.y : "+ northwest.Y+", vi : "+verInterval+", vhi : "+verHalfInterval+", = : "+(northwest.Y - (verInterval+verHalfInterval)));
            //Console.WriteLine("\t\tnorth1 > nw.y : " + northwest.Y + ", vi : " + verInterval + ", = : " + (northwest.Y - verInterval));
            if (northwest.Y - (intervals[2] + intervals[3]) > 0 - 5)
            {
                expansion[0] = true;
                expansion[1] = true;
            }
            else if (northwest.Y - intervals[2] > 0 - 5)
            {
                expansion[1] = true;
            }

            //southern edge
            //Console.WriteLine("\t\tsouth2 > se.y : " + southeast.Y + ", vi : " + verInterval + ", vhi : " + verHalfInterval + ", = : " + (southeast.Y + (verInterval + verHalfInterval)));
            //Console.WriteLine("\t\tsouth1 > se.y : " + southeast.Y + ", vi : " + verInterval + ", = : " + (southeast.Y + verInterval));
            if (southeast.Y + (intervals[2] + intervals[3]) < imgHeight + 5)
            {
                expansion[2] = true;
                expansion[3] = true;
            }
            else if (southeast.Y + intervals[2] < imgHeight + 5)
            {
                expansion[3] = true;
            }

            //eastern edge
            //Console.WriteLine("\t\teast > se.x : " + southeast.X + ", hi : " + horInterval + ", = : " + (southeast.X + horInterval));
            if (southeast.X + ((intervals[0] * 2) + intervals[1]) < imgWide + 5)
            {
                expansion[4] = true;
                expansion[5] = true;
            }
            else if (southeast.X + (intervals[0] + intervals[1]) < imgWide + 5)
            {
                expansion[5] = true;
            }

            //western edge
            //Console.WriteLine("\t\twest > nw.x : " + northwest.X + ", hi : " + horInterval + ", = : " + (northwest.X - horInterval));
            if (northwest.X - ((intervals[0] * 2) + intervals[1]) > 0 - 5)
            {
                expansion[6] = true;
                expansion[7] = true;
            }
            if (northwest.X - (intervals[0] + intervals[1]) > 0 - 5)
            {

                expansion[7] = true;
            }

            return expansion;
        }//end nonStaggeredPreCalc()

        //////////////////////////////////////////////////////////////////////
        // PART 4

        private Point[] calcNewCorner(Point[] cnrPnt, int verHalfInterval, int horInterval, bool[] expansion)
        {
            //Console.Write("\t");
            //foreach (Point pnt in cnrPnt) { Console.Write("(" + pnt.X + "," + pnt.Y + "), "); }
            //Console.WriteLine();

            // expansion[0] = north2        cnrPnt[0] = northwest
            // expansion[1] = north1        cnrPnt[1] = northeast
            // expansion[2] = south2        cnrPnt[2] = southwest
            // expansion[3] = south1        cnrPnt[3] = southeast
            // expansion[4] = east2
            // expansion[5] = east
            // expansion[6] = west2
            // expansion[7] = west

            ///////////////////////////////////////////////////////////
            // NORTHERN EDGE
            if (expansion[1] && !expansion[0] && !expansion[3])
            {
                cnrPnt[0].Y -= verHalfInterval;
                cnrPnt[1].Y -= verHalfInterval;
                cnrPnt[2].Y -= verHalfInterval;
                cnrPnt[3].Y -= verHalfInterval;
            }
            else if (expansion[0])
            {
                cnrPnt[0].Y -= (verHalfInterval * 2);
                cnrPnt[1].Y -= (verHalfInterval * 2);
            }
            else if (expansion[1])
            {
                cnrPnt[0].Y -= verHalfInterval;
                cnrPnt[1].Y -= verHalfInterval;
            }

            ///////////////////////////////////////////////////////////
            // SOUTHERN EDGE
            if (expansion[3] && expansion[1] && !expansion[0])
            {
                cnrPnt[2].Y += verHalfInterval;
                cnrPnt[3].Y += verHalfInterval;
            }
            else if (expansion[2])
            {
                cnrPnt[2].Y += (verHalfInterval * 2);
                cnrPnt[3].Y += (verHalfInterval * 2);
            }

            ///////////////////////////////////////////////////////////
            // EASTERN EDGE
            if (expansion[4])
            {
                cnrPnt[1].X += (horInterval * 2);
                cnrPnt[3].X += (horInterval * 2);
            }
            else if (expansion[5])
            {
                cnrPnt[1].X += horInterval;
                cnrPnt[3].X += horInterval;
            }

            ///////////////////////////////////////////////////////////
            // WESTERN EDGE
            if (expansion[6])
            {
                cnrPnt[0].X -= (horInterval * 2);
                cnrPnt[2].X -= (horInterval * 2);
            }
            else if (expansion[7])
            {
                cnrPnt[0].X -= horInterval;
                cnrPnt[2].X -= horInterval;
            }

            //Console.Write("\t");
            //foreach (Point pnt in cnrPnt) { Console.Write("(" + pnt.X + "," + pnt.Y + "), "); }
            //Console.WriteLine();

            return cnrPnt;
        }//end nonStaggeredCalcCorner()

        //////////////////////////////////////////////////////////////////////
        // PART 5

        private bool isShifted(bool[] expansion)
        {//checks if hex map is shifted
            if ((expansion[0] && expansion[2])
                || ((!expansion[0] && !expansion[2]) && ((expansion[1] && expansion[3]) || (!expansion[1] && !expansion[3])))
                || (expansion[2] && !expansion[0] && !expansion[1])
                || (expansion[0] && !expansion[2] && !expansion[3]))
            {//if (both n2 and s2 are true) OR (n2 and s2 are false and n1 and s1 are true) OR (all are false)
                return false;
            }
            else
            {
                return true;
            }
        }//isShifted()

        private bool determineOffset(bool[] expansion)
        {
            // expansion[0] = north2
            // expansion[1] = north1       
            // expansion[2] = south2        
            // expansion[3] = south1       
            // expansion[4] = east2
            // expansion[5] = east
            // expansion[6] = west2
            // expansion[7] = west

            if ((!expansion[6] && expansion[7]) || (!expansion[0] && expansion[1]))
            {
                return true;//even
            }
            else
            {
                return false;//odd
            }
        }//end determineOffset()

        //////////////////////////////////////////////////////////////////////
        // PART 6

        private void calculateTileCoords(Point[] cnrPnt, int width, int height, bool shift, bool offset, ref Point[,] pointMatrix)
        {
            double hstag = ((1 / (height - 1f)) * 1) / 2;

            //Console.WriteLine(height);
            for (int y = 0; y < height; y++)
            {
                double percentY = calcPercentage(height, y);
                for (int x = 0; x < width; x++)
                {
                    int xCo = 0, yCo = 0;
                    //Console.WriteLine("\t"+x + "  " + y);
                    double percentX = calcPercentage(width, x);
                    if ((x % 2 != 0 && !offset) || (x % 2 == 0 && offset))
                    {///if staggered row
                        //Console.WriteLine("\tstaggered row : " + x + ", offset : " + offset);
                        if (y != height - 1 || shift)//
                        {///get every point except last in non-shifted staggered row 
                         /// or all points in shifted staggered roow 
                            //Console.WriteLine("\t\ts(" + x + "," + y + ") - " + percentX + " : " + percentY + " : " + hstag + " : " + (percentY + hstag));
                            xCo = calcXCoord(cnrPnt, percentX, percentY + hstag);
                            yCo = calcYCoord(cnrPnt, percentX, percentY + hstag);
                            //Console.WriteLine("\t\ts(" + x + "," + y + ") - " + xCo + " : " + yCo);
                        }
                        else
                        {//if last point skip over
                            continue;
                        }
                    }
                    else
                    {///non-staggered row
                        //Console.WriteLine("\t\t(" + x + "," + y + ") - " + percentX + " : " + percentY);
                        xCo = calcXCoord(cnrPnt, percentX, percentY);
                        yCo = calcYCoord(cnrPnt, percentX, percentY);
                        //Console.WriteLine("\t\t(" + x + "," + y + ") - " + xCo + " : " + yCo);
                    }
                    //Console.WriteLine(px + "  " + py);

                    Point p = new Point(xCo, yCo);
                    //paintR(p);
                    pointMatrix[x, y] = p;
                    //Console.WriteLine(xCo+"  " +yCo);
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

        // cnrPnt[0] = northwest
        // cnrPnt[1] = northeast
        // cnrPnt[2] = southwest
        // cnrPnt[3] = southeast

    }//class HorHexTileCalculations
}
