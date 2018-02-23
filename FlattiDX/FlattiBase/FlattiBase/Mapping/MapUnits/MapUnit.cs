using FlattiBase.Fonts;
using FlattiBase.Helper;
using FlattiBase.Screens;
using Flattiverse;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace FlattiBase.Mapping.MapUnits
{

    /// <summary>
    /// A Unit on a map with reference distances to other units
    /// </summary>
    public abstract class MapUnit : IDisposable
    {
        public readonly string Name;

        public readonly bool IsOrbiting;
        public readonly bool IsSolid;
        public readonly bool IsMasking;

        protected Mobility internalMobility;

        public float Gravity;

        public Vector Position;
        public Vector OrbitingCenter;
        public Vector Movement;

        public int Age;

        protected float radius;
        protected List<MapOrbitingState> orbitingList;
        
        protected Screen Screen;

        public MapUnit(Screen screen, Unit unit, Vector movementOffset)
        {
            Position = unit.Position;
            Movement = unit.Movement - movementOffset;
            radius = unit.Radius;
            Name = unit.Name;
            Screen = screen;

            Gravity = unit.Gravity;
            internalMobility = unit.Mobility;
            IsOrbiting = unit.IsOrbiting;
            IsMasking = unit.IsMasking;
            IsSolid = unit.IsSolid;

            if (unit.IsOrbiting)
            {
                orbitingList = new List<MapOrbitingState>();
                OrbitingCenter = unit.OrbitingCenter;

                foreach (OrbitingState os in unit.OrbitingList)
                    orbitingList.Add(new MapOrbitingState(os));
            }

            Age = 0;
        }

        public virtual void ParseMessage(FlattiverseMessage msg)
        { }

        public MapUnit(Screen screen, Vector position, Vector movement, float radius, string name)
        {
            Position = position;
            Movement = movement;
            this.radius = radius;
            Name = name;
            Screen = screen;
            Age = 0;
        }

        public virtual float Radius
        {
            get
            {
                return radius;
            }
        }

        public virtual MapUnitKind Kind
        {
            get
            {
                return MapUnitKind.Unknown;
            }
        }

        public virtual MapUnitMobility Mobility
        {
            get
            {
                return MapUnitMobility.Unkown;
            }
        }

        public virtual int AgeMax
        {
            get
            {
                return 0;
            }
        }

        public virtual bool HasAging
        {
            get
            {
                return false;
            }
        }

        public abstract void Draw(SharpDX.Direct2D1.WindowRenderTarget renderTarget, Transformator X, Transformator Y);

        public abstract bool Calculate(int tickCount = 0);

        public virtual bool AgeUnit(Map map)
        {
            if (IsOrbiting)
            {
                Vector orbitingCenter = new Vector(OrbitingCenter);

                foreach (MapOrbitingState os in orbitingList)
                {
                    os.Rotation = os.RotationInterval > 0 ? (os.Rotation + 1) % os.RotationInterval : os.Rotation - 1 <= os.RotationInterval ? 0 : os.Rotation - 1;

                    os.Angle = os.StartAngle + (360f * ((float)os.Rotation / os.RotationIntervalAbsolute));

                    orbitingCenter += Vector.FromAngleLength(os.Angle, os.Distance);
                }
                Position = orbitingCenter;
            }

            if (Age++ < AgeMax)
                return false;

            return true;
        }

        public abstract void Dispose();
    }
}
