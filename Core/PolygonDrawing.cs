using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

using GenArt;

namespace GenArt.Classes
{
    public static class PolygonDrawing 
    {
        //Render a image drawing
        public static void draw(MyDnaDrawing drawing, Graphics g, int scale)
        {
            g.Clear(Color.White);
            foreach (MyDnaPolygon polygon in drawing.Polygons)
                drawPolygon(polygon, g, scale);
        }

        //draw a polygon
        private static void drawPolygon(MyDnaPolygon polygon, Graphics g, int scale)
        {
            using (Brush brush = getSystemBrush(polygon.Brush))
            {
                Point[] points = getSystemPoints(polygon.Points, scale);
                g.FillPolygon(brush,points);
            }
        }

        //Convert a list of dna_points to system point to draw it
        private static Point[] getSystemPoints(IList<MyDnaPoint> points,int scale)
        {
            Point[] pts = new Point[points.Count];
            int i = 0;
            foreach (MyDnaPoint pt in points)
            {
                pts[i++] = new Point(pt.X * scale, pt.Y * scale);
            }
            return pts;
        }


        //Convert a list of my_brush to system brash to draw it
        /// <summary>
        /// Brushes we use to fill our polygons we should convert our mutated color to our mutated polygons and then draw it
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static Brush getSystemBrush(MyDnaBrush b)
        {
            return new SolidBrush(Color.FromArgb(b.alpha, b.red, b.green, b.blue));
        }
    }
}
