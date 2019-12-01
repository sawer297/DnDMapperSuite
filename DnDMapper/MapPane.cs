using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

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

        private SquareTileCalculations sqTileCalc = new SquareTileCalculations();
        private HorHexTileCalculations horTileCalc = new HorHexTileCalculations();
        private VerHexTileCalculations verTileCalc = new VerHexTileCalculations();

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
            
            /*
            if (img.Size.Width > pnlMapPaneImage.Size.Width || img.Size.Height > pnlMapPaneImage.Size.Height)
            {
                s = resizeImage(img.Size.Width, img.Size.Height, pnlMapPaneImage.Size.Width, pnlMapPaneImage.Size.Height);
            }
            */

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

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }//ResizeImage()

        private void setMapPaneBackground(string filepath, Size s)
        {
            Image img = Image.FromFile(filepath);
            //Image fimg = (Image)(new Bitmap(img, s));
            Image fimg = (Image)ResizeImage(img, pnlMapPaneImage.Size.Width, pnlMapPaneImage.Size.Height);
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
            }//foreach

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
                }//if
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
                }//else
            }//foreach

            cornerPoints[0] = northwest;
            cornerPoints[1] = northeast;
            cornerPoints[2] = southwest;
            cornerPoints[3] = southeast;

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
            //cornerPoints.Clear();
            pnlMapPaneImage.Controls.Clear();
            if (txtbxNumTileWide.Text != "" && txtbxNumTileHigh.Text != "")
            {
                numTileWide = Convert.ToInt32(txtbxNumTileWide.Text);
                numTileHigh = Convert.ToInt32(txtbxNumTileHigh.Text);

                if (rbtnSquare.Checked)
                {
                    tileMatrix = sqTileCalc.calculateSquTilePoints(cornerPoints.ToArray(), numTileHigh, numTileWide);
                }
                else if (rbtnHexHor.Checked)
                {
                    tileMatrix = horTileCalc.calculateHorizontalHexPoints(cornerPoints.ToArray(), ref numTileHigh, ref numTileWide, pnlMapPaneImage.Height, pnlMapPaneImage.Width);
                }
                else
                {
                    tileMatrix = verTileCalc.calculateVerticalHexPoints(cornerPoints.ToArray(), ref numTileHigh, ref numTileWide, pnlMapPaneImage.Height, pnlMapPaneImage.Width);
                }
                
                Console.WriteLine("return checkpoint");
                printPoints(tileMatrix, numTileHigh, numTileWide);
                Console.WriteLine("print checkpoint");

                claculateTileWiHi();

            }
        }//btnMapGenerate_Click()

        private void printPoints(Point[,] points, int height, int width)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //Console.WriteLine("\t\t(" + x + "," + y + ") - " + points[x, y].X + " : " + points[x, y].Y);
                    if (points[x, y].X != 0 && points[x, y].Y != 0)
                    {
                        paintR(points[x, y]);
                    }
                }//for x
            }//for y
        }//printPoints()

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

    }//class MapPane
}

////////////////////////////////////////////////////////////////////////////////
//  MAJOR SECTION TEMPLATE
////////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////
//  MINOR SECTION TEMPLATE
////////////////////////////////

/////////////////////////////////////////////////////
//  TEMPLATE
///////////////////////