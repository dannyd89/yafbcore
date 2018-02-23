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

    public class MapUnitJellyfish : MapUnit
    {
        public SpaceJellyFish spaceJellyFish;

        public MapUnitJellyfish(FlattiBase.Screens.Screen screen, SpaceJellyFish spaceJellyFish, Vector movementOffset)
            : base(screen, spaceJellyFish, movementOffset)
        {
            this.spaceJellyFish = spaceJellyFish;
        }

        public override MapUnitKind Kind
        {
            get
            {
                return MapUnitKind.SpaceJellyFish;
            }
        }

        public override MapUnitMobility Mobility
        {
            get
            {
                return MapUnitMobility.Mobile;
            }
        }

        public override int AgeMax
        {
            get
            {
                return 4;
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
                        SolidColorBrushes.LimeGreen,
                        new Vector2(X[Position.X], Y[Position.Y]),
                        X.Prop(Radius));
        }

        public override bool Calculate(int tickCount = 0)
        {
            throw new NotImplementedException();
        }

        public override bool AgeUnit(Map map)
        {
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
