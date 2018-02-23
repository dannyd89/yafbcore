using FlattiBase.Helper;
using SharpDX.Direct2D1;

namespace FlattiBase.Interfaces
{
    /// <summary>
    /// Used for fixed non-scaled drawables 
    /// </summary>
    interface IFixedDrawable
    {
        void Draw(RenderTarget renderTarget);
    }
}
