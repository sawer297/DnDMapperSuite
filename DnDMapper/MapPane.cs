using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DnDMapper
{
    public partial class MapPane : UserControl
    {
        private bool fourCorners = false, isDoor = false;
        private int numTileWide, numTileHigh;
        private int tileWidth, tileHeight;
        private int edgeWidth, edgeHeight;

        private Point northwest, northeast, southwest, southeast;
        private List<Point> cornerPoints = new List<Point>(4);
        private Point[,] tileMatrix;
        private Dictionary<Point, Edges> tileMap = new Dictionary<Point, Edges>();

        private Point pointA, pointB;
        private bool innerJumpA = false, innerJumpB = false;

        private static Image nArrow = Image.FromFile(@"CompassImages/Arrow.png");
        private static Image sArrow = Image.FromFile(@"CompassImages/Arrow.png");
        private static Image wArrow = Image.FromFile(@"CompassImages/Arrow.png");
        private static Image eArrow = Image.FromFile(@"CompassImages/Arrow.png");

        private static Image nWall = Image.FromFile(@"CompassImages/Wall.png");
        private static Image sWall = Image.FromFile(@"CompassImages/Wall.png");
        private static Image eWall = Image.FromFile(@"CompassImages/Wall.png");
        private static Image wWall = Image.FromFile(@"CompassImages/Wall.png");

        private static Image north = Image.FromFile(@"CompassImages/North.png");
        private static Image south = Image.FromFile(@"CompassImages/South.png");
        private static Image east = Image.FromFile(@"CompassImages/East.png");
        private static Image west = Image.FromFile(@"CompassImages/West.png");

        private static Image cCenter = Image.FromFile(@"CompassImages/Center.png");
        private static Image nsCenter = Image.FromFile(@"CompassImages/Line.png");
        private static Image weCenter = Image.FromFile(@"CompassImages/Line.png");

        public MapPane()
        {
            InitializeComponent();
            load();
            
        }

        ////////////////////////////////////////////////////////////////////////////////
        //  CUSTOM RESIZE FUNCTION ~ SO THINGS ARE WHERE I WANT THEM
        ////////////////////////////////////////////////////////////////////////////////

        private void MapPane_Resize(object sender, EventArgs e)
        {
            int w = Size.Width, h = Size.Height;

            int w35p = Convert.ToInt32(w * 0.35);
            int h35p = Convert.ToInt32(h * 0.35);

            pnlMapPaneCompass.Size = new Size(w35p - 4, h35p - 4);
            pnlMapPaneCompass.Location = new Point(2, 2);

            tlpnlCompass.Size = pnlMapPaneCompass.Size;
            tlpnlCompass.Location = new Point(0, 0);

            pnlMapPaneControls.Size = new Size(w35p - 4, (h - h35p) - 4);
            pnlMapPaneControls.Location = new Point(2, h35p);

            tabControls.Size = pnlMapPaneControls.Size;
            tabControls.Location = new Point(0, 0);

            btnMapDoorAdd.Size = new Size((pnlMapDoorsControls.Size.Width / 2) - 4, 30);
            btnMapDoorAdd.Location = new Point(2, 2);

            btnMapDoorRemove.Size = new Size((pnlMapDoorsControls.Size.Width / 2) - 4, 30);
            btnMapDoorRemove.Location = new Point(pnlMapDoorsControls.Size.Width / 2, 2);

            pnlMapPaneImage.Size = new Size((w - w35p) - 4, h - 4);
            pnlMapPaneImage.Location = new Point(w35p, 2);
        }//MapPane_Resize()

        private void load()
        {
            sArrow.RotateFlip(RotateFlipType.Rotate180FlipNone);
            eArrow.RotateFlip(RotateFlipType.Rotate90FlipNone);
            wArrow.RotateFlip(RotateFlipType.Rotate270FlipNone);

            sWall.RotateFlip(RotateFlipType.Rotate180FlipNone);
            eWall.RotateFlip(RotateFlipType.Rotate90FlipNone);
            wWall.RotateFlip(RotateFlipType.Rotate270FlipNone);

            weCenter.RotateFlip(RotateFlipType.Rotate90FlipNone); 
        }

        ////////////////////////////////////////////////////////////////////////////////
        //  SET UP FUNCTIONS ~ MAP INFO TAB FUNCTIONS
        ////////////////////////////////////////////////////////////////////////////////

            ///////////////////////////////////////////////////////////////////////
            //  UPLOAD AND RESIZE MAP IMAGE FUNCTIONS 
            ////////////////////////////////

        private void btnUploadMapImage_Click(object sender, EventArgs e)
        {
            fourCorners = false;
            chkbxCornerTiles.Checked = false;
            cornerPoints.Clear();
            tileMap.Clear();
            pnlMapPaneImage.Controls.Clear();

            string filepath = getMapImageFilepath();

            txtbxMapImageFile.Text = filepath;

            if (filepath != "")
            {
                changeMapPaneImage(filepath);
            }
        }//btnUploadMapImage_Click()

        private void btnMapInfoImageResize_Click(object sender, EventArgs e)
        {
            fourCorners = false;
            chkbxCornerTiles.Checked = false;
            cornerPoints.Clear();
            tileMap.Clear();
            pnlMapPaneImage.Controls.Clear();

            string filepath = txtbxMapImageFile.Text;
            changeMapPaneImage(filepath);
        }//btnMapInfoImageResize_Click()

        private string getMapImageFilepath()
        {
            string filepath = "";
            //allow user to browse to file
            OpenFileDialog filePicker = new OpenFileDialog();
            filePicker.Filter = "All Files (*.*)|*.*";
            filePicker.FilterIndex = 1;
            filePicker.Multiselect = false;

            if (filePicker.ShowDialog() == DialogResult.OK)
            {
                filepath = filePicker.FileName;
            }

            return filepath;
        }//getMapImageFilepath()

        private void changeMapPaneImage(string filepath)
        {
            Image img = Image.FromFile(filepath);
            Size s = img.Size;
            
            if (img.Size.Width > pnlMapPaneImage.Size.Width || img.Size.Height > pnlMapPaneImage.Size.Height)
            {
                s = resizeImage(img.Size.Width, img.Size.Height, pnlMapPaneImage.Size.Width, pnlMapPaneImage.Size.Height);
            }

            setMapPaneBackground(filepath, s);
        }//changeMapPaneImage()

        private Size resizeImage(int imgWidth, int imgHeight, int pnlWidth, int pnlHeight)
        {
            Size s;

            while (imgWidth > pnlWidth || imgHeight > pnlHeight)
            {
                imgWidth--;
                imgHeight--;
            }
            
            s = new Size(imgWidth, imgHeight);
            return s;
        }//resizeImage()

        private void setMapPaneBackground(string filepath, Size s)
        {
            Image img = Image.FromFile(filepath);
            Image fimg = (Image)(new Bitmap(img, s));
            pnlMapPaneImage.BackgroundImage = fimg;
        }//setMapPaneBackground()

            ///////////////////////////////////////////////////////////////////////
            //  CORNER POINT CAPTURE FUNCTIONS
            ////////////////////////////////

        private void pnlMapPaneImage_Click(object sender, EventArgs e)
        {
            if (!fourCorners)
            {
                captureMouseClick();
            }
        }//pnlMapPaneImage_Click()

        private void captureMouseClick()
        {
            Point pnt = pnlMapPaneImage.PointToClient(Cursor.Position);
            paint(pnt);

            if (!fourCorners)
            {
                cornerPoints.Add(pnt);
                if (cornerPoints.Count == 4)
                {
                    fourCorners = true;
                    chkbxCornerTiles.Checked = true;
                    clacCorners();
                }
            }
        }//captureMouseClick()

        private void paint(Point p)
        {
            Graphics g = pnlMapPaneImage.CreateGraphics();
            g.DrawEllipse(Pens.Black, p.X - 5, p.Y - 5, 10, 10);
        }//paint()

        private void paintR(Point p)
        {
            Graphics g = pnlMapPaneImage.CreateGraphics();
            g.DrawEllipse(Pens.Red, p.X - 10, p.Y - 10, 20, 20);

            /*
            FontFamily fontFamily = new FontFamily("Arial");
            Font font = new Font(
               fontFamily,
               8,
               FontStyle.Regular,
               GraphicsUnit.Point);
            RectangleF rectF = new RectangleF(10, 10, 500, 500);
            SolidBrush solidBrush = new SolidBrush(Color.Red);

            Point temp = new Point(p.X - 25, p.Y - 20);

            g.DrawString("(" + p.X + " " + p.Y + ")", font, solidBrush, temp);
            */
        }//paintR()

        private void clacCorners()
        {
            int x = 0, y = 0;
            foreach (Point p in cornerPoints)
            {
                x += p.X;
                y += p.Y;
            }

            x /= 4;
            y /= 4;

            foreach (Point p in cornerPoints)
            {
                if (p.X < x)
                {
                    if (p.Y < y)
                    {
                        northwest = p; //NW
                    }
                    else
                    {
                        southwest = p; //SW
                    }
                }
                else
                {
                    if (p.Y < y)
                    {
                        northeast = p;//NE
                    }
                    else
                    {
                        southeast = p;//SE
                    }
                }
            }
        }//clacCorners()

        private void btnResetCornerTiles_Click(object sender, EventArgs e)
        {
            fourCorners = false;
            chkbxCornerTiles.Checked = false;
            cornerPoints.Clear();
            pnlMapPaneImage.Refresh();
        }//btnResetCornerTiles_Click()

            ///////////////////////////////////////////////////////////////////////
            //  CALCULATE POINTS AND GENERATE BUTTONS
            ////////////////////////////////

        private void btnMapGenerate_Click(object sender, EventArgs e)
        {
            tileMap.Clear();
            cornerPoints.Clear();
            pnlMapPaneImage.Controls.Clear();
            if (txtbxNumTileWide.Text != "" && txtbxNumTileHigh.Text != "")
            {
                numTileWide = Convert.ToInt32(txtbxNumTileWide.Text);
                numTileHigh = Convert.ToInt32(txtbxNumTileHigh.Text);
                tileMatrix = new Point[numTileWide, numTileHigh]; //switch this over to doubles to hold precents
                calculateTileCoords(numTileWide, numTileHigh);
                claculateTileWiHi();

                createBtns();
            }
        }//btnMapGenerate_Click()

                /////////////////////////////////////////////////////
                //  PART 1 - CALCULATE TILE COORDS - LINE 269
                ///////////////////////

        private void calculateTileCoords(int width, int height)
        {
            //Console.WriteLine(height);
            for (int y = 0; y < height; y++)
            {
                double percentY = calcPercentage(height, y);
                for (int x = 0; x < width; x++)
                {
                    //Console.WriteLine(x + "  " + y);
                    double percentX = calcPercentage(width, x);
                    //Console.WriteLine(px + "  " + py);
                    int xCo = calcXCoord(percentX, percentY);
                    int yCo = calcYCoord(percentX, percentY);
                    Point p = new Point(xCo, yCo);
                    //paintR(p);
                    tileMatrix[x, y] = p;
                    //Console.WriteLine(xCo+"  " +yCo);
                }
            }
        }//calculateTileCoords()

        private double calcPercentage(int numTile, int kurPnt)
        {
            return (1 / (numTile - 1f)) * kurPnt;
        }//calcpercentage()

        private int calcXCoord(Double Px, Double Py)
        {
            double x = (northwest.X + (northeast.X - northwest.X) * Px) * (1 - Py) + (southwest.X + (southeast.X - southwest.X) * Px) * Py;
            //int r = Convert.ToInt32(Math.Ceiling(x));
            int r = Convert.ToInt32(x);
            //int r = Convert.ToInt32(Math.Floor(x));
            return r;
        }//calcXCoord()

        private int calcYCoord(Double Px, Double Py)
        {
            double y = (northwest.Y + (southwest.Y - northwest.Y) * Py) * (1 - Px) + (northeast.Y + (southeast.Y - northeast.Y) * Py) * Px;
            // int r = Convert.ToInt32(Math.Ceiling(y));
            int r = Convert.ToInt32(y);
            //int r = Convert.ToInt32(Math.Floor(y));
            return r;
        }//clacYCoord()

                /////////////////////////////////////////////////////
                //  PART 2 - CALCULATE TILE WIDTH AND HEIGHT - LINE 270
                ///////////////////////

        private void claculateTileWiHi()
        {
            Point p1 = tileMatrix[0, 0], p3 = tileMatrix[0, 1], p2 = tileMatrix[1, 0];
            tileWidth = Convert.ToInt32(Math.Floor((p2.X - p1.X) * .75));
            tileHeight = Convert.ToInt32(Math.Floor((p3.Y - p1.Y) * .75));
            edgeWidth = Convert.ToInt32(Math.Floor((p2.X - p1.X) * .25));
            edgeHeight = Convert.ToInt32(Math.Floor((p3.Y - p1.Y) * .25));
        }//claculateTileWiHi()

                /////////////////////////////////////////////////////
                //  PART 3 - CREATE BUTTONS - LINE 272
                ///////////////////////

        private void createBtns()
        {
            for (int x = 0; x < numTileWide; x++)
            {
                for (int y = 0; y < numTileHigh; y++)
                {
                    pnlMapPaneImage.Controls.Add(createTileBtn(x, y));
                    Point p = tileMatrix[x, y];
                    Edges e = new Edges();
                    e.addTile(x, y);
                    if ((y - 1) >= 0)
                    {
                        e.addEdge(tileMatrix[x, y - 1], x, y - 1, 0);
                    }//north edge
                    if ((x + 1) < numTileWide)
                    {
                        e.addEdge(tileMatrix[x + 1, y], x + 1, y, 1);
                        pnlMapPaneImage.Controls.Add(createEdgeBtn(x, y, x + 1, y));
                    }//east edge
                    if ((x - 1) >= 0)
                    {
                        e.addEdge(tileMatrix[x - 1, y], x - 1, y, 2);
                    }
                    if ((y + 1) < numTileHigh)
                    {
                        e.addEdge(tileMatrix[x, y + 1], x, y + 1, 3);
                        pnlMapPaneImage.Controls.Add(createEdgeBtn(x, y, x, y + 1));
                    }//south edge
                    tileMap.Add(p, e);
                }
            }
        }//createBtns()

        private int[] tokenize(string str)
        {
            int[] coords = { 0, 0, 0, 0 };
            char[] delimitiers = { '.', '_' };
            string[] split = str.Split(delimitiers);
            int x = 0;
            foreach (string s in split)
            {
                coords[x] = Convert.ToInt32(s);
                x++;
            }
            return coords;
        }//tokenize()

                    /////////////////////////////////////////////////////
                    //  PART 3.1 - CREATE TILE BUTTON - LINE 347
                    ///////////////////////

        private PictureBox createTileBtn(int x, int y)
        {
            PictureBox pic = new PictureBox();

            pic.Size = new Size(tileWidth, tileHeight);

            Point p = tileMatrix[x, y], 
                p2 = new Point(p.X - (tileWidth / 2), p.Y - (tileHeight / 2));
            pic.Location = p2;

            pic.Name = x + "." + y;

            pic.BackColor = Color.FromArgb(100, Color.LightGreen);

            pic.MouseClick += new MouseEventHandler(tile_onclick);
            pic.MouseHover += new EventHandler(tile_hover);
            return pic;
        }//createTileBtn()

        private void tile_onclick(object sender, EventArgs e)
        {
            PictureBox b = sender as PictureBox;
            int[] coords = tokenize(b.Name.ToString());

            Point p = tileMatrix[coords[0], coords[1]];
            if (isDoor)
            {
                setTileDoor(p);
                lstbxMapDoors.Items.Add(b.Name.ToString());
                isDoor = false;
                btnMapDoorRemove.Text = "Remove Door";
            }
            else if (innerJumpA)
            {
                pointA = p;
                innerJumpA = false;
                txtbxMapJumpTileA.Text = b.Name.ToString();
            }
            else if (innerJumpB)
            {
                pointB = p;
                innerJumpB = false;
                txtbxMapJumpTileB.Text = b.Name.ToString();
            } 
            else
            {
                toggleTile(p);
                outputTileData(p);
                tileReColor(p);
            }
        }//tile_onclick()

        private void setTileDoor(Point p)
        {
            tileMap[p].setDoor(4);
            tileReColor(p);
        }//setTileDoor()

        private void tileReColor(Point p)
        {
            int x, y;
            tileMap[p].getCoords(4, out x, out y);
            string id = x + "." + y;

            if (!tileMap[p].getState(4))
            {
                changeTileColor(Color.FromArgb(100, Color.Gray), id);
            }
            else
            {
                if (tileMap[p].isDoor(4))
                {
                    changeTileColor(Color.FromArgb(100, Color.Magenta), id);
                }
                else
                {
                    changeTileColor(Color.FromArgb(100, Color.LightGreen), id);
                }
            }
        }//tileReColor()

        private void changeTileColor(Color c, string id)
        {
            PictureBox pic = pnlMapPaneImage.Controls[id] as PictureBox;
            if (pic != null)
            {
                pic.BackColor = c;
            }
        }//changeTileColor()

        private void toggleTile(Point p)
        {
            Point p2;
            tileMap[p].toggleActive();
            for (int x = 0; x < 4; x++)
            {
                p2 = tileMap[p].getPoint(x);
                if (p2.X != 0)
                {
                    toggleEdge(p, p2);

                    if (tileMap[p].getState(4) && !tileMap[p2].getState(4))
                    {
                        switchTile(p2);
                    }
                }

            }
        }//toggleTile()

        private void toggleEdge(Point p1, Point p2)
        {
            int x1, x2, y1, y2;
            int indexA = tileMap[p1].pointIndex(p2);
            int indexB = tileMap[p2].pointIndex(p1);
            tileMap[p1].getCoords(indexA, out x1, out y1);
            tileMap[p2].getCoords(indexB, out x2, out y2);

            string id = clacEdgeId(x1, y1, x2, y2);

            if (tileMap[p1].getState(4))
            {
                tileMap[p1].enableEdge(indexA);
                tileMap[p2].enableEdge(indexB);
            }
            else
            {
                tileMap[p1].disableEdge(indexA);
                tileMap[p2].disableEdge(indexB);
            }

            edgeReColor(p1, p2, indexA, indexB);

        }//toggleEdge()

        private string clacEdgeId(int x1, int y1, int x2, int y2)
        {
            string id = "";
            if (x1 == x2)
            {
                if (y1 < y2)
                {
                    id = x1 + "." + y1 + "_" + x2 + "." + y2;
                }
                else
                {
                    id = x2 + "." + y2 + "_" + x1 + "." + y1;
                }
            }
            else
            {
                if (x1 < x2)
                {
                    id = x1 + "." + y1 + "_" + x2 + "." + y2;
                }
                else
                {
                    id = x2 + "." + y2 + "_" + x1 + "." + y1;
                }
            }
            return id;
        }//clacEdgeId()

        private void edgeReColor(Point p1, Point p2, int idxA, int idxB)
        {
            int x1, y1, x2, y2;
            tileMap[p1].getCoords(idxA, out x1, out y1);
            tileMap[p2].getCoords(idxB, out x2, out y2);

            string id = clacEdgeId(x1, y1, x2, y2);

            if (tileMap[p1].getState(idxA) && tileMap[p2].getState(idxB))
            {//A<->B
                if (tileMap[p1].isDoor(idxA))
                {
                    changeEdgeColor(Color.Magenta, id);
                }
                else
                {
                    changeEdgeColor(Color.Blue, id);
                }
            }
            else if (!tileMap[p1].getState(idxA) && !tileMap[p2].getState(idxB))
            {//AxxxB
                changeEdgeColor(Color.Red, id);
            }
            else if (tileMap[p1].getState(idxA) && !tileMap[p2].getState(idxB))
            {//A-->B
                if (tileMap[p1].isDoor(idxA))
                {
                    changeEdgeColor(Color.LightPink, id);
                }
                else
                {
                    changeEdgeColor(Color.LightBlue, id);
                }
            }
            else
            {//A<--B
                if (tileMap[p1].isDoor(idxA))
                {
                    changeEdgeColor(Color.DarkMagenta, id);
                }
                else
                {
                    changeEdgeColor(Color.DarkBlue, id);
                }
            }
        }//edgeReColor()

        private void changeEdgeColor(Color c, string id)
        {
            //Console.WriteLine(id);
            Button b = pnlMapPaneImage.Controls[id] as Button;
            if (b != null)
            {
                b.BackColor = c;
            }

        }//changeEdgeColor() 

        private void switchEdge(Point p1, Point p2)
        {
            int x1, y1, x2, y2;
            int indexA = tileMap[p1].pointIndex(p2);
            int indexB = tileMap[p2].pointIndex(p1);
            tileMap[p1].getCoords(indexA, out x1, out y1);
            tileMap[p2].getCoords(indexB, out x2, out y2);

            string id = clacEdgeId(x1, y1, x2, y2);

            if (tileMap[p1].getState(indexA) && tileMap[p2].getState(indexB))
            {//A<->B to AxxxB
                tileMap[p1].disableEdge(indexA);
                tileMap[p2].disableEdge(indexB);
            }
            else if (!tileMap[p1].getState(indexA) && !tileMap[p2].getState(indexB))
            {//AxxxB to A-->B
                if (!tileMap[p1].getState(4) || !tileMap[p2].getState(4))
                {
                    reActiveTile(p1, p2);
                }
                else
                {
                    tileMap[p1].enableEdge(indexA);
                }

            }
            else if (tileMap[p1].getState(indexA) && !tileMap[p2].getState(indexB))
            {//A-->B to A<--B
                tileMap[p1].disableEdge(indexA);
                tileMap[p2].enableEdge(indexB);
            }
            else
            {//A<--B to A<->B
                tileMap[p1].enableEdge(indexA);
            }

            edgeReColor(p1, p2, indexA, indexB);

        }//switchEdge()

        private void reActiveTile(Point p1, Point p2)
        {
            int indexA = tileMap[p1].pointIndex(p2);
            tileMap[p1].enableEdge(indexA);
            int indexB = tileMap[p2].pointIndex(p1);
            tileMap[p2].enableEdge(indexB);

            if (!tileMap[p1].getState(4))
            {
                switchTile(p1);
            }

            if (!tileMap[p2].getState(4))
            {
                switchTile(p2);
            }
        }//reActiveTile()

        private void switchTile(Point p)
        {
            tileMap[p].switchActive();
            tileReColor(p);
        }//switchTile()

        private void tile_hover(object sender, EventArgs e)
        {
            PictureBox b = sender as PictureBox;
            int[] coords = tokenize(b.Name.ToString());

            Point p = tileMatrix[coords[0], coords[1]];
            outputTileData(p);

        }//tile_hover()

        private void outputTileData(Point p)
        {
            //clearCompass();

            if (tileMap[p].getState(0))
            { picCompassInNorth.Image = (new Bitmap(nArrow, picCompassInNorth.Size)); }
            else
            { picCompassInNorth.Image = (new Bitmap(nWall, picCompassInNorth.Size)); }

            if (tileMap[p].getState(1))
            { picCompassInEast.Image = (new Bitmap(eArrow, picCompassInEast.Size)); }
            else
            { picCompassInEast.Image = (new Bitmap(eWall, picCompassInEast.Size)); }

            if (tileMap[p].getState(2))
            { picCompassInWest.Image = (new Bitmap(wArrow, picCompassInWest.Size)); }
            else
            { picCompassInWest.Image = (new Bitmap(wWall, picCompassInWest.Size)); }

            if (tileMap[p].getState(3))
            { picCompassInSouth.Image = (new Bitmap(sArrow, picCompassInSouth.Size)); }
            else
            { picCompassInSouth.Image = (new Bitmap(sWall, picCompassInSouth.Size)); }

            picCompassOutNorth.Image = (new Bitmap(north, picCompassOutNorth.Size));
            picCompassOutEast.Image = (new Bitmap(east, picCompassOutEast.Size));
            picCompassOutSouth.Image = (new Bitmap(south, picCompassOutSouth.Size));
            picCompassOutWest.Image = (new Bitmap(west, picCompassOutWest.Size));
            picCompassCenter.Image = (new Bitmap(cCenter, picCompassCenter.Size));
            

        }//outputTileData()

                    /////////////////////////////////////////////////////
                    //  PART 3.2 - CREATE EDGE BUTTON - LINE 358, 367
                    ///////////////////////

        private Button createEdgeBtn(int x1, int y1, int x2, int y2)
        {
            Button b = new Button();
            b.Size = new Size(edgeWidth, edgeHeight);
            Point pA = tileMatrix[x1, y1], pB = tileMatrix[x2, y2];
            int ax = ((pA.X + pB.X) / 2) - (edgeWidth / 2);
            int ay = ((pA.Y + pB.Y) / 2) - (edgeHeight / 2);
            Point p = new Point(ax, ay);
            b.Location = p;
            b.Text = "";
            b.Name = x1 + "." + y1 + "_" + x2 + "." + y2;
            b.BackColor = Color.Blue;
            b.MouseClick += new MouseEventHandler(edge_onclick);
            b.MouseHover += new EventHandler(edge_hover);
            return b;
        }//createEdgeBtn()

        private void edge_onclick(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string name = b.Name;
            int[] coords = tokenize(b.Name.ToString());

            Point pA = tileMatrix[coords[0], coords[1]];
            Point pB = tileMatrix[coords[2], coords[3]];

            if (isDoor)
            {
                setEdgeDoor(pA, pB);
                lstbxMapDoors.Items.Add(b.Name.ToString());
                isDoor = false;
                btnMapDoorRemove.Text = "Remove Door";
            }
            else
            {
                switchEdge(pA, pB);
                outputEdgeData(pA, pB);
            }
        }//edge_onclick()

        private void setEdgeDoor(Point p1, Point p2)
        {
            int indexA = tileMap[p1].pointIndex(p2);
            int indexB = tileMap[p2].pointIndex(p1);

            tileMap[p1].setDoor(indexA);
            tileMap[p2].setDoor(indexB);

            edgeReColor(p1, p2, indexA, indexB);

        }//setEdgeDoor()

        private void edge_hover(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string name = b.Name;
            int[] coords = tokenize(b.Name.ToString());

            Point p1 = tileMatrix[coords[0], coords[1]];
            Point p2 = tileMatrix[coords[2], coords[3]];
            outputEdgeData(p1, p2); //recode this for new compass
            
        }//edge_hover()

        private void outputEdgeData(Point p1, Point p2)
        {
            int x1, x2;
            int indexA = tileMap[p1].pointIndex(p2);
            int indexB = tileMap[p2].pointIndex(p1);
            tileMap[p1].getCoords(4, out x1, out int y1);
            tileMap[p2].getCoords(4, out x2, out int y2);

            if (x1 == x2) //vertical edge
            {
                displayNS(tileMap[p1].getState(indexA), tileMap[p2].getState(indexB));
            }
            else //horizontal edge
            {
                displayWE(tileMap[p1].getState(indexA), tileMap[p2].getState(indexB));
            }
        }//outputEdgeData()

        public void displayWE(bool westState, bool eastState)
        {
            picCompassInNorth.Image = null;
            picCompassOutNorth.Image = null;
            picCompassInSouth.Image = null;
            picCompassOutSouth.Image = null;
            picCompassOutEast.Image = (new Bitmap(east, picCompassOutEast.Size));
            picCompassOutWest.Image = (new Bitmap(west, picCompassOutWest.Size));
            picCompassCenter.Image = (new Bitmap(weCenter, picCompassCenter.Size));

            if (westState && eastState)
            {
                picCompassInWest.Image = (new Bitmap(wArrow, picCompassInWest.Size));
                picCompassInEast.Image = (new Bitmap(eArrow, picCompassInEast.Size));
            }
            else if (!westState && eastState)
            {
                picCompassInWest.Image = (new Bitmap(wArrow, picCompassInWest.Size));
                picCompassInEast.Image = (new Bitmap(eWall, picCompassInEast.Size));
            }
            else if (westState && !eastState)
            {
                picCompassInWest.Image = (new Bitmap(wWall, picCompassInWest.Size));
                picCompassInEast.Image = (new Bitmap(eArrow, picCompassInEast.Size));
            }
            else
            {
                picCompassInWest.Image = (new Bitmap(wWall, picCompassInWest.Size));
                picCompassInEast.Image = (new Bitmap(eWall, picCompassInEast.Size));
            }
        }//displaWE()

        public void displayNS(bool northState, bool southState)
        {
            picCompassInEast.Image = null;
            picCompassOutEast.Image = null;
            picCompassInWest.Image = null;
            picCompassOutWest.Image = null;
            picCompassOutNorth.Image = (new Bitmap(north, picCompassOutNorth.Size));
            picCompassOutSouth.Image = (new Bitmap(south, picCompassOutSouth.Size));
            picCompassCenter.Image = (new Bitmap(nsCenter, picCompassCenter.Size));

            if (northState && southState)
            {
                picCompassInNorth.Image = (new Bitmap(nArrow, picCompassInNorth.Size));
                picCompassInSouth.Image = (new Bitmap(sArrow, picCompassInSouth.Size));
            }
            else if (!northState && southState)
            {
                picCompassInNorth.Image = (new Bitmap(nArrow, picCompassInNorth.Size));
                picCompassInSouth.Image = (new Bitmap(sWall, picCompassInSouth.Size));
            }
            else if (northState && !southState)
            {
                picCompassInNorth.Image = (new Bitmap(nWall, picCompassInNorth.Size));
                picCompassInSouth.Image = (new Bitmap(sArrow, picCompassInSouth.Size));
            }
            else
            {
                picCompassInNorth.Image = (new Bitmap(nWall, picCompassInNorth.Size));
                picCompassInSouth.Image = (new Bitmap(sWall, picCompassInSouth.Size));
            }
        }//displaWE()

        ////////////////////////////////////////////////////////////////////////////////
        //  MAP DOOR FUNCTIONS
        ////////////////////////////////////////////////////////////////////////////////

        private void btnMapDoorAdd_Click(object sender, EventArgs e)
        {
            //outputLabel.lblOutText.Visible = true;
            //outputLabel.lblOutText.Text = "Please click a tile or edge button to set door";
            btnMapDoorRemove.Text = "Cancel";
            isDoor = true;
        }//btnMapDoorAdd_Click()

        private void btnMapDoorRemove_Click(object sender, EventArgs e)
        {
            if (isDoor)
            {
                isDoor = false;
                btnMapDoorRemove.Text = "Remove Door";
            }
            else
            {
                if (lstbxMapDoors.SelectedIndex != -1)
                {
                    determineEdgeOrTile(lstbxMapDoors.SelectedItem.ToString());
                    lstbxMapDoors.Items.Remove(lstbxMapDoors.SelectedItem);
                    //outputLabel.lblOutText.Visible = true;
                    //outputLabel.lblOutText.Text = "Door removed";
                }
            }
        }//btnMapDoorRemove_Click()

        private void determineEdgeOrTile(string id)
        {
            int[] coords = tokenize(id);
            if (coords[2] == 0)
            {
                Point p = tileMatrix[coords[0], coords[1]];
                removeTileDoor(p);
                changeTileColor(Color.FromArgb(100, Color.LightGreen), id);
            }
            else
            {
                Point p1 = tileMatrix[coords[0], coords[1]];
                Point p2 = tileMatrix[coords[2], coords[3]];
                removeEdgeDoor(p1, p2);
                changeEdgeColor(Color.Blue, id);
            }
        }//determineEdgeOrTile()

        private void removeTileDoor(Point p)
        {
            tileMap[p].disableDoor(4);
            tileReColor(p);
        }//removeTileDoor()

        private void removeEdgeDoor(Point p1, Point p2)
        {
            int indexA = tileMap[p1].pointIndex(p2);
            int indexB = tileMap[p2].pointIndex(p1);

            tileMap[p1].disableDoor(indexA);
            tileMap[p2].disableDoor(indexB);

            edgeReColor(p1, p2, indexA, indexB);

        }//removeEdgeDoor()

        ////////////////////////////////////////////////////////////////////////////////
        //  MAP JUMP FUNCTIONS
        ////////////////////////////////////////////////////////////////////////////////

        private void btnPointA_Click(object sender, EventArgs e)
        {
            innerJumpA = true;
        }//btnPointA_Click()

        private void btnPointB_Click(object sender, EventArgs e)
        {
            innerJumpB = true;
        }//btnPointB_Click()

        private void btnJump_Click(object sender, EventArgs e)
        {
            if (pointA != null && pointB != null)
            {
                tileMap[pointA].addInJump(pointB);
                tileMap[pointB].addInJump(pointA);
                paintline(pointA, pointB);
            }
        }//BtnJump_Click()

        private void paintline(Point p1, Point p2)
        {
            Graphics g = pnlMapPaneImage.CreateGraphics();
            //g.DrawEllipse(Pens.Black, p.X - 5, p.Y - 5, 10, 10);
            g.DrawLine(Pens.Red, p1, p2);

        }//paint()

    }
}

////////////////////////////////////////////////////////////////////////////////
//  TEMPLATE
////////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////
//  TEMPLATE
////////////////////////////////

/////////////////////////////////////////////////////
//  TEMPLATE
///////////////////////