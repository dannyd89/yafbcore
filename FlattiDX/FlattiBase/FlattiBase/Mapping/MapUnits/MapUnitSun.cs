using FlattiBase.Brushes;
using FlattiBase.Colors;
using FlattiBase.Helper;
using FlattiBase.Simples;
using Flattiverse;
using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FlattiBase.Mapping.MapUnits
{

    public class MapUnitSun : MapUnit
    {
        public Sun sun;

        private SharpDX.DirectWrite.TextLayout textLayout;

        private float biggestRadius;

        public MapUnitSun(FlattiBase.Screens.Screen screen, Sun sun, Vector movementOffset)
            : base(screen, sun, movementOffset)
        {
            this.sun = sun;

            textLayout = new SharpDX.DirectWrite.TextLayout(screen.Parent.DirectWriteFactory,
                                                            sun.Name,
                                                            FlattiBase.Fonts.FormFonts.SmallTextFont,
                                                            100f, 15f);

            foreach (Corona corona in sun.Coronas)
                if (corona.Radius > biggestRadius)
                    biggestRadius = corona.Radius;
        }

        //public override float Radius
        //{
        //    get
        //    {
        //        return biggestRadius;
        //    }
        //}

        public override MapUnitKind Kind
        {
            get
            {
                return MapUnitKind.Sun;
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
            Vector2 position = new Vector2(X[Position.X], Y[Position.Y]);

            foreach (Corona corona in sun.Coronas)
                Circle.Draw(renderTarget,
                            SolidColorBrushes.LightYellow,
                            position,
                            X.Prop(corona.Radius), X.Prop((corona.Energy * 25f) / 1000f));

            // Draw Planet
            Circle.Draw(renderTarget,
                        SolidColorBrushes.OrangeRed,
                        position,
                        X.Prop(radius));

            if (!textLayout.IsDisposed)
            {
                float halfWidth = textLayout.Metrics.Width / 2f;
                float halfHeight = textLayout.Metrics.Height / 2f;
                renderTarget.DrawTextLayout(new Vector2(X[Position.X] - halfWidth, Y[Position.Y] - halfHeight),
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
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            textLayout.Dispose();
        }
    }
}
