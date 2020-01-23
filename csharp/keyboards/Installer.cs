using System;
using System.Runtime.InteropServices;

namespace keyboards
{
    public static class Installer
    {
        internal static string ActiveDisplay { get; set; }

        [DllImport("libX11")]
        private static extern IntPtr XOpenDisplay(string displayName);

        [DllImport("libX11")]
        private static extern void XCloseDisplay(IntPtr display);

        private static bool CheckAccessToDisplay(string d)
        {
            var hasAccess = true;
            var display = XOpenDisplay(d);
            if (display == IntPtr.Zero) hasAccess = false;

            if (hasAccess)
                XCloseDisplay(display);

            return hasAccess;
        }

        internal static bool RootHasPermission()
        {
            for (var display = 0; display < 10; display++)
            {
                if (!CheckAccessToDisplay($":{display}")) continue;

                ActiveDisplay = $":{display}";
                return true;
            }

            return false;
        }

        private static bool IsInProfile()
        {
            var contents =
                new SpecialFile(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.profile")
                    .Contents;
            return contents.Contains("SI:localuser:root");
        }

        internal static void Install()
        {
            if (!RootHasPermission())
            {
                Console.WriteLine(
                    "I've detected that root doesn't have access to your x-session. If you would like to\n" +
                    "turn off the keyboard backlights when the screen is off, you'll need to give root\n" +
                    "permission to access it. This might have security implications that you'll need to\n" +
                    "evaluate for yourself. To give root permission to your screen until you logout:");
                Console.WriteLine("   xhost +SI:localuser:root");
                Console.WriteLine("You can add it to your ~/.profile if you'd like to make it permanent.");
            }
            else if (!IsInProfile())
            {
                Console.WriteLine(
                    "I've detected that root doesn't have permanent access to your x-session. If you would like to\n" +
                    "turn off the keyboard backlights when the screen is off, you'll need to give root\n" +
                    "permission to access it. This might have security implications that you'll need to\n" +
                    "evaluate for yourself. To give root permission to your screen until you logout:");
                Console.WriteLine("   xhost +SI:localuser:root");
                Console.WriteLine("Add it to your ~/.profile to make it permanent.");
            }
        }
    }
}