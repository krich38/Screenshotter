using System;
using System.Runtime.InteropServices;

namespace ScreenShifter
{
    class ClipboardListener
    {

        public const int WM_CLIPBOARDUPDATE = 0x031D;

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        public static bool IsListening = false;

        public static bool AddListener(IntPtr handle)
        {
            IsListening = AddClipboardFormatListener(handle);
            return IsListening;
        }

        public static bool RemoveListener(IntPtr handle)
        {
            bool b = RemoveClipboardFormatListener(handle);
            if (b)
                IsListening = false;
            return b;
        }

    }
}
