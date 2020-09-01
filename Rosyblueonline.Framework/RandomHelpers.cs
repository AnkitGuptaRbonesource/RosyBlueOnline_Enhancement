using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Framework
{
    public sealed class RandomHelpers
    {
        //int Min = 0, Max = 0;
        Random random = null;
        private static readonly RandomHelpers instance = new RandomHelpers();

        private RandomHelpers()
        {
            random = new Random();
        }

        public static RandomHelpers Instance
        {
            get
            {
                return instance;
            }
        }

        public int RandomNumber(int min, int max)
        {
            return random.Next(min, max);
        }

        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

    }
}
