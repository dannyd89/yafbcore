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

    public class MapUnitAsteroid : MapUnit
    {
        public Asteroid asteroid;

        public MapUnitAsteroid(FlattiBase.Screens.Screen screen, Asteroid asteroid, Vector movementOffset)
            : base(screen, asteroid, movementOffset)
        {
            this.asteroid = asteroid;
        }

        public override MapUnitKind Kind
        {
            get
            {
                return MapUnitKind.Asteroid;
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
                return 3;
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
                        SolidColorBrushes.LightPink,
                        new Vector2(X[Position.X], Y[Position.Y]),
                        X.Prop(Radius));
        }

        public override bool Calculate(int tickCount = 0)
        {
            throw new NotImplementedException();
        }

        public override bool AgeUnit(Map map)
        {
            Position += Movement;

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
