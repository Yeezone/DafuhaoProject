using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using com.QH.QPGame.GameUtils;
using com.QH.QPGame.Utility;
using UnityEngine;
using System.Collections.Generic;

namespace com.QH.QPGame.Lobby
{
    public class Win32Api : Singleton<Win32Api>
    {
        private static RECT desktopPos;
        private static IntPtr windowHandle;
        private int Width;
        private int Height;
        private Int64 oriStyle;
        private bool init = false;
        private Process mProcess = null;

        public bool Initialize(bool check, int width, int height)
        {
            windowHandle = FindU3DHandle();

            GetWindowRect(GetDesktopWindow(), out desktopPos);

            oriStyle = GetWindowLong(windowHandle, GWL_STYLE).ToInt64();

            if (check && !init && !CheckAndCreateMutex(UnityEngine.Application.productName + "_Singleton_Check"))
            {
                MessageBox(windowHandle, "游戏已经在运行了", "发生错误", 0x10);
                return false;
            }

            //禁用掉输入法
            //ImmDisableIME(-1);

            Width = width;
            Height = height;

            init = true;

            return true;
        }

        public void UnInitialize()
        {
            CloseBrowser();
            ReleaseMutex();
        }

        public uint MsgBox(string text, string title = "提示", uint flag = MB_OK)
        {
            return MessageBox(windowHandle, text, title, MB_OK);
        }

        public IntPtr GetWindowHandle()
        {
            return windowHandle;
        }

        public IntPtr FindU3DHandle()
        {
            var found = IntPtr.Zero;
            var tid = GetCurrentThreadId();
            EnumThreadWindows(tid, delegate(IntPtr hwnd, IntPtr param)
               {
                   var sb = new StringBuilder(256);
                   GetClassNameW(hwnd, sb, 256);

                   if (sb.ToString() != "UnityWndClass")
                   {
                       return true;
                   }
               
                   found = hwnd;

                   return true;
               }, IntPtr.Zero);

            return found;
        }


        public void ShowWindow(uint cmd)
        {
            ShowWindow(windowHandle, cmd);
        }

        public void ShowMinWindow()
        {
            ShowWindow(windowHandle, SW_SHOWMINIMIZED);
        }

        public void ShowNormalWindow()
        {
            ShowWindow(windowHandle, SW_SHOWNORMAL);
        }

        public bool IsMaxWindow()
        {
            return Screen.fullScreen;
        }

        public void SwitchMaxWindow()
        {
            if (Screen.fullScreen)
            {
                Screen.SetResolution(Width, Height, false);
            }
            else
            {
                Screen.fullScreen = true;
            }
            /*if(IsZoomed(U3DWindowHandle))
            {
                ShowWindow(SW_SHOWNORMAL);
            }
            else
            {
                ShowWindow(SW_SHOWMAXIMIZED);
            }*/
        }

        public void EnableBorder(bool res)
        {
            if (!res)
            {
                var style = oriStyle;
                style &= ~(WS_CAPTION | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_SYSMENU | WS_THICKFRAME);

                SetWindowLong(windowHandle, GWL_STYLE, (int)style);

                SetWindowPos(windowHandle, 0, 0, 0, Width, Height, SWP_NOMOVE);
            }
            else
            {
                SetWindowLong(windowHandle, GWL_STYLE, (int)oriStyle);
            }
        }

        public void ResizeWindow(int w, int h)
        {
            //SetWindowPos(U3DWindowHandle, 0, 0,0, w, h, SWP_NOMOVE);
            Screen.SetResolution(w, h, false);
        }

        public static string GetExePath()
        {
            return Application.dataPath + "/../";
        }

        public void MoveWindow(int x, int y)
        {
            //从屏幕坐标转换为窗口坐标
            POINT pt = new POINT();
            GetCursorPos(out pt);
            ScreenToClient(windowHandle, out pt);

            int width = UnityEngine.Screen.width;
            int height = UnityEngine.Screen.height;

            //从native以左上角为原点的坐标转换为u3d的左下角为原点的坐标
            pt.Y = height - pt.Y;
            pt.X -= x;
            pt.Y -= y;

            RECT windowPos;
            GetWindowRect(windowHandle, out windowPos);

            //计算偏移
            RECT rect = new RECT();
            rect.Left = windowPos.Left + pt.X;
            rect.Top = windowPos.Top - pt.Y;
            rect.Right = rect.Left + width + pt.X;
            rect.Bottom = rect.Top + height - pt.Y;

            //防止被甩出桌面
            RECT r = new RECT();
            if (!IntersectRect(ref r, ref rect, ref desktopPos))
            {
                return;
            }

            SetWindowPos(windowHandle, 0, rect.Left, rect.Top, 0, 0, SWP_NOSIZE);
        }

        public bool CopyTextToClipboard(string text)
        {
            if (text.Trim().Length == 0)
            {
                return false;
            }

            if (!OpenClipboard(IntPtr.Zero) || !EmptyClipboard())
            {
                return false;
            }

            try
            {
                IntPtr ptr = Marshal.StringToHGlobalUni(text);
                var lockPtr = GlobalLock(ptr);
                GlobalUnlock(lockPtr);
                SetClipboardData(CLIP_TEXT_DATA, ptr);
                CloseClipboard();
                //Marshal.ZeroFreeGlobalAllocUnicode(lockPtr);
            }
            catch (Exception ex)
            {
                Logger.Sys.LogException(ex);
                return false;
            }

            return true;
        }

        private IntPtr mProcessMutex = IntPtr.Zero;

        public bool CheckAndCreateMutex(string name)
        {
            try
            {
                IntPtr mutex = CreateMutex(0, false, name);
                if (mutex.ToInt32() == -1)
                {
                    return false;
                }

                if (Marshal.GetLastWin32Error() == 183)
                {
                    return false;
                }
                mProcessMutex = mutex;
            }
            catch (Exception ex)
            {
                Logger.Sys.LogException(ex);
                return false;
            }

            return true;
        }

        public void ReleaseMutex()
        {
            try
            {
                if (mProcessMutex != IntPtr.Zero)
                {
                    CloseHandle(mProcessMutex);
                    mProcessMutex = IntPtr.Zero;
                }
            }
            catch (Exception ex)
            {
                Logger.Sys.LogException(ex);
            }

        }

        public void OpenUrlInBrowser(string title, string url, int width, int height)
        {
            try
            {
                CloseBrowser();

                string path = GetExePath();

#if UNITY_EDITOR
                windowHandle = GetActiveWindow();
                path += "/BuildAssets/Inno/additions/";
#endif

                string fileName = path + "/wb.exe";

                RECT windowPos;
                GetWindowRect(windowHandle, out windowPos);

                int x = windowPos.Left + (windowPos.Right - windowPos.Left - width) / 2;
                int y = windowPos.Top + (windowPos.Bottom - windowPos.Top - height) / 2;

                var rect = new RECT() { Left = x, Right = width + x, Top = y, Bottom = y + height };
                string strParam = string.Format("/title={0} /parent={1} /rect={2},{3},{4},{5} /url={6}", title,
                                               windowHandle.ToInt32(), rect.Left, rect.Top, rect.Right, rect.Bottom, url);


                Logger.Sys.Log("OpenUrlInBrowser:" + fileName);
                Logger.Sys.Log("OpenUrlInBrowser:" + strParam);

                mProcess = new Process();
                mProcess.EnableRaisingEvents = true;
                mProcess.Exited += (sender, args) => { Logger.Sys.Log("!!!!!!!!!!!!!!!!!!!!!"); mProcess = null; };
                mProcess.StartInfo.WorkingDirectory = path;
                mProcess.StartInfo.FileName = fileName;
                mProcess.StartInfo.Arguments = strParam;
                mProcess.Start();

                //mProcess.MainWindowHandle
            }
            catch (Exception ex)
            {
                MessageBox(windowHandle, "请检查是否安装完整并尝试重新安装，错误：" + ex.Message, "发生错误", 0x10);
            }
        }

        public void CloseBrowser()
        {
            try
            {
                Logger.Sys.Log("CloseBrowser " + (mProcess != null ? mProcess.HasExited.ToString() : ""));
                if (mProcess != null)
                {
                    mProcess.Kill();
                    mProcess.Dispose();
                    mProcess = null;
                }
            }
            catch (Exception ex)
            {
            }
        }


        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOMOVE = 0x0002;
        public const int SW_SHOWNORMAL = 1;
        public const int SW_SHOWMINIMIZED = 2;
        public const int SW_SHOWMAXIMIZED = 3;
        public const int CLIP_TEXT_DATA = 13;
        public const int GWL_STYLE = -16;

        public const uint WS_POPUP = 0x80000000;
        public const uint WS_CAPTION = 0xC00000;     /* WS_BORDER | WS_DLGFRAME  */
        public const uint WS_MINIMIZEBOX = 0x20000;
        public const uint WS_MAXIMIZEBOX = 0x10000;
        public const uint WS_SYSMENU = 0x80000;
        public const uint WS_THICKFRAME = 0x40000;

        public const uint MB_OK = 0x00000000;
        public const uint MB_OKCANCEL = 0x00000001;
        public const uint MB_ABORTRETRYIGNORE = 0x00000002;
        public const uint MB_YESNOCANCEL = 0x00000003;
        public const uint MB_YESNO = 0x00000004;
        public const uint MB_RETRYCANCEL = 0x00000005;


        public const uint KLF_ACTIVATE = 0x00000001;
        public const uint KLF_SUBSTITUTE_OK = 0x00000002;
        public const uint KLF_REORDER = 0x00000008;
        public const uint KLF_NOTELLSHELL = 0x00000080;
        public const uint KLF_SETFORPROCESS = 0x00000100;
        public const uint KLF_SHIFTLOCK = 0x00010000;
        public const uint KLF_RESET = 0x40000000;

        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("Imm32.dll")]
        public static extern bool ImmDisableIME(int id);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hwnd, int _nIndex);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hwnd, bool unknown = true);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hwnd, ref uint lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern bool IsZoomed(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindowExW(string className, string windowName);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindowExW(IntPtr parent, IntPtr child, string className, string window);

        public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lparam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool EnumWindows(EnumWindowsProc proc, IntPtr param);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool EnumThreadWindows(int thread, EnumWindowsProc proc, IntPtr param);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetParent(IntPtr hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetClassNameW(IntPtr hwnd, StringBuilder str, int len);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowTextW(IntPtr hwnd, StringBuilder str, int len);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetCurrentThreadId();

        [DllImport("user32.dll")]
        public static extern bool IntersectRect(ref RECT rect, ref RECT dest, ref RECT src);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT pt);

        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hWnd, out POINT pt);

        [DllImport("user32.dll")]
        public static extern bool OpenClipboard(IntPtr owner);

        [DllImport("user32.dll")]
        public static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        public static extern bool EmptyClipboard();

        [DllImport("user32.dll")]
        public static extern bool SetClipboardData(uint format, IntPtr data);

        [DllImport("user32.dll", EntryPoint = "MessageBoxW", CharSet = CharSet.Unicode)]
        public static extern uint MessageBox(IntPtr hwnd, string text, string caption, uint type);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalLock(IntPtr data);

        [DllImport("kernel32.dll")]
        public static extern bool GlobalUnlock(IntPtr data);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr OpenMutex(uint dwDesiredAccess, bool bInheritHandle, string IpName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateMutex(uint dwDesiredAccess, bool bInheritHandle, string IpName);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadKeyboardLayoutW(string name, int flags);

        [DllImport("user32.dll")]
        public static extern bool ActivateKeyboardLayout(IntPtr hkl, int flags);

        [DllImport("user32.dll")]
        public static extern bool UnloadKeyboardLayout(IntPtr hkl);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardLayoutNameW(out StringBuilder name);

        [DllImport("user32.dll")]
        public static extern int GetKeyboardLayoutList(int size, IntPtr[] array);

        [DllImport("user32.dll")]
        public static extern IntPtr GetKeyboardLayout(int name);

    }
}

