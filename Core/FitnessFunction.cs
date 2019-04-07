using System.Drawing;
using System.Drawing.Imaging;

namespace GenArt.Classes
{
    public static class FitnessFunction
    {
        public static int MaxWidth = 512;
        public static int MaxHeight = 512;



        // using getPixel find difference and draw new mutated Draw, change dna
        public static double getDrawingFitness(MyDnaDrawing newDrawing, Color[,] sourceColors)
        {
            double error = 0;

            using (var b = new Bitmap(MaxWidth, MaxHeight, PixelFormat.Format24bppRgb))
            using (Graphics g = Graphics.FromImage(b))
            {
                PolygonDrawing.draw(newDrawing, g, 1);

                BitmapData bmd1 = b.LockBits(new Rectangle(0, 0, MaxWidth, MaxHeight), ImageLockMode.ReadOnly,
                                             PixelFormat.Format24bppRgb);
                for (int y = 0; y < MaxHeight; y++)
                {
                    for (int x = 0; x < MaxWidth; x++)
                    {
                        Color c1 = getPixel(bmd1, x, y);
                        Color c2 = sourceColors[x, y];

                        double pixelError = getColorFitness(c1, c2);
                        error += pixelError;
                    }
                }
                b.UnlockBits(bmd1);
            }
            return error;
        }

        //using The width of the index step of the new Bitmap object in bytes to make diffrence 
        private static unsafe Color getPixel(BitmapData bmd, int x, int y)
        {
            byte* p = (byte*) bmd.Scan0 + y*bmd.Stride + 3*x;
            return Color.FromArgb(p[2], p[1], p[0]);
        }

        //find the diffrence between colors
        private static double getColorFitness(Color c1, Color c2)
        {
            double r = c1.R - c2.R;
            double g = c1.G - c2.G;
            double b = c1.B - c2.B;

            return r*r + g*g + b*b;
        }
    }
}