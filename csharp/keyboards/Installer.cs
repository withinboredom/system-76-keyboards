using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace keyboards
{
    public static class Installer
    {
        private static string? Parameters { get; set; }

        private static string SystemD =>
            $@"
[Unit]
Description=System76 Keyboard Colors
[Service]
Type=Simple
ExecStart=/usr/local/bin/keyboard-color {Parameters}
PIDFile=keyboard-colors.pid
[Install]
WantedBy=multi-user.target
";

        internal static void CreateParametersFromOptions(string[] options)
        {
            Parameters = string.Join(' ', options.Where(s => !s.Contains("--install") || !s.Contains("-i")).ToArray());
        }

        private static void PutMeInRightSpot()
        {
            var path = Environment.CurrentDirectory + "/keyboard-color";

            if (!File.Exists(path) && File.Exists("/usr/local/bin/keyboard-color"))
                return;

            if (!File.Exists(path))
            {
                Console.WriteLine("Unable to locate `keyboard-color` in the current directory.");
                Environment.Exit(1);
            }

            Console.WriteLine($"Copying {path} to /usr/local/bin");
            File.Copy(path, "/usr/local/bin/keyboard-color", true);

            if (File.Exists("/opt/keyboard-colors/keyboard-color.php"))
                Console.WriteLine("Please delete /opt/keyboard-colors/keyboard-color.php as it's no longer needed.");
        }

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

        private static void CreateService()
        {
            var servicePath = "/etc/systemd/system/keyboard-colors.service";
            File.WriteAllText(servicePath, SystemD);
        }

        internal static void Install()
        {
            Console.WriteLine("Copying `keyboard-color` to /usr/local/bin");
            PutMeInRightSpot();

            Console.WriteLine("Creating service file in /etc/systemd/system/keyboard-colors.service");
            CreateService();

            Console.WriteLine("If you want to start this on boot run:");
            Console.WriteLine("   sudo systemctl enable keyboard-colors");

            Console.WriteLine("If you want to start the service right now, run:");
            Console.WriteLine("   sudo systemctl start keyboard-colors\n");

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