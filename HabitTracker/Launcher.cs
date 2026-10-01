using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace HabitTrackerLauncher
{
    static class Program
    {
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool SetWindowText(IntPtr hWnd, string lpString);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern int GetMenuItemCount(IntPtr hMenu);

        [DllImport("user32.dll")]
        private static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        private const int WH_MOUSE_LL = 14;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_RBUTTONUP = 0x0205;
        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int WM_NCLBUTTONUP = 0x00A2;
        private const int WM_NCRBUTTONDOWN = 0x00A4;
        private const int WM_NCRBUTTONUP = 0x00A5;

        private const uint WM_SETICON = 0x0080;
        private const uint ICON_SMALL = 0;
        private const uint ICON_BIG = 1;
        private const uint IMAGE_ICON = 1;
        private const uint LR_LOADFROMFILE = 0x00000010;
        private const uint MF_BYPOSITION = 0x00000400;
        private const uint MF_REMOVE = 0x00001000;

        // Windows 11 DWM Attributes
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        private const int DWMWA_BORDER_COLOR = 34;
        private const int DWMWA_CAPTION_COLOR = 35;
        private const int DWMWA_TEXT_COLOR = 36;
        private const int DWMWCP_ROUND = 2;

        private static IntPtr appHwnd = IntPtr.Zero;
        private static IntPtr hookId = IntPtr.Zero;
        private static LowLevelMouseProc mouseProcDelegate;

        private static int ColorToBgr(Color c)
        {
            return (c.B << 16) | (c.G << 8) | c.R;
        }

        [STAThread]
        static void Main()
        {
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string htmlPath = Path.Combine(appDir, "index.html");
            string profileDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HabitTrackerProfile");

            string edgeExe = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
            if (!File.Exists(edgeExe))
            {
                edgeExe = @"C:\Program Files\Microsoft\Edge\Application\msedge.exe";
            }

            if (File.Exists(edgeExe))
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = edgeExe;
                psi.Arguments = string.Format(
                    "--app=\"file:///{0}\" --window-size=1590,980 --user-data-dir=\"{1}\" --no-first-run --no-default-browser-check --disable-extensions --disable-background-networking --disable-sync --enable-gpu-rasterization --enable-zero-copy --disable-features=EdgeContextMenus",
                    htmlPath.Replace('\\', '/'),
                    profileDir
                );
                psi.UseShellExecute = true;
                psi.WindowStyle = ProcessWindowStyle.Normal;
                Process.Start(psi);

                // Set up global low-level mouse hook to completely drop top-left clicks
                mouseProcDelegate = HookCallback;
                hookId = SetWindowsHookEx(WH_MOUSE_LL, mouseProcDelegate, IntPtr.Zero, 0);

                // Window styling and icon thread
                Thread t = new Thread(() =>
                {
                    string icoFile = Path.Combine(appDir, "flame_streak_v1.ico");
                    if (!File.Exists(icoFile)) icoFile = Path.Combine(appDir, "app.ico");

                    bool styled = false;

                    while (true)
                    {
                        Thread.Sleep(200);

                        if (appHwnd == IntPtr.Zero || !IsWindow(appHwnd))
                        {
                            styled = false;
                            EnumWindows((hWnd, lParam) =>
                            {
                                StringBuilder className = new StringBuilder(256);
                                GetClassName(hWnd, className, className.Capacity);
                                if (className.ToString() == "Chrome_WidgetWin_1" && IsWindowVisible(hWnd))
                                {
                                    uint pid;
                                    GetWindowThreadProcessId(hWnd, out pid);
                                    try
                                    {
                                        Process proc = Process.GetProcessById((int)pid);
                                        if (proc.ProcessName.IndexOf("msedge", StringComparison.OrdinalIgnoreCase) >= 0)
                                        {
                                            RECT r;
                                            if (GetWindowRect(hWnd, out r) && (r.Right - r.Left) > 500)
                                            {
                                                appHwnd = hWnd;
                                                return false; // found main window
                                            }
                                        }
                                    }
                                    catch { }
                                }
                                return true;
                            }, IntPtr.Zero);
                        }

                        if (appHwnd != IntPtr.Zero && IsWindow(appHwnd) && !styled)
                        {
                            styled = true;

                            // 1. Taskbar Icon (ICON_BIG) set to Flame Icon
                            if (File.Exists(icoFile))
                            {
                                IntPtr hBig = LoadImage(IntPtr.Zero, icoFile, IMAGE_ICON, 64, 64, LR_LOADFROMFILE);
                                if (hBig != IntPtr.Zero) SendMessage(appHwnd, WM_SETICON, (IntPtr)ICON_BIG, hBig);
                            }

                            // 2. Corner Icon (ICON_SMALL) set to transparent empty icon
                            using (Bitmap transBmp = new Bitmap(16, 16, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                            {
                                IntPtr hTrans = transBmp.GetHicon();
                                SendMessage(appHwnd, WM_SETICON, (IntPtr)ICON_SMALL, hTrans);
                            }

                            // 3. Strip System Menu entries
                            IntPtr hMenu = GetSystemMenu(appHwnd, false);
                            if (hMenu != IntPtr.Zero)
                            {
                                int count = GetMenuItemCount(hMenu);
                                for (int m = count - 1; m >= 0; m--)
                                {
                                    RemoveMenu(hMenu, (uint)m, MF_BYPOSITION | MF_REMOVE);
                                }
                            }

                            // 4. Set aesthetic spaced title
                            SetWindowText(appHwnd, "    ✦   H A B I T   T R A C K E R   ✦");

                            // 5. Windows 11 DWM Styling
                            int trueVal = 1;
                            DwmSetWindowAttribute(appHwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, ref trueVal, sizeof(int));
                            int roundCorners = DWMWCP_ROUND;
                            DwmSetWindowAttribute(appHwnd, DWMWA_WINDOW_CORNER_PREFERENCE, ref roundCorners, sizeof(int));

                            int captionColor = ColorToBgr(Color.FromArgb(15, 23, 42));
                            DwmSetWindowAttribute(appHwnd, DWMWA_CAPTION_COLOR, ref captionColor, sizeof(int));

                            int textColor = ColorToBgr(Color.FromArgb(226, 232, 240));
                            DwmSetWindowAttribute(appHwnd, DWMWA_TEXT_COLOR, ref textColor, sizeof(int));

                            int borderColor = ColorToBgr(Color.FromArgb(99, 102, 241));
                            DwmSetWindowAttribute(appHwnd, DWMWA_BORDER_COLOR, ref borderColor, sizeof(int));
                        }
                    }
                });
                t.IsBackground = true;
                t.Start();

                // Run message loop until the window is actually closed by the user
                System.Windows.Forms.Timer exitCheckTimer = new System.Windows.Forms.Timer();
                exitCheckTimer.Interval = 1000;
                int windowFoundCount = 0;
                exitCheckTimer.Tick += (s, args) =>
                {
                    if (appHwnd != IntPtr.Zero && IsWindow(appHwnd))
                    {
                        windowFoundCount++;
                    }
                    else if (windowFoundCount > 2)
                    {
                        // Window was closed by user
                        exitCheckTimer.Stop();
                        if (hookId != IntPtr.Zero) UnhookWindowsHookEx(hookId);
                        Application.Exit();
                    }
                };
                exitCheckTimer.Start();

                Application.Run();
                if (hookId != IntPtr.Zero) UnhookWindowsHookEx(hookId);
            }
            else
            {
                Process.Start(htmlPath);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && appHwnd != IntPtr.Zero && IsWindow(appHwnd))
            {
                int msg = wParam.ToInt32();
                if (msg == WM_LBUTTONDOWN || msg == WM_LBUTTONUP ||
                    msg == WM_RBUTTONDOWN || msg == WM_RBUTTONUP ||
                    msg == WM_NCLBUTTONDOWN || msg == WM_NCLBUTTONUP ||
                    msg == WM_NCRBUTTONDOWN || msg == WM_NCRBUTTONUP)
                {
                    MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                    RECT rect;
                    if (GetWindowRect(appHwnd, out rect))
                    {
                        // Top-left corner box (80px wide, 45px high)
                        if (hookStruct.pt.x >= rect.Left && hookStruct.pt.x <= rect.Left + 80 &&
                            hookStruct.pt.y >= rect.Top && hookStruct.pt.y <= rect.Top + 45)
                        {
                            // Drop click completely so the menu NEVER opens!
                            return (IntPtr)1;
                        }
                    }
                }
            }
            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }
    }
}
