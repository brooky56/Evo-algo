using System;
using System.Collections.Generic;
using GenArt.Classes;

namespace GenArt
{
    [Serializable]
    public class MyDnaPolygon
    {
        public List<MyDnaPoint> Points { get; set; }
        public MyDnaBrush Brush { get; set; }

        private static readonly Random random = new Random();
        
        public static int MaxWidth = 512;
        public static int MaxHeight = 512;

        public static int ActivePointsPerPolygonMin = 3;
        public static int ActiveAddPointMutationRate = 1500;
        public static int ActiveRemovePointMutationRate = 1500;
        public static int ActivePointsMin;
        public static int ActivePointsPerPolygonMax = 10;
        public static int ActivePointsMax = 1500;

        public void Init()
        {
            Points = new List<MyDnaPoint>();
            var origin = new MyDnaPoint();
            origin.Init();

            for (int i = 0; i < ActivePointsPerPolygonMin; i++)
            {
                var point = new MyDnaPoint();
                point.X = Math.Min(Math.Max(0, origin.X + GetRandomNumber(-3, 3)), MaxWidth);
                point.Y = Math.Min(Math.Max(0, origin.Y + GetRandomNumber(-3, 3)), MaxHeight);

                Points.Add(point);
            }

            Brush = new MyDnaBrush();
            Brush.init();
        }

        public MyDnaPolygon Clone()
        {
            var newPolygon = new MyDnaPolygon();
            newPolygon.Points = new List<MyDnaPoint>();
            newPolygon.Brush = Brush.reproduction();
            foreach (MyDnaPoint point in Points)
                newPolygon.Points.Add(point.Clone());

            return newPolygon;
        }

        public void Mutate(MyDnaDrawing drawing)
        {
            if (WillMutate(ActiveAddPointMutationRate))
                AddPoint(drawing);

            if (WillMutate(ActiveRemovePointMutationRate))
                RemovePoint(drawing);

            Brush.mutatation(drawing);
            Points.ForEach(p => p.Mutate(drawing));
        }

        private void RemovePoint(MyDnaDrawing drawing)
        {
            if (Points.Count > ActivePointsPerPolygonMin)
            {
                if (drawing.PointCount > ActivePointsMin)
                {
                    int index = GetRandomNumber(0, Points.Count);
                    Points.RemoveAt(index);

                    drawing.SetDirty();
                }
            }
        }
        public static int GetRandomNumber(int min, int max)
        {
            return random.Next(min, max);
        }

        public static bool WillMutate(int mutationRate)
        {
            if (GetRandomNumber(0, mutationRate) == 1)
                return true;
            return false;
        }

        private void AddPoint(MyDnaDrawing drawing)
        {
            if (Points.Count < ActivePointsPerPolygonMax)
            {
                if (drawing.PointCount < ActivePointsMax)
                {
                    var newPoint = new MyDnaPoint();

                    int index = GetRandomNumber(1, Points.Count - 1);

                    MyDnaPoint prev = Points[index - 1];
                    MyDnaPoint next = Points[index];

                    newPoint.X = (prev.X + next.X)/2;
                    newPoint.Y = (prev.Y + next.Y)/2;


                    Points.Insert(index, newPoint);

                    drawing.SetDirty();
                }
            }
        }
    }
}