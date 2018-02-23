using FlattiBase.Colors;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Brushes
{
    public static class SolidColorBrushes
    {
        #region No Transparency

        #region White
        private static SolidColorBrush white;

        public static SolidColorBrush White
        {
            get
            {
                return SolidColorBrushes.white;
            }
        }
        #endregion

        #region Black
        private static SolidColorBrush black;

        public static SolidColorBrush Black
        {
            get
            {
                return SolidColorBrushes.black;
            }
        }
        #endregion

        #region Yellow
        private static SolidColorBrush yellow;

        public static SolidColorBrush Yellow
        {
            get
            {
                return SolidColorBrushes.yellow;
            }
        }
        #endregion

        #region Red
        private static SolidColorBrush red;

        public static SolidColorBrush Red
        {
            get
            {
                return SolidColorBrushes.red;
            }
        }
        #endregion

        #region Lightgray
        private static SolidColorBrush lightGray;

        public static SolidColorBrush LightGray
        {
            get
            {
                return SolidColorBrushes.lightGray;
            }
        }
        #endregion

        #region BlueViolet
        private static SolidColorBrush blueViolet;

        public static SolidColorBrush BlueViolet
        {
            get
            {
                return blueViolet;
            }
        }
        #endregion

        #region Violet
        private static SolidColorBrush violet;

        public static SolidColorBrush Violet
        {
            get
            {
                return violet;
            }
        }
        #endregion

        #region LightSeaGreen
        private static SolidColorBrush lightSeaGreen;

        public static SolidColorBrush LightSeaGreen
        {
            get
            {
                return SolidColorBrushes.lightSeaGreen;
            }
        }
        #endregion

        #region DarkGreen
        private static SolidColorBrush darkGreen;

        public static SolidColorBrush DarkGreen
        {
            get
            {
                return SolidColorBrushes.darkGreen;
            }
        }
        #endregion

        #region OrangeRed
        private static SolidColorBrush orangeRed;

        public static SolidColorBrush OrangeRed
        {
            get
            {
                return SolidColorBrushes.orangeRed;
            }
        }
        #endregion

        #region LightYellow
        private static SolidColorBrush lightYellow;

        public static SolidColorBrush LightYellow
        {
            get
            {
                return SolidColorBrushes.lightYellow;
            }
        }
        #endregion

        #region DarkRed
        private static SolidColorBrush darkRed;

        public static SolidColorBrush DarkRed
        {
            get
            {
                return SolidColorBrushes.darkRed;
            }
        }
        #endregion

        #region LightBlue
        private static SolidColorBrush lightBlue;

        public static SolidColorBrush LightBlue
        {
            get
            {
                return SolidColorBrushes.lightBlue;
            }
        }
        #endregion

        #region CadetBlue
        private static SolidColorBrush cadetBlue;

        public static SolidColorBrush CadetBlue
        {
            get
            {
                return SolidColorBrushes.cadetBlue;
            }
        }
        #endregion

        #region LightGoldenrodYellow
        private static SolidColorBrush lightGoldenrodYellow;

        public static SolidColorBrush LightGoldenrodYellow
        {
            get
            {
                return SolidColorBrushes.lightGoldenrodYellow;
            }
        }
        #endregion

        #region IndianRed
        private static SolidColorBrush indianRed;

        public static SolidColorBrush IndianRed
        {
            get
            {
                return SolidColorBrushes.indianRed;
            }
        }
        #endregion

        #region DarkOliveGreen
        private static SolidColorBrush darkOliveGreen;

        public static SolidColorBrush DarkOliveGreen
        {
            get
            {
                return SolidColorBrushes.darkOliveGreen;
            }
        }
        #endregion

        #region GreenYellow
        private static SolidColorBrush greenYellow;

        public static SolidColorBrush GreenYellow
        {
            get
            {
                return SolidColorBrushes.greenYellow;
            }
        }
        #endregion

        #region RosyBrown
        private static SolidColorBrush rosyBrown;

        public static SolidColorBrush RosyBrown
        {
            get
            {
                return SolidColorBrushes.rosyBrown;
            }
        }
        #endregion

        #region LimeGreen
        private static SolidColorBrush limeGreen;

        public static SolidColorBrush LimeGreen
        {
            get
            {
                return SolidColorBrushes.limeGreen;
            }
        }
        #endregion

        #region LimeGreen
        private static SolidColorBrush lightPink;

        public static SolidColorBrush LightPink
        {
            get
            {
                return SolidColorBrushes.lightPink;
            }
        }
        #endregion

        #endregion

        #region Halftransparency

        #region DarkGrayHalfTransparent
        private static SolidColorBrush blackHalfTransparent;

        public static SolidColorBrush BlackHalfTransparent
        {
            get
            {
                return SolidColorBrushes.blackHalfTransparent;
            }
        }
        #endregion

        #region DarkGrayHalfTransparent
        private static SolidColorBrush darkGrayHalfTransparent;

        public static SolidColorBrush DarkGrayHalfTransparent
        {
            get
            {
                return SolidColorBrushes.darkGrayHalfTransparent;
            }
        }
        #endregion

        #endregion

        public static Dictionary<string, SolidColorBrush> TeamColors;

        /// <summary>
        /// Inits all colors
        /// </summary>
        /// <param name="renderTarget"></param>
        public static void Init(WindowRenderTarget renderTarget)
        {
            red = new SolidColorBrush(renderTarget, AdvancedColors.Red);
            black = new SolidColorBrush(renderTarget, AdvancedColors.Black);
            white = new SolidColorBrush(renderTarget, AdvancedColors.White);
            yellow = new SolidColorBrush(renderTarget, AdvancedColors.Yellow);
            lightGray = new SolidColorBrush(renderTarget, AdvancedColors.LightGray);
            blueViolet = new SolidColorBrush(renderTarget, AdvancedColors.BlueViolet);
            violet = new SolidColorBrush(renderTarget, AdvancedColors.Violet);
            lightSeaGreen = new SolidColorBrush(renderTarget, AdvancedColors.LightSeaGreen);
            darkGreen = new SolidColorBrush(renderTarget, AdvancedColors.DarkGreen);
            orangeRed = new SolidColorBrush(renderTarget, AdvancedColors.OrangeRed);
            lightYellow = new SolidColorBrush(renderTarget, AdvancedColors.LightYellow);
            darkRed = new SolidColorBrush(renderTarget, AdvancedColors.DarkRed);
            lightBlue = new SolidColorBrush(renderTarget, AdvancedColors.LightBlue);
            cadetBlue = new SolidColorBrush(renderTarget, AdvancedColors.CadetBlue);
            lightGoldenrodYellow = new SolidColorBrush(renderTarget, AdvancedColors.LightGoldenrodYellow);
            indianRed = new SolidColorBrush(renderTarget, AdvancedColors.IndianRed);
            darkOliveGreen = new SolidColorBrush(renderTarget, AdvancedColors.DarkOliveGreen);
            greenYellow = new SolidColorBrush(renderTarget, AdvancedColors.GreenYellow);
            rosyBrown = new SolidColorBrush(renderTarget, AdvancedColors.RosyBrown);
            limeGreen = new SolidColorBrush(renderTarget, AdvancedColors.LimeGreen);
            lightPink = new SolidColorBrush(renderTarget, AdvancedColors.LightPink);

            blackHalfTransparent = new SolidColorBrush(renderTarget, AdvancedColors.BlackHalfTransparent);
            darkGrayHalfTransparent = new SolidColorBrush(renderTarget, AdvancedColors.DarkGrayHalfTransparent);

            TeamColors = new Dictionary<string, SolidColorBrush>();
        }
    }
}
