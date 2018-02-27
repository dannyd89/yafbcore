using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YAFBCore.Flattiverse.Mapping.Units
{ 
    public static class MapUnitFactory
    {
        public static MapUnit GetMapUnit(Map map, Unit unit, Vector movementOffset)
        {
            switch (unit.Kind)
            {
                case UnitKind.Asteroid:
                    return new AsteroidMapUnit(map, (Asteroid)unit, movementOffset);
                case UnitKind.BlackHole:
                    return new BlackHoleMapUnit(map, (BlackHole)unit, movementOffset);
                case UnitKind.Buoy:
                    return new BuoyMapUnit(map, (Buoy)unit, movementOffset);
                case UnitKind.CloakPowerUp:
                    return new CloakPowerUpMapUnit(map, (CloakPowerUp)unit, movementOffset);
                case UnitKind.DoubleDamagePowerUp:
                    return new DoubleDamagePowerUpMapUnit(map, (DoubleDamagePowerUp)unit, movementOffset);
                case UnitKind.EnergyPowerUp:
                    return new EnergyPowerUpMapUnit(map, (EnergyRefreshingPowerUp)unit, movementOffset);
                case UnitKind.Explosion:
                    return new ExplosionMapUnit(map, (Explosion)unit, movementOffset);
                case UnitKind.Gate:
                    return new GateMapUnit(map, (Gate)unit, movementOffset);
                case UnitKind.HastePowerUp:
                    return new HastePowerUpMapUnit(map, (HastePowerUp)unit, movementOffset);
                case UnitKind.HullPowerUp:
                    return new HullPowerUpMapUnit(map, (HullRefreshingPowerUp)unit, movementOffset);
                case UnitKind.IonsPowerUp:
                    return new IonsPowerUpMapUnit(map, (IonsRefreshingPowerUp)unit, movementOffset);
                case UnitKind.Meteoroid:
                    return new MeteoroidMapUnit(map, (Meteoroid)unit, movementOffset);
                case UnitKind.MissionTarget:
                    return new MissionTargetMapUnit(map, (MissionTarget)unit, movementOffset);
                case UnitKind.Moon:
                    return new MoonMapUnit(map, (Moon)unit, movementOffset);
                case UnitKind.ParticlesPowerUp:
                    return new ParticlesPowerUpMapUnit(map, (ParticlesRefreshingPowerUp)unit, movementOffset);
                case UnitKind.Planet:
                    return new PlanetMapUnit(map, (Planet)unit, movementOffset);
                case UnitKind.PlayerBase:
                    return new PlayerBaseMapUnit(map, (PlayerBase)unit, movementOffset);
                case UnitKind.PlayerDrone:
                    return new PlayerDroneMapUnit(map, (PlayerDrone)unit, movementOffset);
                case UnitKind.PlayerPlatform:
                    return new PlayerPlatformMapUnit(map, (PlayerPlatform)unit, movementOffset);
                case UnitKind.PlayerProbe:
                    return new PlayerProbeMapUnit(map, (PlayerProbe)unit, movementOffset);
                case UnitKind.PlayerShip:
                    return new PlayerShipMapUnit(map, (PlayerShip)unit, movementOffset);                
                case UnitKind.QuadDamagePowerUp:
                    return new QuadDamagePowerUpMapUnit(map, (QuadDamagePowerUp)unit, movementOffset);
                case UnitKind.ShieldPowerUp:
                    return new ShieldPowerUpMapUnit(map, (ShieldRefreshingPowerUp)unit, movementOffset);
                case UnitKind.Shot:
                    return new ShotMapUnit(map, (Shot)unit, movementOffset);
                case UnitKind.ShotProductionPowerUp:
                    return new ShotProductionPowerUpMapUnit(map, (ShotProductionRefreshingPowerUp)unit, movementOffset);
                case UnitKind.SpaceJellyFish:
                    return new SpaceJellyFishMapUnit(map, (SpaceJellyFish)unit, movementOffset);
                case UnitKind.SpaceJellyFishSlime:
                    return new SpaceJellyFishSlimeMapUnit(map, (SpaceJellyFishSlime)unit, movementOffset);
                case UnitKind.Sun:
                    return new SunMapUnit(map, (Sun)unit, movementOffset);
                case UnitKind.Switch:
                    return new SwitchMapUnit(map, (Switch)unit, movementOffset);
                case UnitKind.TotalRefreshPowerUp:
                    return new TotalRefreshPowerUpMapUnit(map, (TotalRefreshingPowerUp)unit, movementOffset);
                case UnitKind.WormHole:
                    return new WormHoleMapUnit(map, (WormHole)unit, movementOffset);
                default:
                    return new UnknownMapUnit(map, unit, movementOffset);;
            }
        }

        public static MapUnit GetMapUnit(Map map, Controllable controllable, Vector movementOffset)
        {
#warning TODO: Hier muss noch eine Unterscheidung rein für die verschiedenen Typen an Controllables
            return new PlayerShipMapUnit(map, controllable, movementOffset);
        }
    }
}
