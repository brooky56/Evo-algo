﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using GenArt;
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
        private int repaintOnSelectedSteps = 3;
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
            drawing.Init();
            return drawing;
        }


        private void StartEvolution()
        {
            SetupSourceColorMatrix();
            if (currentDrawing == null)
                currentDrawing = GetNewInitializedDrawing();
            lastSelected = 0;

            while (isRunning)
            {
                MyDnaDrawing newDrawing;
                lock (currentDrawing)
                {
                    newDrawing = currentDrawing.Clone();
                }
                newDrawing.Mutate();

                if (newDrawing.IsDirty)
                {
                    generation++;

                    double newErrorLevel = FitnessFunction.GetDrawingFitness(newDrawing, sourceColors);

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
        private void SetupSourceColorMatrix()
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
                Stop();
            else
                Start();
        }

        private void Start()
        {
            btnStart.Text = "Stop";
            isRunning = true;
            tmrRedraw.Enabled = true;


            if (thread != null)
                KillThread();

            thread = new Thread(StartEvolution)
                         {
                             IsBackground = true,
                             Priority = ThreadPriority.AboveNormal
                         };

            thread.Start();
        }

        private void KillThread()
        {
            if (thread != null)
            {
                thread.Abort();
            }
            thread = null;
        }

        private void Stop()
        {
            if (isRunning)
                KillThread();

            btnStart.Text = "Start";
            isRunning = false;
            tmrRedraw.Enabled = false;

        }

        private void tmrRedraw_Tick(object sender, EventArgs e)
        {
            if (currentDrawing == null)
                return;

            int polygons = currentDrawing.Polygons.Count;
            int points = currentDrawing.PointCount;
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
                    guiDrawing = currentDrawing.Clone();
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

        private void OpenImage()
        {
            Stop();
            string fileName = FileWorking.openFileName(FileWorking.imageType);
            sourceImage.Image = Image.FromFile(fileName);
            MaxHeight = sourceImage.Height;
            MaxWidth = sourceImage.Width;
            //set image
            splitContainer1.SplitterDistance = sourceImage.Width + 30;
        }

        private void button_open_Click(object sender, EventArgs e)
        {
            OpenImage();
        }

    }
}