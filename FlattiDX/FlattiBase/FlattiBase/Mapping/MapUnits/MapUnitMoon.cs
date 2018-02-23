using FlattiBase.Brushes;
using FlattiBase.Helper;
using FlattiBase.Simples;
using Flattiverse;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FlattiBase.Mapping.MapUnits
{

    public class MapUnitMoon : MapUnit
    {
        public Moon moon;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="moon"></param>
        /// <param name="movementOffset"></param>
        public MapUnitMoon(FlattiBase.Screens.Screen screen, Moon moon, Vector movementOffset)
            : base(screen, moon, movementOffset)
        {
            this.moon = moon;
        }

        /// <summary>
        /// 
        /// </summary>
        public override MapUnitKind Kind
        {
            get
            {
                return MapUnitKind.Moon;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override MapUnitMobility Mobility
        {
            get
            {
                return moon.IsOrbiting ? MapUnitMobility.Steady : MapUnitMobility.Still;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int AgeMax
        {
            get
            {
                return int.MaxValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool HasAging
        {
            get
            {
                return moon.IsOrbiting;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="renderTarget"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public override void Draw(SharpDX.Direct2D1.WindowRenderTarget renderTarget, Transformator X, Transformator Y)
        {
            Circle.Draw(renderTarget,
                        SolidColorBrushes.LightGray,
                        new Vector2(X[Position.X], Y[Position.Y]),
                        X.Prop(Radius));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tickCount"></param>
        /// <returns></returns>
        public override bool Calculate(int tickCount = 0)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public override bool AgeUnit(Map map)
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

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            
        }
    }
}
