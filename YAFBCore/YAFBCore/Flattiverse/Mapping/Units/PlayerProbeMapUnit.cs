using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Mapping.Units
{
	public class PlayerProbeMapUnit : MapUnit
	{
		private PlayerProbe playerProbe;
	
		public PlayerProbeMapUnit(Map map, PlayerProbe playerProbe, Vector movementOffset)
			: base(map, playerProbe, movementOffset)
		{
			this.playerProbe = playerProbe;
		}
	}
}
