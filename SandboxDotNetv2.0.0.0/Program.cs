using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Security;
using System.Windows.Forms;

namespace SandboxDotNetv2._0._0._0
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Length < 2 || args[0] != "run")
            {
                Console.WriteLine("Invalid convention to run sandbox");
                Console.WriteLine("Usage:  SandboxDotNetv2.0.0.0.exe run --file <filepath> [--args <arg1> <arg2> ...] [--perm <permission1> <permission2> <...> | --policy <filepath>] ");

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new SandboxUserInterface());  //Graphic User Interface
            }
            else { CommandLineRunner(args); } //Command Prompt        

        }

        private static void CommandLineRunner(string[] args)
        {
            string filePath = null;
            string policyFilePath = null;
            string[] argValues = null;
            List<string> permValues = new List<string>();
            string pathToUntrusted;
            string untrustedClass;
            try
            {
                CommandSetup(args, ref filePath, ref policyFilePath, ref argValues, permValues);

                if (filePath == null)
                {
                    Console.WriteLine("Error: filePath string is empty.");
                    return;
                }
                else if (filePath != null)
                {
                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine("Error: file not found.");
                        return;
                    }
                }

                PermissionSet permissions = new PermissionSet(PermissionState.None);
                pathToUntrusted = Path.GetDirectoryName(filePath);
                untrustedClass = Path.GetFileNameWithoutExtension(filePath);
                if (policyFilePath != null)
                {
                    if (!File.Exists(policyFilePath))
                    {
                        Console.WriteLine("Error: Policy file not found.");
                        return;
                    }
                    var configPermission = SandboxAccessPolicy.GetPermissionsFromConfig(policyFilePath);
                    if (configPermission.Count > 0)
                    {
                        Console.WriteLine("Config Permission values: " + string.Join(", ", configPermission));
                        Sandboxer.SetupAndRun(pathToUntrusted, untrustedClass, argValues, configPermission);
                    }

                }

                else if (permValues.Count > 0)
                {
                    Console.WriteLine("Permission values: " + string.Join(", ", permValues));
                    foreach (var command in permValues)
                    {
                        var permission = PermissionFactory.CreatePermission(command);
                        permissions.AddPermission(permission);
                    }
                }
                Sandboxer.SetupAndRun(pathToUntrusted, untrustedClass, argValues, PermissionFactory.PermissionBuilder(permissions));

            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to execute command, try again.", e.Message);
            }
        }

        private static void CommandSetup(string[] args, ref string filePath, ref string policyFilePath, ref string[] argValues, List<string> permValues)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--file" && i + 1 < args.Length)
                {
                    filePath = args[i + 1];
                }
                else if (args[i] == "--args")
                {
                    List<string> argList = new List<string>();
                    for (int j = i + 1; j < args.Length && !args[j].StartsWith("--"); j++)
                    {
                        argList.Add(args[j]);
                        i = j;
                    }
                    argValues = argList.ToArray();
                }
                else if (args[i] == "--perm" && i + 1 < args.Length)
                {
                    permValues.Add(args[i + 1]);
                }
                else if (args[i] == "--policy" && i + 1 < args.Length)
                {
                    policyFilePath = args[i + 1];
                }
            }
        }
    }
}
