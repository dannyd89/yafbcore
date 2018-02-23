using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlattiBase.Mapping.MapUnits
{
    public enum MapUnitKind
    {
        //
        // Zusammenfassung:
        //     Unknown kind.
        Unknown = -1,
        //
        // Zusammenfassung:
        //     A sun.
        Sun = 1,
        //
        // Zusammenfassung:
        //     A planet.
        Planet = 2,
        //
        // Zusammenfassung:
        //     A moon.
        Moon = 3,
        //
        // Zusammenfassung:
        //     A meteoroid.
        Meteoroid = 4,
        //
        // Zusammenfassung:
        //     A buoy.
        Buoy = 5,
        //
        // Zusammenfassung:
        //     A Nebula.
        Nebula = 24,
        //
        // Zusammenfassung:
        //     A black hole.
        BlackHole = 32,
        //
        // Zusammenfassung:
        //     A wormhole.
        WormHole = 33,
        //
        // Zusammenfassung:
        //     A mission target.
        MissionTarget = 48,
        //
        // Zusammenfassung:
        //     A player ship.
        PlayerShip = 64,
        //
        // Zusammenfassung:
        //     A player platform.
        PlayerPlatform = 65,
        //
        // Zusammenfassung:
        //     A player probe.
        PlayerProbe = 66,
        //
        // Zusammenfassung:
        //     A player drone.
        PlayerDrone = 67,
        //
        // Zusammenfassung:
        //     A player base.
        PlayerBase = 68,
        //
        // Zusammenfassung:
        //     A switch.
        Switch = 96,
        //
        // Zusammenfassung:
        //     A gate.
        Gate = 97,
        //
        // Zusammenfassung:
        //     A plasma-storm.
        Storm = 98,
        //
        // Zusammenfassung:
        //     A solid storm-whirl.
        StormWhirl = 99,
        //
        // Zusammenfassung:
        //     A commencing storm-whirl, which will become a StormWhirl.
        StormCommencingWhirl = 100,
        //
        // Zusammenfassung:
        //     A Pixel.
        Pixel = 104,
        //
        // Zusammenfassung:
        //     A cluster of 16x16 pixels.
        PixelCluster = 105,
        //
        // Zusammenfassung:
        //     An energy power-up.
        EnergyPowerUp = 112,
        //
        // Zusammenfassung:
        //     A particles power-up.
        ParticlesPowerUp = 113,
        //
        // Zusammenfassung:
        //     A ions power-up.
        IonsPowerUp = 114,
        //
        // Zusammenfassung:
        //     A hull-repair power-up.
        HullPowerUp = 115,
        //
        // Zusammenfassung:
        //     A shields restore power-up.
        ShieldPowerUp = 116,
        //
        // Zusammenfassung:
        //     A shot production power-up.
        ShotProductionPowerUp = 117,
        //
        // Zusammenfassung:
        //     A total-refresh power-up.
        TotalRefreshPowerUp = 120,
        //
        // Zusammenfassung:
        //     A haste power-up.
        HastePowerUp = 121,
        //
        // Zusammenfassung:
        //     A double damage power-up.
        DoubleDamagePowerUp = 122,
        //
        // Zusammenfassung:
        //     A quad damage power-up.
        QuadDamagePowerUp = 123,
        //
        // Zusammenfassung:
        //     A cloak power-up.
        CloakPowerUp = 124,
        //
        // Zusammenfassung:
        //     A Spawner. This is an internal unit for adminstrative purpose. This unit will
        //     never be received by a regular player.
        Spawner = 127,
        //
        // Zusammenfassung:
        //     An explosion.
        Explosion = 128,
        //
        // Zusammenfassung:
        //     A shot.
        Shot = 129,
        //
        // Zusammenfassung:
        //     A space jellyfish.
        SpaceJellyFish = 160,
        //
        // Zusammenfassung:
        //     A space jellyfish slime.
        SpaceJellyFishSlime = 161,
        //
        // Zusammenfassung:
        //     An asteroid.
        Asteroid = 162,
        //
        // Zusammenfassung:
        //     An AI-ship.
        AIShip = 168,
        //
        // Zusammenfassung:
        //     An AI-platform.
        AIPlatform = 169,
        //
        // Zusammenfassung:
        //     An AI-probe.
        AIProbe = 170,
        //
        // Zusammenfassung:
        //     An AI-drone.
        AIDrone = 171,
        //
        // Zusammenfassung:
        //     An AI-base.
        AIBase = 172,

        TempPlayerShip = 999

    }
}
