using System;

namespace YAFBCore.Utils
{
    public static class Helper
    {
        /// <summary>
        /// Schneidet einen String ab einer festgelegten Länge ab
        /// </summary>
        /// <param name="source"></param>
        /// <param name="maxLength"></param>
        /// <param name="appendOnOverflow"></param>
        /// <returns></returns>
        public static string TruncateString(string source, int maxLength, string appendOnOverflow)
        {
            if (source.Length > maxLength)
            {
                source = source.Substring(0, maxLength);

                if (appendOnOverflow != null)
                    source = source + appendOnOverflow;
            }

            return source;
        }
    }
}
