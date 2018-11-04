using Flattiverse;
using System;
using System.Collections.Generic;
using System.Text;
using YAFBCore.Utils.Mathematics;

namespace YAFBCore.Extensions
{
    internal static class VectorExtensions
    {
        /// <summary>
        /// Checks if vector is a zero vector
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool IsZeroVector(this Vector v)
        {
            return MathUtil.NearEqual(v.X + v.Y, 0f);
        }
    }
}
