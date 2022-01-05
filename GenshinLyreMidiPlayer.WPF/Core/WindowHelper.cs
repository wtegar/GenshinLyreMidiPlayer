﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using GenshinLyreMidiPlayer.Data.Properties;
using Microsoft.Win32;

namespace GenshinLyreMidiPlayer.WPF.Core;

public static class WindowHelper
{
    public static string? InstallLocation => Registry.LocalMachine
        .OpenSubKey(@"SOFTWARE\launcher", false)
        ?.GetValue("InstPath") as string;

    private static string GenshinProcessName
        => Path.GetFileNameWithoutExtension(Settings.Default.GenshinLocation)!;

    public static bool EnsureGameOnTop()
    {
        var genshinWindow = FindWindowByProcessName(GenshinProcessName);

        if (genshinWindow is null)
            return false;

        SwitchToThisWindow((IntPtr) genshinWindow, true);

        return GetForegroundWindow().Equals(genshinWindow);
    }

    public static bool IsGameFocused()
    {
        var genshinWindow = FindWindowByProcessName(GenshinProcessName);
        return genshinWindow != null &&
            IsWindowFocused((IntPtr) genshinWindow);
    }

    private static bool IsWindowFocused(IntPtr windowPtr)
    {
        var hWnd = GetForegroundWindow();
        return hWnd.Equals(windowPtr);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    private static extern IntPtr GetForegroundWindow();

    private static IntPtr? FindWindowByProcessName(string processName)
    {
        var process = Process.GetProcessesByName(processName);
        return process.FirstOrDefault(p => p.MainWindowHandle != IntPtr.Zero)?.MainWindowHandle;
    }

    [DllImport("user32.dll")]
    private static extern void SwitchToThisWindow(IntPtr hWnd, bool fUnknown);
}