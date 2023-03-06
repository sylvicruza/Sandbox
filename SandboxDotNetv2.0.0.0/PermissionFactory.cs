using System;
using System.Net;
using System.Security.Permissions;
using System.Security;
using System.Collections.Generic;
using System.Collections;

namespace SandboxDotNetv2._0._0._0
{
    public static class PermissionFactory
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
                    return new CustomPermission().Copy();

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

         public static Dictionary<string, IPermission> GetPermission(string permissionType)
        {
            Dictionary<string, IPermission> dict = new Dictionary<string, IPermission>();

            if (permissionType.Equals(PermissionType.SECURITY_PERMISSION_EXECUTION))
            {
                dict.Add(permissionType, new SecurityPermission(SecurityPermissionFlag.Execution));
            }
            else if (permissionType.Equals(PermissionType.SECURITY_PERMISSION_UNMANAGEDCODE))
            {
                dict.Add(permissionType, new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
            }
            else if (permissionType.Equals(PermissionType.FILE_IO_PERMISSION_READ))
            {
                dict.Add(permissionType, new FileIOPermission(FileIOPermissionAccess.Read, AppDomain.CurrentDomain.BaseDirectory));
            }
            else if (permissionType.Equals(PermissionType.FILE_IO_PERMISSION_WRITE))
            {
                dict.Add(permissionType, new FileIOPermission(FileIOPermissionAccess.Write, AppDomain.CurrentDomain.BaseDirectory));
            }
            else if (permissionType.Equals(PermissionType.SECURITY_PERMISSION_ALLACCESS))
            {
                dict.Add(permissionType, new FileIOPermission(FileIOPermissionAccess.AllAccess, AppDomain.CurrentDomain.BaseDirectory));
            }
            else if (permissionType.Equals(PermissionType.UI_PERMISSION))
            {
                dict.Add(permissionType, new UIPermission(UIPermissionWindow.AllWindows, UIPermissionClipboard.AllClipboard));
            }
            else if (permissionType.Equals(PermissionType.SANDBOX_PERMISSION_ALLACCESS))
            {
                dict.Add(permissionType, new UIPermission(UIPermissionWindow.AllWindows, UIPermissionClipboard.AllClipboard));
            }
            else if (permissionType.Equals(PermissionType.REFLECTION_PERMISSION_EMIT))
            {
                dict.Add(permissionType, new ReflectionPermission(ReflectionPermissionFlag.AllFlags));
            }

            else
            {
                throw new ArgumentException($"Invalid permission type: {permissionType}");
            }
            /*case PermissionType.FILE_IO_PERMISSION_READ:
               // return new FileIOPermission(FileIOPermissionAccess.Read, AppDomain.CurrentDomain.BaseDirectory);

            case PermissionType.FILE_IO_PERMISSION_WRITE:
               // return new FileIOPermission(FileIOPermissionAccess.Write, AppDomain.CurrentDomain.BaseDirectory);

            case PermissionType.SECURITY_PERMISSION_ALLACCESS:
             //   return new FileIOPermission(FileIOPermissionAccess.AllAccess, AppDomain.CurrentDomain.BaseDirectory);

            case PermissionType.UI_PERMISSION:
               // return new UIPermission(UIPermissionWindow.AllWindows, UIPermissionClipboard.AllClipboard);

            case PermissionType.SANDBOX_PERMISSION_ALLACCESS:
                //return new CustomPermission().Copy();

            case PermissionType.REFLECTION_PERMISSION_EMIT:
               // return new ReflectionPermission(ReflectionPermissionFlag.AllFlags);

            case PermissionType.REFLECTION_PERMISSION_REFLECTION:
               // return new ReflectionPermission(ReflectionPermissionFlag.TypeInformation);

            case PermissionType.FILE_DIALOG_PERMISSION:
               // return new FileDialogPermission(FileDialogPermissionAccess.Open);

            case PermissionType.WEB_PERMISSION_NETWORK:
              //  return new WebPermission(NetworkAccess.Connect, "*");

            case PermissionType.REGISTRY_PERMISSION_READ:
               // return new RegistryPermission(RegistryPermissionAccess.Read, "*");

            case PermissionType.REGISTRY_PERMISSION_WRITE:
              //  return new RegistryPermission(RegistryPermissionAccess.Write, "*");

            case PermissionType.REGISTRY_PERMISSION_ALLACCESS:
              //  return new RegistryPermission(RegistryPermissionAccess.AllAccess, "*");

            case PermissionType.PRINCIPAL_PERMISSION:
              //  return new PrincipalPermission(null, "Administrators");

            case PermissionType.ENVIRONMENT_PERMISSION_COMMANDLINE:
              //  return new EnvironmentPermission(EnvironmentPermissionAccess.Read, "CommandLine");

            case PermissionType.ENVIRONMENT_PERMISSION_ALLACCESS:
              //  return new EnvironmentPermission(EnvironmentPermissionAccess.AllAccess, "*");*/

            /*default:
                throw new ArgumentException($"Invalid permission type: {permissionType}");*/
        
            return dict;
        }
        public static PermissionSet permissions(string permissionType)
        {
            PermissionSet permissionSet = null;
            var selectedPermissions = GetPermission(permissionType);
            foreach (var permission in selectedPermissions)
            {
                if (permission.Key.Equals(PermissionType.SANDBOX_PERMISSION_ALLACCESS))
                {
                    Console.WriteLine("The dictionary contains 'key'.");              
                    permissionSet = new PermissionSet(PermissionState.Unrestricted);
                    permissionSet.AddPermission(permission.Value);
                    return permissionSet;
                }
                permissionSet = new PermissionSet(PermissionState.None);
                permissionSet.AddPermission(permission.Value);
                return permissionSet;

            }           
            return permissionSet;

        }

        /* class CustomUIPermission
         {
             public IPermission PermissionWrapper()
             {
                 return new UIPermission(PermissionState.Unrestricted);
             }
         }*/

        [Serializable]
        public class CustomPermission : CodeAccessPermission, IUnrestrictedPermission
        {
            public CustomPermission()
            {
            }

            public override IPermission Copy()
            {
                return new CustomPermission();
            }

            public override void FromXml(SecurityElement securityElement)
            {
                throw new NotImplementedException();
            }

            public override SecurityElement ToXml()
            {
                throw new NotImplementedException();
            }

            public override bool IsSubsetOf(IPermission target)
            {
                if (target == null)
                    return false;

                if (target.GetType() != GetType())
                    return false;

                return true;
            }

            public override IPermission Intersect(IPermission target)
            {
                if (target == null)
                    return null;

                if (target.GetType() != GetType())
                    return null;

                return Copy();
            }

            public override IPermission Union(IPermission target)
            {
                if (target == null)
                    return Copy();

                if (target.GetType() != GetType())
                    throw new ArgumentException("Invalid permission type.", "target");

                return Copy();
            }

            public bool IsUnrestricted()
            {
                return true;
            }
        }

    }


}
