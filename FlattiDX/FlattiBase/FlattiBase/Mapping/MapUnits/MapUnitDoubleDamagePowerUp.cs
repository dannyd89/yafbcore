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
    public class MapUnitDoubleDamagePowerUp : MapUnit
    {
        public DoubleDamagePowerUp doubleDamagePowerUp;

        public MapUnitDoubleDamagePowerUp(FlattiBase.Screens.Screen screen, DoubleDamagePowerUp doubleDamagePowerUp, Vector movementOffset)
            : base(screen, doubleDamagePowerUp, movementOffset)
        {
            this.doubleDamagePowerUp = doubleDamagePowerUp;

            this.radius = 5f;
        }

        public override MapUnitKind Kind
        {
            get
            {
                return MapUnitKind.DoubleDamagePowerUp;
            }
        }

        public override MapUnitMobility Mobility
        {
            get
            {
                return doubleDamagePowerUp.IsOrbiting ? MapUnitMobility.Steady : MapUnitMobility.Still;
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
                return doubleDamagePowerUp.IsOrbiting;
            }
        }

        public override void Draw(SharpDX.Direct2D1.WindowRenderTarget renderTarget, Transformator X, Transformator Y)
        {
            Circle.Draw(renderTarget,
                        SolidColorBrushes.LightBlue,
                        new Vector2(X[Position.X], Y[Position.Y]),
                        X.Prop(Radius), X.Prop(5f));

            //float halfWidth = TextLayout.Metrics.Width / 2f;
            //float halfHeight = TextLayout.Metrics.Height / 2f;
            //renderTarget.DrawTextLayout(new Vector2(X[Position.X] - halfWidth, Y[Position.Y] - halfHeight),
            //                            TextLayout,
            //                            SolidColorBrushes.LightBlue, SharpDX.Direct2D1.DrawTextOptions.NoSnap);
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
