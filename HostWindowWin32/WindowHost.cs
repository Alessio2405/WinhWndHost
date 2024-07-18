using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;

namespace HostWindowWin32
{
    public class WindowHost : HwndHost
    {
        private delegate bool EnumWindowsProc(int hWnd, int lParam);

        private int _childHandle = (int)IntPtr.Zero;

        private int handleTemp;

        [DllImport("user32.dll")]
        private static extern int SetParent(int hWndChild, int hWndNewParent);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindow(int hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetLayeredWindowAttributes(int hwnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(int hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(int hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        public WindowHost(int handle)
        {
            _childHandle = handle;
        }

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            int num = 2;
            int num2 = 0;
            HandleRef result = new HandleRef(this, IntPtr.Zero);
            for (num2 = 0; num2 <= num; num2++)
            {
                try
                {
                    SetWindowLongPtr((IntPtr)_childHandle, -16, (IntPtr)1073741824);
                    SetParent(_childHandle, (int) hwndParent.Handle);
                    handleTemp = _childHandle;
                    result = new HandleRef(this, (IntPtr)_childHandle);
                    if (result.Handle != IntPtr.Zero)
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                }

                Thread.Sleep(100);
            }

            return result;
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            SendMessage((int)hwnd.Handle, 2u, 0, 0);
        }

        public bool IsChildHandleValid()
        {
            return IsWindow(_childHandle);
        }
        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            else
                return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }
    }
}
