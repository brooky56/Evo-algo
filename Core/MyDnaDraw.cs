using System.Collections.Generic;
using System.Xml.Serialization;
using GenArt.Classes;
using System;

namespace GenArt
{
    [Serializable]
    public class MyDnaDrawing
    {
        private static readonly Random random = new Random();
        public static int ActiveAddPolygonMutationRate = 700;
        public static int ActiveMovePolygonMutationRate = 700;
        public static int ActiveRemovePolygonMutationRate = 1500;
        public static int ActivePolygonsMax = 255;
        public static int ActivePolygonsMin;
        public List<MyDnaPolygon> Polygons { get; set; }

        [XmlIgnore]
        public bool IsDirty { get; private set; }

        public int PointCount
        {
            get
            {
                int pointCount = 0;
                foreach (MyDnaPolygon polygon in Polygons)
                    pointCount += polygon.Points.Count;

                return pointCount;
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

        public void SetDirty()
        {
            IsDirty = true;
        }

        public void Init()
        {
            Polygons = new List<MyDnaPolygon>();

            for (int i = 0; i < ActivePolygonsMin; i++)
                AddPolygon();

            SetDirty();
        }

        public MyDnaDrawing Clone()
        {
            var drawing = new MyDnaDrawing();
            drawing.Polygons = new List<MyDnaPolygon>();
            foreach (MyDnaPolygon polygon in Polygons)
                drawing.Polygons.Add(polygon.Clone());

            return drawing;
        }


        public void Mutate()
        {
            if (WillMutate(ActiveAddPolygonMutationRate))
                AddPolygon();

            if (WillMutate(ActiveRemovePolygonMutationRate))
                RemovePolygon();

            if (WillMutate(ActiveMovePolygonMutationRate))
                MovePolygon();

            foreach (MyDnaPolygon polygon in Polygons)
                polygon.Mutate(this);
        }

        public void MovePolygon()
        {
            if (Polygons.Count < 1)
                return;

            int index = GetRandomNumber(0, Polygons.Count);
            MyDnaPolygon poly = Polygons[index];
            Polygons.RemoveAt(index);
            index = GetRandomNumber(0, Polygons.Count);
            Polygons.Insert(index, poly);
            SetDirty();
        }

        public void RemovePolygon()
        {
            if (Polygons.Count > ActivePolygonsMin)
            {
                int index = GetRandomNumber(0, Polygons.Count);
                Polygons.RemoveAt(index);
                SetDirty();
            }
        }

        public void AddPolygon()
        {
            if (Polygons.Count < ActivePolygonsMax)
            {
                var newPolygon = new MyDnaPolygon();
                newPolygon.Init();

                int index = GetRandomNumber(0, Polygons.Count);

                Polygons.Insert(index, newPolygon);
                SetDirty();
            }
        }
    }
}