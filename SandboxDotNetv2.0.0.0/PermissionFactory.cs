using System;
using System.Net;
using System.Security.Permissions;
using System.Security;
using System.Collections.Generic;
using System.Collections;

namespace SandboxDotNetv2._0._0._0
{
    public static partial class PermissionFactory
    {
        public static IPermission CreatePermission(string permissionType)
        {
            switch (permissionType)
            {
                case PermissionType.SECURITY_PERMISSION_EXECUTION:
                    return new SecurityPermission(SecurityPermissionFlag.Execution);

                case PermissionType.SECURITY_PERMISSION_UNMANAGEDCODE:
                    return new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);

                case PermissionType.FILE_IO_PERMISSION_READ:
                    return new FileIOPermission(FileIOPermissionAccess.Read, AppDomain.CurrentDomain.BaseDirectory);

                case PermissionType.FILE_IO_PERMISSION_WRITE:
                    return new FileIOPermission(FileIOPermissionAccess.Write, AppDomain.CurrentDomain.BaseDirectory);

                case PermissionType.SECURITY_PERMISSION_ALLACCESS:
                    return new FileIOPermission(FileIOPermissionAccess.AllAccess, AppDomain.CurrentDomain.BaseDirectory);

                case PermissionType.UI_PERMISSION:
                    return new UIPermission(UIPermissionWindow.AllWindows, UIPermissionClipboard.AllClipboard);

                case PermissionType.SANDBOX_PERMISSION_ALLACCESS:
                    return new CustomPermission();

                case PermissionType.REFLECTION_PERMISSION_EMIT:
                    return new ReflectionPermission(ReflectionPermissionFlag.AllFlags);

                case PermissionType.REFLECTION_PERMISSION_REFLECTION:
                    return new ReflectionPermission(ReflectionPermissionFlag.TypeInformation);

                case PermissionType.FILE_DIALOG_PERMISSION:
                    return new FileDialogPermission(FileDialogPermissionAccess.Open);

                case PermissionType.WEB_PERMISSION_NETWORK:
                    return new WebPermission(NetworkAccess.Connect, "*");

                case PermissionType.REGISTRY_PERMISSION_READ:
                    return new RegistryPermission(RegistryPermissionAccess.Read, "*");

                case PermissionType.REGISTRY_PERMISSION_WRITE:
                    return new RegistryPermission(RegistryPermissionAccess.Write, "*");

                case PermissionType.REGISTRY_PERMISSION_ALLACCESS:
                    return new RegistryPermission(RegistryPermissionAccess.AllAccess, "*");

                case PermissionType.PRINCIPAL_PERMISSION:
                    return new PrincipalPermission(null, "Administrators");

                case PermissionType.ENVIRONMENT_PERMISSION_COMMANDLINE:
                    return new EnvironmentPermission(EnvironmentPermissionAccess.Read, "CommandLine");

                case PermissionType.ENVIRONMENT_PERMISSION_ALLACCESS:
                    return new EnvironmentPermission(EnvironmentPermissionAccess.AllAccess, "*");

                default:
                    throw new ArgumentException($"Invalid permission type: {permissionType}");
            }
        }


       



        public static PermissionSet PermissionBuilder(PermissionSet permissionSet)
        {
            foreach (IPermission permission in permissionSet)
            {
                if (permission.GetType() == typeof(CustomPermission) && permission.Equals(new CustomPermission()))
                {
                    Console.WriteLine("The permission contains CustomPermission");
                    permissionSet = new CustomPermission().AllAccess();
                    return permissionSet;
                }
            }
            return permissionSet;
        }

    }


}
