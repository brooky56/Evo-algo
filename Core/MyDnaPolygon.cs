using System;
using System.Collections.Generic;
using GenArt.Classes;

namespace GenArt
{
    
    public class MyDnaPolygon
    {
        public List<MyDnaPoint> Points { get; set; }
        public MyDnaBrush Brush { get; set; }

        private static readonly Random random = new Random();
        
        public static int MaxWidth = 512;
        public static int MaxHeight = 512;
        //parameters block
        public static int PointsPerPolygonMin = 3;
        public static int AddPointMutationRate = 1500;
        public static int RemovePointMutationRate = 1500;
        public static int PointsMin;
        public static int PointsPerPolygonMax = 12;
        public static int PointsMax = 1500;

        //init polygon
        public void init()
        {
            Points = new List<MyDnaPoint>();
            var origin = new MyDnaPoint();
            origin.init();

            for (int i = 0; i < PointsPerPolygonMin; i++)
            {
                var point = new MyDnaPoint();
                point.x = Math.Min(Math.Max(0, origin.x + getRandomNumber(-3, 3)), MaxWidth);
                point.y = Math.Min(Math.Max(0, origin.y + getRandomNumber(-3, 3)), MaxHeight);

                Points.Add(point);
            }
            //fill polygon using our Brush
            Brush = new MyDnaBrush();
            Brush.init();
        }

        //clonning of polygons
        public MyDnaPolygon reproduction()
        {
            var newPolygon = new MyDnaPolygon();
            newPolygon.Points = new List<MyDnaPoint>();
            newPolygon.Brush = Brush.reproduction();
            foreach (MyDnaPoint point in Points)
                newPolygon.Points.Add(point.reproduction());

            return newPolygon;
        }

        //mutation function for polygons 
        public void mutation(MyDnaDrawing drawing)
        {
            if (mutate(AddPointMutationRate))
                addPoint(drawing);

            if (mutate(RemovePointMutationRate))
                removePoint(drawing);

            Brush.mutatation(drawing);
            Points.ForEach(p => p.mutation(drawing));
        }

        //if generated chromosome of our population is not fit to us we set dirty
        //working with polygon we can expand it make another shape
        private void removePoint(MyDnaDrawing drawing)
        {
            if (Points.Count > PointsPerPolygonMin)
            {
                if (drawing.pointCount > PointsMin)
                {
                    int index = getRandomNumber(0, Points.Count);
                    Points.RemoveAt(index);
                    drawing.setDefect();
                }
            }
        }
        //random genearing function
        public static int getRandomNumber(int min, int max)
        {
            return random.Next(min, max);
        }

        //mutation based in random function
        public static bool mutate(int mutationRate)
        {
            if (getRandomNumber(0, mutationRate) == 1)
                return true;
            return false;
        }

        //adding points in polygon
        //modify polygon with adding points (make from 3th(min) -> 5th edges )
        private void addPoint(MyDnaDrawing drawing)
        {
            if (Points.Count < PointsPerPolygonMax)
            {
                if (drawing.pointCount < PointsMax)
                {
                    var newPoint = new MyDnaPoint();

                    int index = getRandomNumber(1, Points.Count - 1);

                    MyDnaPoint prev = Points[index - 1];
                    MyDnaPoint next = Points[index];

                    newPoint.x = (prev.x + next.x)/2;
                    newPoint.y = (prev.y + next.y)/2;

                    Points.Insert(index, newPoint);
                    drawing.setDefect();
                }
            }
        }
    }
}