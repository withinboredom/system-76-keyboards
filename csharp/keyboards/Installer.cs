using System;
using System.IO;
using System.Linq;

namespace keyboards
{
    public static class Installer
    {
        internal static void CreateParametersFromOptions(string[] options)
        {
            Parameters = string.Join(' ', options.Where(s => !s.Contains("--install") || !s.Contains("-i")).ToArray());
        }

        internal static void PutMeInRightSpot()
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
            {
                Console.WriteLine("Please delete /opt/keyboard-colors/keyboard-color.php as it's no longer needed.");
            }
        }

        internal static void CreateService()
        {
            var servicePath = "/etc/systemd/system/keyboard-colors.service";
            File.WriteAllText(servicePath, SystemD);
            Console.WriteLine("Run `systemctl enable keyboard-colors.service` to start at boot");
            Console.WriteLine("Run `systemctl restart keyboard-colors.service` to start right now");
            Console.WriteLine("Would you like to start the service at bootup? (y/n)");
            var yn = Console.ReadKey(true);
            if (yn.Key == ConsoleKey.Y)
            {
                Console.WriteLine("Executing: systemctl enable keyboard-colors.service");
                System.Diagnostics.Process.Start("systemctl", "enable keyboard-colors.service")?.WaitForExit();
            }

            Console.WriteLine("Would you like to start the service now? (y/n)");
            yn = Console.ReadKey(true);
            if (yn.Key == ConsoleKey.Y)
            {
                Console.WriteLine("Executing: systemctl restart keyboard-colors.service");
                System.Diagnostics.Process.Start("systemctl", "restart keyboard-colors.service")?.WaitForExit();
            }
        }

        private static string Parameters { get; set; }

        private static string SystemD
        {
            get => $@"
[Unit]
Description=System76 Keyboard Colors
[Service]
Type=Simple
ExecStart=/usr/local/bin/keyboard-color {Parameters} --service
PIDFile=keyboard-colors.pid
[Install]
WantedBy=multi-user.target
";
        }
    }
}