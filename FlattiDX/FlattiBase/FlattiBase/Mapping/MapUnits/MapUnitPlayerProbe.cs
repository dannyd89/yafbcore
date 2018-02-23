using FlattiBase.Brushes;
using FlattiBase.Helper;
using FlattiBase.Simples;
using Flattiverse;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FlattiBase.Mapping.MapUnits
{

    public class MapUnitPlayerProbe : MapUnit
    {
        public PlayerProbe playerProbe;

        public MapUnitPlayerProbe(FlattiBase.Screens.Screen screen, PlayerProbe playerProbe, Vector movementOffset)
            : base(screen, playerProbe, movementOffset)
        {
            this.playerProbe = playerProbe;
        }

        public override MapUnitKind Kind
        {
            get
            {
                return MapUnitKind.PlayerProbe;
            }
        }

        public override MapUnitMobility Mobility
        {
            get
            {
                return playerProbe.IsOrbiting ? MapUnitMobility.Steady : MapUnitMobility.Still;
            }
        }

        public override int AgeMax
        {
            get
            {
                return int.MaxValue;
            }
        }

        public override bool HasAging
        {
            get
            {
                return playerProbe.IsOrbiting;
            }
        }

        public override void Draw(SharpDX.Direct2D1.WindowRenderTarget renderTarget, Transformator X, Transformator Y)
        {
            // Draw Planet
            Circle.Draw(renderTarget,
                        SolidColorBrushes.DarkGreen,
                        new Vector2(X[Position.X], Y[Position.Y]),
                        X.Prop(Radius));
        }

        public override bool Calculate(int tickCount = 0)
        {
            throw new NotImplementedException();
        }

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
