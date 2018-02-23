using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Mapping.Units
{
	public class PixelMapUnit : MapUnit
	{
		private Pixel pixel;
	
		public PixelMapUnit(Map map, Pixel pixel, Vector movementOffset)
			: base(map, pixel, movementOffset)
		{
			this.pixel = pixel;
		}
	}
}
