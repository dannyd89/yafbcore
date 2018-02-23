using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Helper
{
    public static class ImageCollections
    {
        public static Dictionary<string, SharpDX.Direct2D1.Bitmap> PlayersSmallAvatars = new Dictionary<string, SharpDX.Direct2D1.Bitmap>();

        public static SharpDX.Direct2D1.Bitmap PlayerDefaultSmallAvatar;
    }
}
