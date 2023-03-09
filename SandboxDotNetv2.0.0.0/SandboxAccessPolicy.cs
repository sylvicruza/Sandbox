using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SandboxDotNetv2._0._0._0
{
    public static class SandboxAccessPolicy
    {
        private const string SecurityPermission = "SecurityPermission";
        private const string WebPermission = "WebPermission";
        private const string FileIOPermission = "FileIOPermission";
        private const string UIPermission = "UIPermission";
        private const string ReflectionPermission = "ReflectionPermission";
        private const string PrincipalPermission = "PrincipalPermission";
        private const string FileDialogPermission = "FileDialogPermission";
        private const string RegistryPermission = "RegistryPermission";
        private const string EnvironmentPermission = "EnvironmentPermission";

        public static PermissionSet UpdatePermissionSet(PermissionSet permissionSet)
        {
            permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            foreach (var permission in permissionSet)
            {
                Console.WriteLine(permission.GetType().Name);

                if (permission.GetType().Name.Contains(SecurityPermission))
                {
                    permissionSet.AddPermission(new SecurityPermission(PermissionState.Unrestricted));
                }
                else if (permission.GetType().Name.Contains(WebPermission))
                {
                    permissionSet.AddPermission(new WebPermission(PermissionState.Unrestricted));
                }
                else if (permission.GetType().Name.Contains(FileIOPermission))
                {
                    permissionSet.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
                }
                else if (permission.GetType().Name.Contains(UIPermission))
                {
                    permissionSet.AddPermission(new UIPermission(PermissionState.Unrestricted));
                }
                else if (permission.GetType().Name.Contains(ReflectionPermission))
                {
                    permissionSet.AddPermission(new ReflectionPermission(PermissionState.Unrestricted));
                }
                else if (permission.GetType().Name.Contains(PrincipalPermission))
                {
                    permissionSet.AddPermission(new PrincipalPermission(PermissionState.Unrestricted));
                }
                else if (permission.GetType().Name.Contains(FileDialogPermission))
                {
                    permissionSet.AddPermission(new FileDialogPermission(PermissionState.Unrestricted));
                }
                else if (permission.GetType().Name.Contains(RegistryPermission))
                {
                    permissionSet.AddPermission(new RegistryPermission(PermissionState.Unrestricted));
                }
                else if (permission.GetType().Name.Contains(EnvironmentPermission))
                {
                    permissionSet.AddPermission(new EnvironmentPermission(PermissionState.Unrestricted));
                }
                else
                {
                    MessageBox.Show(Messages.InvalidPermission, Messages.OperationFailed, MessageBoxButtons.OK, MessageBoxIcon.Error);


                }
            }

            return permissionSet;

        }


        public static IPermission UpdatePermission(string permissionType)
        {
            return PermissionFactory.CreatePermission(permissionType);

        }

        public static PermissionSet GetPermissionsFromConfig(string configFilePath)
        {
            var permissionSet = new PermissionSet(PermissionState.None);

            try
            {
                var doc = new XmlDocument();
                doc.Load(configFilePath);
                var permissionNodes = doc.SelectNodes("configuration/runtime/security/requestPermission");

                if (permissionNodes != null)
                {
                    foreach (XmlNode permissionNode in permissionNodes)
                    {
                        var className = permissionNode.Attributes["class"].Value;
                        var unrestricted = bool.Parse(permissionNode.Attributes["Unrestricted"].Value);

                        var permissionType = Type.GetType(className);
                        var permission = unrestricted ? (CodeAccessPermission)Activator.CreateInstance(permissionType) : null;

                        permissionSet.AddPermission(permission);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading permissions from config file: {ex.Message}");
            }

            return permissionSet;
        }



    }
}

