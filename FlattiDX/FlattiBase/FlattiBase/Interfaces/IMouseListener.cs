
namespace FlattiBase.Interfaces
{
    public interface IMouseListener
    {
        void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e);

        void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e);

        void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e);
    }
}
