using System;
using System.Runtime.InteropServices;

public class DisplayScaleHelper
{
    private const int LOGPIXELSX = 88; // Logical pixels/inch in X
    private const int LOGPIXELSY = 90; // Logical pixels/inch in Y

    [DllImport("gdi32.dll")]
    private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

    [DllImport("user32.dll")]
    private static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    public static float GetWindowsDisplayScale()
    {
        IntPtr hdc = GetDC(IntPtr.Zero);
        if (hdc == IntPtr.Zero)
        {
            throw new InvalidOperationException("Unable to get device context.");
        }

        int dpiX = GetDeviceCaps(hdc, LOGPIXELSX);
        ReleaseDC(IntPtr.Zero, hdc);

        // The default DPI is 96, so the scale factor is dpi / 96
        float scaleFactor = dpiX / 96.0f;
        return scaleFactor;
    }
}
