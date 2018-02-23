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

    public class MapUnitExplosion : MapUnit
    {
        public Explosion explosion;

        public MapUnitExplosion(FlattiBase.Screens.Screen screen, Explosion explosion, Vector movementOffset)
            : base(screen, explosion, movementOffset)
        {
            this.explosion = explosion;
        }

        public override MapUnitKind Kind
        {
            get
            {
                return MapUnitKind.Explosion;
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
            float animatedRadius = (radius * Age) / AgeMax;

            // Draw Explosion
            Circle.Draw(renderTarget,
                        SolidColorBrushes.DarkRed,
                        new Vector2(X[Position.X], Y[Position.Y]),
                        X.Prop(animatedRadius));

            //Console.WriteLine("Hull Dmg: " + explosion.DamageHull);
            //Console.WriteLine("Energy Dmg: " + explosion.DamageEnergy);
            //Console.WriteLine("Shield Dmg: " + explosion.DamageShield);
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
