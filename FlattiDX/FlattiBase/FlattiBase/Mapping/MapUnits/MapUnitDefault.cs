using FlattiBase.Brushes;
using FlattiBase.Helper;
using FlattiBase.Simples;
using Flattiverse;
using SharpDX;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Mapping.MapUnits
{
    public class MapUnitDefault : MapUnit
    {
        public Unit unit;

        private TextLayout textLayout;

        public MapUnitDefault(FlattiBase.Screens.Screen screen, Unit unit, Vector movementOffset)
            : base(screen, unit, movementOffset)
        {
            this.unit = unit;

            textLayout = new TextLayout(screen.Parent.DirectWriteFactory, 
                                        "{[U] " + unit.Kind.ToString() + " } " + unit.Name, 
                                        FlattiBase.Fonts.FormFonts.SmallTextFont, 
                                        200f, 30f);
        }

        public override MapUnitKind Kind
        {
            get
            {
                return MapUnitKind.Unknown;
            }
        }

        public override MapUnitMobility Mobility
        {
            get
            {
                return internalMobility == Flattiverse.Mobility.Still ? MapUnitMobility.Still : internalMobility == Flattiverse.Mobility.Steady ? MapUnitMobility.Steady : MapUnitMobility.Mobile;

            }
        }

        public override int AgeMax
        {
            get
            {
                return 10;
            }
        }

        public override bool HasAging
        {
            get
            {
                return true;
            }
        }

        public override void Draw(SharpDX.Direct2D1.WindowRenderTarget renderTarget, Transformator X, Transformator Y)
        {
            Circle.Draw(renderTarget,
                        SolidColorBrushes.DarkGreen,
                        new Vector2(X[Position.X], Y[Position.Y]),
                        X.Prop(Radius));

            if (!textLayout.IsDisposed)
            {
                float halfWidth = textLayout.Metrics.Width / 2f;
                renderTarget.DrawTextLayout(new Vector2(X[Position.X] - halfWidth, Y[Position.Y] + Y.Prop(Radius + 2f)),
                                            textLayout,
                                            SolidColorBrushes.White, SharpDX.Direct2D1.DrawTextOptions.NoSnap);
            }
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

        public override void Dispose()
        {
            textLayout.Dispose();
        }
    }
}
