using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SandboxDotNetv2._0._0._0
{
    public class Sandboxer : MarshalByRefObject
    {

        public void SetupAndRun(string pathToUntrusted, string untrustedAssembly, string[] parameters, PermissionSet permissionSet)
        {
            try
            {
                AppDomainSetup adSetup = CreateAppDomainSetup(pathToUntrusted);
                StrongName fullTrustAssembly = typeof(Sandboxer).Assembly.Evidence.GetHostEvidence<StrongName>();
                ObjectHandle handle = CreateSandboxAppDomain(permissionSet, adSetup, fullTrustAssembly);
                ExecuteInSandboxedDomain(untrustedAssembly, parameters, handle);

            }
            catch (Exception ex)
            {
                string message = ex.Message;
                MessageBox.Show("Application requires permission to access resource. \n It " + message, "Action needed!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);               
            }

        }

        private static AppDomainSetup CreateAppDomainSetup(string pathToUntrusted)
        {
            AppDomainSetup adSetup = new AppDomainSetup();
            adSetup.ApplicationBase = Path.GetFullPath(pathToUntrusted);
            return adSetup;
        }
        private static ObjectHandle CreateSandboxAppDomain(PermissionSet permissionSet, AppDomainSetup adSetup, StrongName fullTrustAssembly)
        {
            AppDomain newDomain = AppDomain.CreateDomain("Sandbox", null, adSetup, permissionSet, fullTrustAssembly);
            ObjectHandle handle = Activator.CreateInstanceFrom(
                newDomain, typeof(Sandboxer).Assembly.ManifestModule.FullyQualifiedName,
                typeof(Sandboxer).FullName
            );
            return handle;
        }

        private static void ExecuteInSandboxedDomain(string untrustedAssembly, string[] parameters, ObjectHandle handle)
        {
            Sandboxer newDomainInstance = (Sandboxer)handle.Unwrap();
            newDomainInstance.ExecuteUntrustedCode(untrustedAssembly, parameters);
        }

        public void ExecuteUntrustedCode(string assemblyName, string[] parameters)
        {
            MethodInfo target = Assembly.Load(assemblyName).EntryPoint;
            try
            {
                // Now invoke the method.
                target.Invoke(null, new object[] { parameters });

            }
            catch (Exception ex)
            {
                // When we print informations from a SecurityException extra information can be printed if we are
                // calling it with a full-trust stack.
                Console.WriteLine("SecurityException caught:\n{0}", ex.ToString());
                MessageBox.Show(ex.Message);              
                new PermissionSet(PermissionState.Unrestricted).Assert();            
                CodeAccessPermission.RevertAssert();
                return;

                
            }
        }

    }
}