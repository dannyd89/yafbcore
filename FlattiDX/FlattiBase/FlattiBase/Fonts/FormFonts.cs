using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Fonts
{
    public static class FormFonts
    {
        #region headline large font
        private static TextFormat headlineLargeFont;

        public static TextFormat HeadlineLargeFont
        {
            get
            {
                return headlineLargeFont;
            }
        }
        #endregion

        #region headline medium font
        private static TextFormat headlineMediumFont;

        public static TextFormat HeadlineMediumFont
        {
            get
            {
                return headlineMediumFont;
            }
        }
        #endregion

        #region headline small font
        private static TextFormat headlineSmallFont;

        public static TextFormat HeadlineSmallFont
        {
            get
            {
                return headlineSmallFont;
            }
        }
        #endregion

        #region column header font
        private static TextFormat columnHeaderFont;

        public static TextFormat ColumnHeaderFont
        {
            get
            {
                return columnHeaderFont;
            }
        }
        #endregion

        #region normal text font
        private static TextFormat normalTextFont;

        public static TextFormat NormalTextFont
        {
            get
            {
                return normalTextFont;
            }
        }
        #endregion

        #region small text font
        private static TextFormat smallTextFont;

        public static TextFormat SmallTextFont
        {
            get
            {
                return smallTextFont;
            }
        }
        #endregion

        #region Button normal font
        private static TextFormat buttonNormalFont;

        public static TextFormat ButtonNormalFont
        {
            get 
            {
                return buttonNormalFont; 
            }
        }
        #endregion

        public static void Init(SharpDX.DirectWrite.Factory directWriteFactory)
        {
            buttonNormalFont = new TextFormat(directWriteFactory, "Venera 700", 13f);
            headlineLargeFont = new TextFormat(directWriteFactory, "Futura", 68f);
            headlineMediumFont = new TextFormat(directWriteFactory, "Venera 900", 36f);
            headlineSmallFont = new TextFormat(directWriteFactory, "Venera 700", 20f);
            columnHeaderFont = new TextFormat(directWriteFactory, "Venera 700", 16f);
            normalTextFont = new TextFormat(directWriteFactory, "Venera 700", 14f);
            smallTextFont = new TextFormat(directWriteFactory, "Venera 500", 17f);

            //buttonNormalFont = new TextFormat(directWriteFactory, "Helvetica", 14f);
            //headlineLargeFont = new TextFormat(directWriteFactory, "Helvetica", 68f);
            //headlineMediumFont = new TextFormat(directWriteFactory, "Helvetica", 36f);
            //headlineSmallFont = new TextFormat(directWriteFactory, "Helvetica", 20f);
            //columnHeaderFont = new TextFormat(directWriteFactory, "Helvetica", 16f);
            //normalTextFont = new TextFormat(directWriteFactory, "Helvetica", 14f);
            //smallTextFont = new TextFormat(directWriteFactory, "Helvetica", 17f);
        }
    }
}
