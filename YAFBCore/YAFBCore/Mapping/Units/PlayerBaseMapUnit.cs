using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class PlayerBaseMapUnit : MapUnit
	{
		private PlayerBase playerBase;
	
		public PlayerBaseMapUnit(Map map, PlayerBase playerBase, Vector movementOffset)
			: base(map, playerBase, movementOffset)
		{
			this.playerBase = playerBase;
		}
	}
}
