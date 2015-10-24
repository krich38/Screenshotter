using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ScreenShifter
{
    class Hotkeys
    {

        private const uint MOD_ALT = 0x1;
        private const uint MOD_CONTROL = 0x2;
        private const uint MOD_SHIFT = 0x4;
        private const uint MOD_WIN = 0x8;

        public const int WM_HOTKEY = 0x312;

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private static int hotKeyCount = -1;

        public static Dictionary<int, Keys> RegisteredHotkeys = new Dictionary<int, Keys>();

        public static bool RegisterHotKey(Form form, Keys key)
        {
            if (key == Keys.None)
                return true; // ignore default

            if (hotKeyCount < 0)
                hotKeyCount = 0;

            uint modifiers = 0;

            if ((key & Keys.Alt) == Keys.Alt)
                modifiers = modifiers | MOD_ALT;

            if ((key & Keys.Control) == Keys.Control)
                modifiers = modifiers | MOD_CONTROL;

            if ((key & Keys.Shift) == Keys.Shift)
                modifiers = modifiers | MOD_SHIFT;

            Keys k = key & ~Keys.Control & ~Keys.Shift & ~Keys.Alt;
            if (!RegisterHotKey(form.Handle, hotKeyCount, modifiers, (uint)k))
                return false;

            RegisteredHotkeys.Add(hotKeyCount, key);
            hotKeyCount++;
            return true;
        }

        public static bool UnRegisterHotKeys(Form form, int id)
        {
            return UnregisterHotKey(form.Handle, id);                
        }

        public static void UnRegisterAllHotKeys(Form form)
        {
            for (int id = 0; id < hotKeyCount; id++)
                UnregisterHotKey(form.Handle, id);
        }

    }
}
