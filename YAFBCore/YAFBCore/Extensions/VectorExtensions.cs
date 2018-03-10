using Flattiverse;
using System;
using System.Collections.Generic;
using System.Text;

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
            return v.X == 0 && v.Y == 0 && v.Length == 0 && v.Angle == 0;
        }
    }
}
