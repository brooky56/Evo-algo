using System;

namespace GenArt
{
    [Serializable]
    public class MyDnaBrush
    {
        public int red { get; set; }
        public int green { get; set; }
        public int blue { get; set; }
        public int alpha { get; set; }

        private static readonly Random random = new Random();
        //here we can chanege paramenters to improve algo proccesing
        //properties block
        public static int AlphaRangeMax = 60;
        public static int AlphaRangeMin = 30;
        public static int AlphaMutationRate = 1500;
        public static int BlueRangeMax = 255;
        public static int BlueRangeMin;
        public static int BlueMutationRate = 1500;
        public static int GreenRangeMax = 255;
        public static int GreenRangeMin;
        public static int GreenMutationRate = 1500;
        public static int RedRangeMax = 255;
        public static int RedRangeMin;
        public static int RedMutationRate = 1500;

        public void init()
        {
            red = getRandomNumber(0, 255);
            green = getRandomNumber(0, 255);
            blue = getRandomNumber(0, 255);
            alpha = getRandomNumber(10, 60);
        }


        public MyDnaBrush reproduction()
        {
            return new MyDnaBrush
                       {
                           alpha = alpha,
                           blue = blue,
                           green = green,
                           red = red,
                       };
        }

        /// <summary>
        /// mutation of colors for brush
        /// </summary>
        /// <param name="drawing"></param>
        public void mutatation(MyDnaDrawing drawing)
        {
            if (mutate(RedMutationRate))
            {
                red = getRandomNumber(RedRangeMin, RedRangeMax);
                drawing.SetDirty();
            }

            if (mutate(GreenMutationRate))
            {
                green = getRandomNumber(GreenRangeMin, GreenRangeMax);
                drawing.SetDirty();
            }

            if (mutate(BlueMutationRate))
            {
                blue = getRandomNumber(BlueRangeMin, BlueRangeMax);
                drawing.SetDirty();
            }

            if (mutate(AlphaMutationRate))
            {
                alpha = getRandomNumber(AlphaRangeMin, AlphaRangeMax);
                drawing.SetDirty();
            }
        }
        public static int getRandomNumber(int min, int max)
        {
            return random.Next(min, max);
        }

        //mutation factor using random 
        public static bool mutate(int mutationRate)
        {
            if (getRandomNumber(0, mutationRate) == 1)
                return true;
            return false;
        }
    }
}