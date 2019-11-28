using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DnDMapper
{
    class Edges
    {
        //North, East, West, South, Tile edges
        private Boolean[] NEWST = new Boolean[5] { false, false, false, false, true };

        //point locations
        private List<Point> Points = new List<Point>(4) { new Point(), new Point(), new Point(), new Point()};

        //matrix coordinates
        private int[,] coords = new int[5, 2] { {-1,-1 }, {-1,-1 }, {-1,-1 }, {-1,-1 }, {-1,-1 } };

        //if edges or tile is a door
        private Boolean[] doors = new Boolean[5] { false, false, false, false, false };

        //List of map's inner jumps ( i.e. stairs, ladders, ect ...) 
        private List<Point> inJumps = new List<Point>();

        public Edges()
        {

        }//Edges()

        public bool getState(int index)
        {
            return NEWST[index];
        }//getState()

        public void disableEdge(int index)
        {
            NEWST[index] = false;
        }//disableEdge()

        public void enableEdge(int index)
        {
            NEWST[index] = true;
        }//enableEdge()

        public int pointIndex(Point p)
        {
            return Points.IndexOf(p);
        }//pointIndex()

        public Point getPoint(int index)
        {
            return Points[index];
        }//getPoint()

        public void getCoords(int index, out int x, out int y)
        {
            x = coords[index, 0];
            y = coords[index, 1];
        }//getCoords()

        public void addTile(int xCoord, int yCoord)
        {
            coords[4, 0] = xCoord;
            coords[4, 1] = yCoord;
        }//addTile()

        public void addEdge(Point pLoc, int xCoord, int yCoord, int side)
        {
            Points[side] = pLoc;
            NEWST[side] = true;
            coords[side, 0] = xCoord;
            coords[side, 1] = yCoord;
        }//addEdge()

        public void toggleActive()
        {
            NEWST[4] = !NEWST[4];
            checkActive();
        }//toggleActive()

        public void switchActive()
        {
            NEWST[4] = !NEWST[4];
        }//switchActive()

        private void checkActive()
        {
            for (int x = 0; x < 4; x++)
            {
                if (Points[x].X != 0)
                {
                    NEWST[x] = NEWST[4];
                }
            }
        }//checkActive()

        public bool isDoor(int index)
        {
            return doors[index];
        }//isDoor()

        public void setDoor(int index)
        {
            doors[index] = true;
            //maybe need to work here more
        }//setDoor()

        public void disableDoor(int index)
        {
            doors[index] = false;
        }//disableDoor()

        public void addInJump(Point pnt)
        {
            inJumps.Add(pnt);
        }//addInJump()

        public bool hasInJumps()
        {
            return (inJumps.Count > 0);
        }//hasInJumps()

        public List<Point> getInJumps()
        {
            return inJumps;
        }//getInJumps()

    }
}
