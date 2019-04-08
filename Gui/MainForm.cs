using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using GenArt.Classes;

namespace GenArt
{
    public partial class MainForm : Form
    {
        
        private MyDnaDrawing currentDrawing;

        private double errorLevel = double.MaxValue;
        private int generation;
        private MyDnaDrawing guiDrawing;
        private bool isRunning;
        private DateTime lastRepaint = DateTime.MinValue;
        private int lastSelected;
        private TimeSpan repaintIntervall = new TimeSpan(0, 0, 0, 0, 0);
        private int repaintOnSelectedSteps = 4;
        private int selected;


        public static int MaxWidth = 512;
        public static int MaxHeight = 512;

        private Color[,] sourceColors;

        private Thread thread;
      

        public MainForm()
        {
            InitializeComponent();
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //to loading form
        }

        private static MyDnaDrawing GetNewInitializedDrawing()
        {
            var drawing = new MyDnaDrawing();
            drawing.init();
            return drawing;
        }


        private void StartEvolution()
        {
            splitSourceImage();
            if (currentDrawing == null)
                currentDrawing = GetNewInitializedDrawing();
            lastSelected = 0;
            
            int i = 0;
            while (isRunning)
            {
                MyDnaDrawing newDrawing;
                lock (currentDrawing)
                {
                    newDrawing = currentDrawing.reproduction();
                }
                newDrawing.mutation();

                if (newDrawing.isDefected)
                {
                    generation++;

                    double newErrorLevel = FitnessFunction.getDrawingFitness(newDrawing, sourceColors);

                    if (newErrorLevel <= errorLevel)
                    {
                        selected++;
                        lock (currentDrawing)
                        {
                            currentDrawing = newDrawing;
                        }
                        errorLevel = newErrorLevel;
                    }
                }
                
                   
                
            }
        }

        //covnerts the source image to a Color[,] for faster lookup
        private void splitSourceImage()
        {
            sourceColors = new Color[MaxWidth, MaxHeight];
            var sourceImage = this.sourceImage.Image as Bitmap;

            for (int y = 0; y < MaxHeight; y++)
            {
                for (int x = 0; x < MaxWidth; x++)
                {
                    Color c = sourceImage.GetPixel(x, y);
                    sourceColors[x, y] = c;
                }
            }
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            if (isRunning)
                stop();
            else
                start();
        }

        private void start()
        {
            btnStart.Text = "Stop";
            isRunning = true;
            tmrRedraw.Enabled = true;


            if (thread != null)
                killThread();

            thread = new Thread(StartEvolution)
                         {
                             IsBackground = true,
                             Priority = ThreadPriority.AboveNormal
                         };

            thread.Start();
        }

        private void killThread()
        {
            if (thread != null)
            {
                thread.Abort();
            }
            thread = null;
        }

        private void stop()
        {
            if (isRunning)
                killThread();

            btnStart.Text = "Start";
            isRunning = false;
            tmrRedraw.Enabled = false;

            
        }


        /*
        private void CreateSaveBitmap(Canvas canvas, string filename)
        {
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
             (int)canvas.Width, (int)canvas.Height,
             96d, 96d, PixelFormats.Pbgra32);
            // needed otherwise the image output is black
            canvas.Measure(new Size((int)canvas.Width, (int)canvas.Height));
            canvas.Arrange(new Rect(new Size((int)canvas.Width, (int)canvas.Height)));

            renderBitmap.Render(canvas);

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            using (FileStream file = File.Create(filename))
            {
                encoder.Save(file);
            }
        }*/
        private void tmrRedraw_Tick(object sender, EventArgs e)
        {
           

            if (currentDrawing == null)
                return;

            int polygons = currentDrawing.Polygons.Count;
            int points = currentDrawing.pointCount;
            double avg = 0;

            if (polygons != 0)
                avg = points/polygons;

            toolStripStatusLabelFitness.Text = errorLevel.ToString();
            toolStripStatusLabelGeneration.Text = generation.ToString();
            toolStripStatusLabelSelected.Text = selected.ToString();
            toolStripStatusLabelPolygons.Text = polygons.ToString();

            bool shouldRepaint = false;
            if (repaintIntervall.Ticks > 0)
                if (lastRepaint < DateTime.Now - repaintIntervall)
                    shouldRepaint = true;

            if (repaintOnSelectedSteps > 0)
                if (lastSelected + repaintOnSelectedSteps < selected)
                    shouldRepaint = true;

            if (shouldRepaint)
            {
                lock (currentDrawing)
                {
                    guiDrawing = currentDrawing.reproduction();
                }
                genFrame.Invalidate();
                lastRepaint = DateTime.Now;
                lastSelected = selected;
            }
        }

        private void pnlCanvas_Paint(object sender, PaintEventArgs e)
        {
            if (guiDrawing == null)
            {
                e.Graphics.Clear(Color.Black);
                return;
            }
            using (var backBuffer = new Bitmap(sourceImage.Width, sourceImage.Height, PixelFormat.Format24bppRgb))

            using (Graphics backGraphics = Graphics.FromImage(backBuffer))
            {
                backGraphics.SmoothingMode = SmoothingMode.HighQuality;
                PolygonDrawing.draw(guiDrawing, backGraphics, 1);

                e.Graphics.DrawImage(backBuffer, 0, 0);
            }
        }

        private void openImage()
        {
            stop();
            string fileName = FileWorking.openFileName(FileWorking.imageType);
            sourceImage.Image = Image.FromFile(fileName);
            MaxHeight = sourceImage.Height;
            MaxWidth = sourceImage.Width;
            //set image
            splitContainer1.SplitterDistance = sourceImage.Width + 30;
            button_open.Text = "Save image";
        }

        private void button_open_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                Bitmap bitmap = new Bitmap(genFrame.Width, genFrame.Height);
                genFrame.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                bitmap.Save("test" + generation + ".jpg", ImageFormat.Jpeg);
            }
            else
            {
                button_open.Text = "Open image";
                openImage();
            }
        }
      
            
      

    }
}