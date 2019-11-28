using System;
using System.Runtime.InteropServices;

namespace keyboards
{
    public static class Installer
    {
        [DllImport("libX11")]
        private static extern IntPtr XOpenDisplay(string displayName);

        [DllImport("libX11")]
        private static extern void XCloseDisplay(IntPtr display);

        internal static bool RootHasPermission()
        {
            var hasAccess = true;
            var display = XOpenDisplay(":0");
            if (display == IntPtr.Zero) hasAccess = false;

            XCloseDisplay(display);

            return hasAccess;
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