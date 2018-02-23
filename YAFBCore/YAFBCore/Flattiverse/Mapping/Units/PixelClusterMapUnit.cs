using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Mapping.Units
{
	public class PixelClusterMapUnit : MapUnit
	{
		private PixelCluster pixelCluster;
	
		public PixelClusterMapUnit(Map map, PixelCluster pixelCluster, Vector movementOffset)
			: base(map, pixelCluster, movementOffset)
		{
			this.pixelCluster = pixelCluster;
		}
	}
}
