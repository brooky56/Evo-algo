using System;

namespace GenArt
{
    [Serializable]
    public class MyDnaPoint
    {
       
        public static int ActiveMovePointMaxMutationRate = 1500;
        public static int ActiveMovePointMidMutationRate = 1500;
        public static int ActiveMovePointMinMutationRate = 1500;
        public static int ActiveMovePointRangeMid = 20;
        public static int ActiveMovePointRangeMin = 3;

   
        public int X { get; set; }
        public int Y { get; set; }

        private static readonly Random random = new Random();

        public static int MaxWidth = 512;
        public static int MaxHeight = 512;

        public void Init()
        {
            X = GetRandomNumber(0, MaxWidth);
            Y = GetRandomNumber(0, MaxHeight);
        }

        public MyDnaPoint Clone()
        {
            return new MyDnaPoint
                       {
                           X = X,
                           Y = Y,
                       };
        }

        public void Mutate(MyDnaDrawing drawing)
        {
            if (WillMutate(ActiveMovePointMaxMutationRate))
            {
                X = GetRandomNumber(0, MaxWidth);
                Y = GetRandomNumber(0, MaxHeight);
                drawing.SetDirty();
            }

            if (WillMutate(ActiveMovePointMidMutationRate))
            {
                X =
                    Math.Min(
                        Math.Max(0,
                                 X +
                                 GetRandomNumber(-ActiveMovePointRangeMid,
                                                       ActiveMovePointRangeMid)), MaxWidth);
                Y =
                    Math.Min(
                        Math.Max(0,
                                 Y +
                                 GetRandomNumber(-ActiveMovePointRangeMid,
                                                      ActiveMovePointRangeMid)), MaxHeight);
                drawing.SetDirty();
            }

            if (WillMutate(ActiveMovePointMinMutationRate))
            {
                X =
                    Math.Min(
                        Math.Max(0,
                                 X +
                                 GetRandomNumber(-ActiveMovePointRangeMin,
                                                       ActiveMovePointRangeMin)), MaxWidth);
                Y =
                    Math.Min(
                        Math.Max(0,
                                 Y +
                                 GetRandomNumber(-ActiveMovePointRangeMin,
                                                       ActiveMovePointRangeMin)), MaxHeight);
                drawing.SetDirty();
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
    }
}