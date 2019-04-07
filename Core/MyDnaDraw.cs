using System.Collections.Generic;
using System.Xml.Serialization;
using System;

namespace GenArt
{
    
    public class MyDnaDrawing
    {
        private static readonly Random random = new Random();
        //parametrs block
        public static int AddPolygonMutationRate = 700;
        public static int MovePolygonMutationRate = 700;
        public static int RemovePolygonMutationRate = 1500;
        public static int PolygonsMax = 255;
        public static int PolygonsMin;
        public List<MyDnaPolygon> Polygons { get; set; }

        
        public bool isDefected { get; private set; }

        public int pointCount
        {
            get
            {
                int pointCount = 0;
                foreach (MyDnaPolygon polygon in Polygons)
                    pointCount += polygon.Points.Count;

                return pointCount;
            }
        }

        //random generating function
        public static int getRandomNumber(int min, int max)
        {
            return random.Next(min, max);
        }

        // muating function depending on random factor
        public static bool mutate(int mutationRate)
        {
            if (getRandomNumber(0, mutationRate) == 1)
                return true;
            return false;
        }

        //deting defect object
        public void setDefect()
        {
            isDefected = true;
        }

        //init polygyn drawing for generating image
        public void init()
        {
            Polygons = new List<MyDnaPolygon>();

            for (int i = 0; i < PolygonsMin; i++)
                addPol();

            setDefect();
        }

        //clonning the whole drawing polygon
        public MyDnaDrawing reproduction()
        {
            var drawing = new MyDnaDrawing();
            drawing.Polygons = new List<MyDnaPolygon>();
            foreach (MyDnaPolygon polygon in Polygons)
                drawing.Polygons.Add(polygon.reproduction());

            return drawing;
        }

        //mutation function
        public void mutation()
        {
            if (mutate(AddPolygonMutationRate))
                addPol();

            if (mutate(RemovePolygonMutationRate))
                removePol();

            if (mutate(MovePolygonMutationRate))
                putPol();

            foreach (MyDnaPolygon polygon in Polygons)
                polygon.mutation(this);
        }

        //insert polygon 
        public void putPol()
        {
            if (Polygons.Count < 1)
                return;

            int index = getRandomNumber(0, Polygons.Count);
            MyDnaPolygon poly = Polygons[index];
            Polygons.RemoveAt(index);
            index = getRandomNumber(0, Polygons.Count);
            Polygons.Insert(index, poly);
            setDefect();
        }

        //remove polygon
        public void removePol()
        {
            if (Polygons.Count > PolygonsMin)
            {
                int index = getRandomNumber(0, Polygons.Count);
                Polygons.RemoveAt(index);
                setDefect();
            }
        }

        //add polygon to drawing
        public void addPol()
        {
            if (Polygons.Count < PolygonsMax)
            {
                var newPolygon = new MyDnaPolygon();
                newPolygon.init();

                int index = getRandomNumber(0, Polygons.Count);

                Polygons.Insert(index, newPolygon);
                setDefect();
            }
        }
    }
}