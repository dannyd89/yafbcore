using System;
using System.Collections.Generic;
using System.Text;
using YAFBCore.Utils.Mathematics;

namespace YAFBCore.Flattiverse.Mapping
{
    /// <summary>
    /// Describes a section in a map
    /// A map is divided in sections for faster path finding etc
    /// </summary>
    internal class MapSection
    {
        public readonly Map Parent;
        public readonly RectangleF Bounds;

        /// <summary>
        /// Creates a map section
        /// </summary>
        /// <param name="parent">Parent of this section</param>
        /// <param name="bounds">Size of this section</param>
        public MapSection(Map parent, RectangleF bounds)
        {
            Parent = parent;
            Bounds = bounds;
        }
    }
}
