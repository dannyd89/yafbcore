using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlattiBase.Helper;
using SharpDX.Direct2D1;
using FlattiBase.Simples;
using FlattiBase.Brushes;
using SharpDX;
using Flattiverse;
using System.Collections.ObjectModel;

namespace FlattiBase.Mapping.MapUnits
{
    public class MapUnitBlackhole : MapUnit
    {
        public BlackHole BlackHole;

        public ReadOnlyCollection<GravityWell> GravityWells;

        public MapUnitBlackhole(FlattiBase.Screens.Screen screen, BlackHole blackHole, Vector movementOffset)
            : base(screen, blackHole, movementOffset)
        {
            BlackHole = blackHole;
        }

        public override MapUnitKind Kind
        {
            get
            {
                return MapUnitKind.BlackHole;
            }
        }

        public override MapUnitMobility Mobility
        {
            get
            {
                return MapUnitMobility.Still;
            }
        }

        public override int AgeMax
        {
            get
            {
                return 0;
            }
        }

        public override bool HasAging
        {
            get
            {
                return false;
            }
        }

        public override bool Calculate(int tickCount = 0)
        {
            return true;
        }

        public override void Dispose()
        {
            
        }

        public override void Draw(WindowRenderTarget renderTarget, Transformator X, Transformator Y)
        {
            Circle.Draw(renderTarget,
                        SolidColorBrushes.Violet,
                        new Vector2(X[Position.X], Y[Position.Y]),
                        X.Prop(Radius));
        }
    }
}
