// Copyright (c) 2008-2018 Matthias Lukaseder

namespace YAFBCore.Utils
{
    /// <summary>
    /// This class can transform a coordinate from a source to a target view
    /// </summary>
    public class Transformator
    {
        private float m;
        private float b;
        private float t;

        public Transformator(float sourceLow, float sourceHigh, float targetLow, float targetHigh)
        {
            m = (targetHigh - targetLow) / (sourceHigh - sourceLow);
            b = -sourceLow;
            t = targetLow;
        }

        /// <summary>
        /// Gets the coordinate transformed onto the target
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public float this[float v]
        {
            get
            {
                return ((v + b) * m + t);
            }
        }

        public float Rev(float v)
        {
            return 0 - (t - v) / m - b;
        }

        public float Prop(float v)
        {
            return (v * m);
        }

        public float RevProp(float v)
        {
            return (v / m);
        }
    }
}
