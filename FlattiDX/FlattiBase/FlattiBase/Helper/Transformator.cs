using System;
using System.Collections.Generic;
using System.Text;

namespace FlattiBase.Helper
{
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
