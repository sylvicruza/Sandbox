using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxDotNetv2._0._0._0
{
    public static class PermissionType
    {
        public const string SECURITY_PERMISSION_EXECUTION = "SecurityPermission - Execution";
        public const string SECURITY_PERMISSION_UNMANAGEDCODE = "SecurityPermission - UnmanagedCode";
        public const string FILE_IO_PERMISSION_READ = "FileIOPermission - Read";
        public const string FILE_IO_PERMISSION_WRITE = "FileIOPermission - Write";
        public const string SECURITY_PERMISSION_ALLACCESS = "FileIOPermission - AllAccess";
        public const string UI_PERMISSION = "UIPermission";
        public const string SANDBOX_PERMISSION_ALLACCESS = "CustomPermission - AllAccess";
        public const string REFLECTION_PERMISSION_EMIT = "ReflectionPermission - ReflectionEmit";
        public const string REFLECTION_PERMISSION_REFLECTION = "ReflectionPermission - Reflection";
        public const string WEB_PERMISSION_NETWORK = "WebPermission - NetworkAccess";
        public const string REGISTRY_PERMISSION_READ = "RegistryPermission - Read";
        public const string FILE_DIALOG_PERMISSION = "FileDialogPermission - Open";
        public const string REGISTRY_PERMISSION_WRITE = "RegistryPermission - Write";
        public const string REGISTRY_PERMISSION_ALLACCESS = "RegistryPermission - AllAccess";
        public const string PRINCIPAL_PERMISSION = "PrincipalPermission";
        public const string ENVIRONMENT_PERMISSION_COMMANDLINE = "EnvironmentPermission - Read CommandLine";
        public const string ENVIRONMENT_PERMISSION_ALLACCESS = "EnvironmentPermission - AllAcesss";
    }
}
