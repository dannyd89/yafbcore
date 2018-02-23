using FlattiBase.Brushes;
using FlattiBase.Colors;
using FlattiBase.Helper;
using FlattiBase.Simples;
using Flattiverse;
using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FlattiBase.Mapping.MapUnits
{

    public class MapUnitShot : MapUnit
    {
        public Shot Shot;

        private Vector CalculatedExplosion;

        public int Time;

        public MapUnitShot(FlattiBase.Screens.Screen screen, Shot shot, Vector movementOffset)
            : base(screen, shot, movementOffset)
        {
            this.Shot = shot;

            this.Time = shot.Time;

            this.radius = 10f;            
        }

        public override MapUnitKind Kind
        {
            get
            {
                return MapUnitKind.Shot;
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
            Circle.Fill(renderTarget,
                        SolidColorBrushes.Red,
                        new Vector2(X[Position.X], Y[Position.Y]),
                        X.Prop(Radius));

            CalculatedExplosion = Position + (Movement * Time);

            Circle.Draw(renderTarget,
                        SolidColorBrushes.LightGray,
                        new Vector2(X[CalculatedExplosion.X], Y[CalculatedExplosion.Y]),
                        X.Prop(Radius));

            //Console.WriteLine("Real shot movement: " + Movement.Length.ToString());

            //Console.WriteLine("Hull Dmg: " + shot.DamageHull);
            //Console.WriteLine("Energy Dmg: " + shot.DamageEnergy);
            //Console.WriteLine("Shield Dmg: " + shot.DamageShield);
        }

        public override bool Calculate(int tickCount = 0)
        {
            throw new NotImplementedException();
        }

        public override bool AgeUnit(Map map)
        {
            Position += Movement;

            if (Age++ < AgeMax)
            {
                Time--;
                return false;
            }

            return true;
        }

        public override void Dispose()
        {
            
        }
    }
}
