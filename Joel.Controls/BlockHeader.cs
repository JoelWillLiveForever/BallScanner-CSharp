using System.Windows;
using System.Windows.Controls;

namespace Joel.Controls
{
    public class BlockHeader : TextBox
    {
        static BlockHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BlockHeader),
                new FrameworkPropertyMetadata(typeof(BlockHeader)));
        }
    }
}
