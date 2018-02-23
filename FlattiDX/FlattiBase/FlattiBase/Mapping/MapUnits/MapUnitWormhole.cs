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

    public class MapUnitWormhole : MapUnit
    {
        public WormHole wormHole;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="wormHole"></param>
        /// <param name="movementOffset"></param>
        public MapUnitWormhole(FlattiBase.Screens.Screen screen, WormHole wormHole, Vector movementOffset)
            : base(screen, wormHole, movementOffset)
        {
            this.wormHole = wormHole;
        }

        /// <summary>
        /// 
        /// </summary>
        public override MapUnitKind Kind
        {
            get
            {
                return MapUnitKind.WormHole;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override MapUnitMobility Mobility
        {
            get
            {
                return MapUnitMobility.Still;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int AgeMax
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool HasAging
        {
            get
            {
                return false;
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
            // Draw Planet
            Circle.Draw(renderTarget, 
                        SolidColorBrushes.BlueViolet, 
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
