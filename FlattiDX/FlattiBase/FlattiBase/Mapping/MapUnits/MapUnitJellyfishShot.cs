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

    public class MapUnitJellyfishShot : MapUnit
    {
        public SpaceJellyFishSlime spaceJellyFishSlime;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="spaceJellyFishSlime"></param>
        /// <param name="movementOffset"></param>
        public MapUnitJellyfishShot(FlattiBase.Screens.Screen screen, SpaceJellyFishSlime spaceJellyFishSlime, Vector movementOffset)
            : base(screen, spaceJellyFishSlime, movementOffset)
        {
            this.spaceJellyFishSlime = spaceJellyFishSlime;

            this.radius = 5f;
        }

        /// <summary>
        /// 
        /// </summary>
        public override MapUnitKind Kind
        {
            get
            {
                return MapUnitKind.SpaceJellyFishSlime;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override MapUnitMobility Mobility
        {
            get
            {
                return MapUnitMobility.Mobile;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int AgeMax
        {
            get
            {
                return 4;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool HasAging
        {
            get
            {
                return true;
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
                        SolidColorBrushes.LimeGreen,
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
