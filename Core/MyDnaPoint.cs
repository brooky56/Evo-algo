using System;

namespace GenArt
{
  
    public class MyDnaPoint
    {
       
        public static int MovePointMaxMutationRate = 1500;
        public static int MovePointMidMutationRate = 1500;
        public static int MovePointMinMutationRate = 1500;
        public static int MovePointRangeMid = 20;
        public static int MovePointRangeMin = 3;

   
        public int x { get; set; }
        public int y { get; set; }

        private static readonly Random random = new Random();

        public static int MaxWidth = 512;
        public static int MaxHeight = 512;

        //init point
        public void init()
        {
            x = getRandomNumber(0, MaxWidth);
            y = getRandomNumber(0, MaxHeight);
        }

        //cloning points
        public MyDnaPoint reproduction()
        {
            return new MyDnaPoint
                       {
                           x = x,
                           y = y,
                       };
        }

        //main muation function for evolving our chromosomes for points
        public void mutation(MyDnaDrawing drawing)
        {
            if (mutate(MovePointMaxMutationRate))
            {
                x = getRandomNumber(0, MaxWidth);
                y = getRandomNumber(0, MaxHeight);
                drawing.setDefect();
            }

            if (mutate(MovePointMidMutationRate))
            {
                x =
                    Math.Min(
                        Math.Max(0,
                                 x +
                                 getRandomNumber(-MovePointRangeMid,
                                                       MovePointRangeMid)), MaxWidth);
                y =
                    Math.Min(
                        Math.Max(0,
                                 y +
                                 getRandomNumber(-MovePointRangeMid,
                                                      MovePointRangeMid)), MaxHeight);
                drawing.setDefect();
            }

            if (mutate(MovePointMinMutationRate))
            {
                x =
                    Math.Min(
                        Math.Max(0,
                                x +
                                 getRandomNumber(-MovePointRangeMin,
                                                       MovePointRangeMin)), MaxWidth);
                y =
                    Math.Min(
                        Math.Max(0,
                                y +
                                 getRandomNumber(-MovePointRangeMin,
                                                       MovePointRangeMin)), MaxHeight);
                drawing.setDefect();
            }
        }
       
        //random generating function
        public static int getRandomNumber(int min, int max)
        {
            return random.Next(min, max);
        }

        //mutation function
        public static bool mutate(int mutationRate)
        {
            if (getRandomNumber(0, mutationRate) == 1)
                return true;
            return false;
        }
    }
}