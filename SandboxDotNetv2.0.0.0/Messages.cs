using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxDotNetv2._0._0._0
{
    public static class Messages
    {
        public const string InvalidInput = "Invalid file input, file path not specified. Please try again.";
        public const string Attention = "Action needed!!!";
        public const string SelectPermmission = "Please select an item before moving it.";
        public const string EnablePermmission = "Turn on Enable Permission button before moving permission.";
        public const string EnableAccess = "Turn on Enable Access Policy button before moving permission.";
        public const string PermissionNeeded = "Application requires permission to access resource. \n It ";
        public const string CustomPermissionNeeded = " Could not find permission consider adding \n 'CustomPermission - AllAccess' in available permission";
        public const string OperationFailed = "Error";
        public const string Information = "Information";
        public const string InvalidPermission = "Invalid permission type";
        public const string About = "About\r\n\r\nWindows Sandbox Tool\r\n\r\nis a lightweight desktop environment tailored for safely running applications in isolation. This document provides a user guide for the Sandboxing Tool, which is designed to execute untrusted code in a secure environment. The tool can safely execute programs that may pose a risk to the system, such as malware, untested code, or unverified third-party software.\r\nUsing the C# .NET approach the tool can be executed from the command line or using the graphical user interface (GUI). It supports a variety of executable files and it allows you to set custom permissions for the sandboxed code.\r\n";
      
    }
}
