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
        public static PermissionSet UpdatePermissionSet(PermissionSet permissionSet)
        {
            try
            {
                foreach (var permission in permissionSet)
                {
                    permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
                    if (permission.GetType() == typeof(FileIOPermission) && permission.Equals(new FileIOPermission(FileIOPermissionAccess.Read, AppDomain.CurrentDomain.BaseDirectory)))
                    {
                        permissionSet.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
                    }
                    else if (permission.GetType() == typeof(WebPermission) && permission.Equals(new WebPermission(NetworkAccess.Connect, "*")))
                    {
                        permissionSet.AddPermission(new WebPermission(PermissionState.Unrestricted));
                    }
                    else if (permission.GetType() == typeof(SecurityPermission) && permission.Equals(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode)))
                    {
                        permissionSet.AddPermission(new SecurityPermission(PermissionState.Unrestricted));
                    }
                    else if (permission.GetType() == typeof(FileIOPermission) && permission.Equals(new FileIOPermission(FileIOPermissionAccess.AllAccess, AppDomain.CurrentDomain.BaseDirectory)))
                    {
                        permissionSet.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
                    }
                    else if (permission.GetType() == typeof(FileIOPermission) && permission.Equals(new FileIOPermission(FileIOPermissionAccess.Write, AppDomain.CurrentDomain.BaseDirectory)))
                    {
                        permissionSet.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
                    }
                    else if (permission.GetType() == typeof(UIPermission) && permission.Equals(new UIPermission(UIPermissionWindow.AllWindows, UIPermissionClipboard.AllClipboard)))
                    {
                        permissionSet.AddPermission(new UIPermission(PermissionState.Unrestricted));
                    }
                    else if (permission.GetType() == typeof(ReflectionPermission) && permission.Equals(new ReflectionPermission(ReflectionPermissionFlag.AllFlags)))
                    {
                        permissionSet.AddPermission(new ReflectionPermission(PermissionState.Unrestricted));
                    }
                    else if (permission.GetType() == typeof(ReflectionPermission) && permission.Equals(new ReflectionPermission(ReflectionPermissionFlag.TypeInformation)))
                    {
                        permissionSet.AddPermission(new ReflectionPermission(PermissionState.Unrestricted));
                    }
                    else if (permission.GetType() == typeof(FileDialogPermission) && permission.Equals(new FileDialogPermission(FileDialogPermissionAccess.Open)))
                    {
                        permissionSet.AddPermission(new FileDialogPermission(PermissionState.Unrestricted));
                    }
                    else if (permission.GetType() == typeof(RegistryPermission) && permission.Equals(new RegistryPermission(RegistryPermissionAccess.AllAccess, "*")))
                    {
                        permissionSet.AddPermission(new RegistryPermission(PermissionState.Unrestricted));
                    }
                    else if (permission.GetType() == typeof(EnvironmentPermission) && permission.Equals(new EnvironmentPermission(EnvironmentPermissionAccess.Read, "CommandLine")))
                    {
                        permissionSet.AddPermission(new EnvironmentPermission(PermissionState.Unrestricted));
                    }
                    else if (permission.GetType() == typeof(EnvironmentPermission) && permission.Equals(new EnvironmentPermission(EnvironmentPermissionAccess.AllAccess, "*")))
                    {
                        permissionSet.AddPermission(new EnvironmentPermission(PermissionState.Unrestricted));
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                return permissionSet;
            }
            catch (Exception ex)
            {
                throw new ArgumentException();
            }
        }


        public static IPermission UpdatePermission(string permissionType)
        {
           return PermissionFactory.CreatePermission(permissionType);
           
        }
        public static void UpdatePermissionFromCommandLine(string permissionName, bool allowed)
        {
            string _configFilePath = ConfigurationManager.AppSettings["SandboxConfigFilePath"];
            try
            {
                var doc = new XmlDocument();
                doc.Load(_configFilePath);
                var permissionNode = doc.SelectSingleNode($"configuration/runtime/security/requestPermission[@class='{permissionName}, mscorlib']");

                if (permissionNode != null)
                {
                    permissionNode.Attributes["Unrestricted"].Value = allowed.ToString();
                    doc.Save(_configFilePath);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating permission: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}

