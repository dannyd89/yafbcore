using Flattiverse;
using JARVIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlattiBase.Mapping.MapUnits
{
    public static class MapUnitFactory
    {
        public static MapUnit GetMapUnit(FlattiBase.Screens.Screen screen, Unit unit, Vector movementOffset)
        {
            switch (unit.Kind)
            {
                case UnitKind.Asteroid:
                    return new MapUnitAsteroid(screen, (Asteroid)unit, movementOffset);
                case UnitKind.BlackHole:
                    return new MapUnitBlackhole(screen, (BlackHole)unit, movementOffset);
                case UnitKind.Buoy:
                    return new MapUnitBuoy(screen, (Buoy)unit, movementOffset);
                case UnitKind.CloakPowerUp:
                    return new MapUnitCloakPowerUp(screen, (CloakPowerUp)unit, movementOffset);
                case UnitKind.DoubleDamagePowerUp:
                    return new MapUnitDoubleDamagePowerUp(screen, (DoubleDamagePowerUp)unit, movementOffset);
                case UnitKind.EnergyPowerUp:
                    return new MapUnitEnergyPowerUp(screen, (EnergyRefreshingPowerUp)unit, movementOffset);
                case UnitKind.Explosion:
                    return new MapUnitExplosion(screen, (Explosion)unit, movementOffset);
                case UnitKind.Gate:
                    return new MapUnitGate(screen, (Gate)unit, movementOffset);
                case UnitKind.HastePowerUp:
                    return new MapUnitHastePowerUp(screen, (HastePowerUp)unit, movementOffset);
                case UnitKind.HullPowerUp:
                    return new MapUnitHullPowerUp(screen, (HullRefreshingPowerUp)unit, movementOffset);
                case UnitKind.IonsPowerUp:
                    return new MapUnitIonsPowerUp(screen, (IonsRefreshingPowerUp)unit, movementOffset);
                case UnitKind.Meteoroid:
                    return new MapUnitMeteoroid(screen, (Meteoroid)unit, movementOffset);
                case UnitKind.MissionTarget:
                    return new MapUnitMissionTarget(screen, (MissionTarget)unit, movementOffset);
                case UnitKind.Moon:
                    return new MapUnitMoon(screen, (Moon)unit, movementOffset);
                case UnitKind.ParticlesPowerUp:
                    return new MapUnitParticlesPowerUp(screen, (ParticlesRefreshingPowerUp)unit, movementOffset);
                case UnitKind.Planet:
                    return new MapUnitPlanet(screen, (Planet)unit, movementOffset);
                case UnitKind.PlayerBase:
                    return new MapUnitPlayerBase(screen, (PlayerBase)unit, movementOffset);
                case UnitKind.PlayerDrone:
                    return new MapUnitPlayerDrone(screen, (PlayerDrone)unit, movementOffset);
                case UnitKind.PlayerPlatform:
                    return new MapUnitPlayerPlatform(screen, (PlayerPlatform)unit, movementOffset);
                case UnitKind.PlayerProbe:
                    return new MapUnitPlayerProbe(screen, (PlayerProbe)unit, movementOffset);
                case UnitKind.PlayerShip:
                    return new MapUnitPlayerShip(screen, (PlayerShip)unit, movementOffset);                
                case UnitKind.QuadDamagePowerUp:
                    return new MapUnitQuadDamagePowerUp(screen, (QuadDamagePowerUp)unit, movementOffset);
                case UnitKind.ShieldPowerUp:
                    return new MapUnitShieldPowerUp(screen, (ShieldRefreshingPowerUp)unit, movementOffset);
                case UnitKind.Shot:
                    return new MapUnitShot(screen, (Shot)unit, movementOffset);
                case UnitKind.ShotProductionPowerUp:
                    return new MapUnitShotProductionPowerUp(screen, (ShotProductionRefreshingPowerUp)unit, movementOffset);
                case UnitKind.SpaceJellyFish:
                    return new MapUnitJellyfish(screen, (SpaceJellyFish)unit, movementOffset);
                case UnitKind.SpaceJellyFishSlime:
                    return new MapUnitJellyfishShot(screen, (SpaceJellyFishSlime)unit, movementOffset);
                case UnitKind.Sun:
                    return new MapUnitSun(screen, (Sun)unit, movementOffset);
                case UnitKind.Switch:
                    return new MapUnitSwitch(screen, (Switch)unit, movementOffset);
                case UnitKind.TotalRefreshPowerUp:
                    return new MapUnitTotalRefreshPowerUp(screen, (TotalRefreshingPowerUp)unit, movementOffset);
                case UnitKind.WormHole:
                    return new MapUnitWormhole(screen, (WormHole)unit, movementOffset);
                default:
                    return new MapUnitDefault(screen, unit, movementOffset);;
            }
        }

        public static MapUnit GetMapUnit(FlattiBase.Screens.Screen screen, Controllable controllable, Vector movementOffset)
        {
            return new MapUnitPlayerShip(screen, controllable, movementOffset);
        }

        public static MapUnit GetMapUnit(FlattiBase.Screens.Screen screen, JUnit junit, Vector position, Vector movementOffset)
        {
            return new MapUnitPlayerShip(screen, junit, position, movementOffset);
        }
    }
}
