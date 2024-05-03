

using System.Windows.Media;

namespace AdminApp.Helpers;

public static class ColorHelpers
{
    public static Brush? Hex2Brush(this string hex)
    {
        try
        {
            var converter = new BrushConverter();
            return (Brush)converter.ConvertFrom(hex)!;
        }
        catch { }
        return null;
    }
}
