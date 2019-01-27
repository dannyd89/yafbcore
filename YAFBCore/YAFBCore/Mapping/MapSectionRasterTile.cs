using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YAFBCore.Mapping
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal sealed class MapSectionRasterTile
    {
        /// <summary>
        /// Status of this tile
        /// </summary>
        public MapSectionRasterTileStatus Status;
        /// <summary>
        /// Weight the tile has for path finding
        /// Higher weight is more useful
        /// </summary>
        public short Weight;
        /// <summary>
        /// X of the parent
        /// </summary>
        public ushort ParentX;
        /// <summary>
        /// Y of the parent
        /// </summary>
        public ushort ParentY;
        /// <summary>
        /// X Position of this tile in the raster
        /// TODO: See if this is really needed
        /// </summary>
        public float X;
        /// <summary>
        /// Y Position of this tile in the raster
        /// TODO: See if this is really needed
        /// </summary>
        public float Y;
        /// <summary>
        /// How many tiles already were checked to come here
        /// </summary>
        public int Gone;
        /// <summary>
        /// Sum of Gone + Weight + Heuristic
        /// The Priority to insert this node at.  Must be set BEFORE adding a node to the queue (ideally just once, in the node's constructor).
        /// Should not be manually edited once the node has been enqueued - use queue.UpdatePriority() instead
        /// </summary>
        public float Priority;
        /// <summary>
        /// Represents the current position in the queue
        /// </summary>
        public int QueueIndex;
#if DEBUG
        /// <summary>
        /// The queue this node is tied to. Used only for debug builds.
        /// </summary>
        public object Queue;
#endif
    }
}
