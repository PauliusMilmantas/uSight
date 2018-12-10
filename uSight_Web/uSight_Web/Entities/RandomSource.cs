using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSight_Web.Entities
{
    public class RandomSource
    {
        private static Lazy<RandomSource> instance = new Lazy<RandomSource>(delegate () { return new RandomSource(); });
        public static RandomSource Instance { get { return instance.Value; } }

        Random rnd;

        private RandomSource()
        {
            rnd = new Random();
        }

        public int Next(int max)
        {
            return rnd.Next(max);
        }
    }
}