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

    public class MapUnitBuoy : MapUnit
    {
        public Buoy buoy;

        public MapUnitBuoy(FlattiBase.Screens.Screen screen, Buoy buoy, Vector movementOffset)
            : base(screen, buoy, movementOffset)
        {
            this.buoy = buoy;
        }

        public override MapUnitKind Kind
        {
            get
            {
                return MapUnitKind.Buoy;
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

        public override void Draw(SharpDX.Direct2D1.WindowRenderTarget renderTarget, Transformator X, Transformator Y)
        {
            Circle.Draw(renderTarget,
                        SolidColorBrushes.White,
                        new Vector2(X[Position.X], Y[Position.Y]),
                        X.Prop(Radius));
        }

        public override bool Calculate(int tickCount = 0)
        {
            throw new NotImplementedException();
        }

        public override bool AgeUnit(Map map)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {

        }
    }
}
