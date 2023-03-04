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
  public  class Sandboxer : MarshalByRefObject
    {
        public void AppDomainSetup(string pathToUntrusted, string untrustedAssembly, string[] parameters, PermissionSet permissionSet)
        {
            try
            {
                // Setting the AppDomainSetup. It is very important to set the ApplicationBase to a folder
                // other than the one in which the sandboxer resides.
                AppDomainSetup adSetup = new AppDomainSetup();
                if (pathToUntrusted == "") throw new FileLoadException("The path to file not defined");

                adSetup.ApplicationBase = Path.GetFullPath(pathToUntrusted);

                // Setting the permissions for the AppDomain. We give the permission to execute and to
                // read/discover the location where the untrusted code is loaded.

                // We want the sandboxer assembly's strong name, so that we can add it to the full trust list.
                StrongName fullTrustAssembly = typeof(Sandboxer).Assembly.Evidence.GetHostEvidence<StrongName>();

                // Now we have everything we need to create the AppDomain, so let's create it.
                AppDomain newDomain = AppDomain.CreateDomain("Sandbox", null, adSetup, permissionSet, fullTrustAssembly);

                // Use CreateInstanceFrom to load an instance of the Sandboxer class into the
                // new AppDomain.
                ObjectHandle handle = Activator.CreateInstanceFrom(
                    newDomain, typeof(Sandboxer).Assembly.ManifestModule.FullyQualifiedName,
                    typeof(Sandboxer).FullName
                );
                // Unwrap the new domain instance into a reference in this domain and use it to execute the
                // untrusted code.
                Sandboxer newDomainInstance = (Sandboxer)handle.Unwrap();

                newDomainInstance.ExecuteUntrustedCode(untrustedAssembly, parameters);
            }
            catch (FileLoadException fe)
            {
                MessageBox.Show(fe.Message, "Action needed!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Application requires permission to access resource.", "Action needed!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public void ExecuteUntrustedCode(string assemblyName, string[] parameters)
        {
            // Load the MethodInfo for a method in the new Assembly. This might be a method you know, or
            // you can use Assembly.EntryPoint to get to the main function in an executable.
            MethodInfo target = Assembly.Load(assemblyName).EntryPoint;
            try
            {
                // Now invoke the method.
                // bool retVal = (bool)target.Invoke(null, parameters);
                target.Invoke(null, new object[] { parameters });

            }
            catch (Exception ex)
            {
                // When we print informations from a SecurityException extra information can be printed if we are
                // calling it with a full-trust stack.
                new PermissionSet(PermissionState.Unrestricted).Assert();
                Console.WriteLine("SecurityException caught:\n{0}", ex.ToString());
                CodeAccessPermission.RevertAssert();
                Console.ReadLine();
            }
        }

    }
}