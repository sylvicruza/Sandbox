using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;

namespace SandboxDotNetv2._0._0._0
{

    public partial class Form1 : Form
    {
        string pathToUntrusted;
        string untrustedClass;

        // Declare the PermissionSet globally
        private PermissionSet permissions = new PermissionSet(PermissionState.None);


        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pathToUntrusted = Path.GetDirectoryName(openFileDialog1.FileName);
                untrustedClass = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                textBox1.Text = pathToUntrusted;


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string path = textBox1.Text; //path to the file
            string argument = textBox2.Text;

            string[] arguments = new string[] { argument };
            Array.ForEach(arguments, s => Console.WriteLine(s));

            Sandboxer sandbox = new Sandboxer();
            sandbox.AppDomainSetup(path, untrustedClass, arguments, permissions);
            // textBox3.Text = sandbox.Output;
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

            //permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));

            // Grant additional permissions based on selected items
            foreach (object itemChecked in checkedListBox1.CheckedItems)
            {
                Console.WriteLine(itemChecked.ToString());

                switch (itemChecked.ToString())
                {
                    case "SecurityPermission - Execution":
                        permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
                        break;
                    case "FileIOPermission - Read":
                        permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Read, AppDomain.CurrentDomain.BaseDirectory));
                        break;
                    case "FileIOPermission - Write":
                        permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Write, AppDomain.CurrentDomain.BaseDirectory));
                        break;
                    case "FileIOPermission - AllAccess":
                        permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.AllAccess, AppDomain.CurrentDomain.BaseDirectory));
                        break;
                    case "UIPermission":
                        permissions.AddPermission(new UIPermission(UIPermissionWindow.SafeTopLevelWindows, UIPermissionClipboard.AllClipboard));
                        break;
                    case "ReflectionPermission - ReflectionEmit":
                        permissions.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.ReflectionEmit));
                        break;
                    case "FileDialogPermission":
                        permissions.AddPermission(new FileDialogPermission(FileDialogPermissionAccess.Open));
                        break;
                    case "RegistryPermission - Read":
                        permissions.AddPermission(new RegistryPermission(RegistryPermissionAccess.Read, "HKEY_LOCAL_MACHINE\\SOFTWARE"));
                        break;
                    case "RegistryPermission - Write":
                        permissions.AddPermission(new RegistryPermission(RegistryPermissionAccess.Write, "HKEY_LOCAL_MACHINE\\SOFTWARE"));
                        break;
                    case "RegistryPermission - AllAccess":
                        permissions.AddPermission(new RegistryPermission(RegistryPermissionAccess.AllAccess, "HKEY_LOCAL_MACHINE\\SOFTWARE"));
                        break;

                    case "PrincipalPermission":
                        permissions.AddPermission(new PrincipalPermission(null, "Administrators"));
                        break;
                    case "EnvironmentPermission":
                        permissions.AddPermission(new EnvironmentPermission(EnvironmentPermissionAccess.AllAccess, "PATH"));
                        break;

                        // Add more cases for other permissions as needed
                }
            }

            // Check if an item is selected in the first CheckBoxList
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                // Get the selected item
                string selectedItem = checkedListBox1.CheckedItems[0].ToString();

                // Remove the selected item from the first CheckBoxList
                checkedListBox1.Items.Remove(selectedItem);

                // Add the selected item to the second CheckBoxList
                checkedListBox2.Items.Add(selectedItem);
            }
            else
            {
                // Display a message to the user indicating that they need to select an item before moving it
                MessageBox.Show("Please select an item before moving it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        class Sandboxer : MarshalByRefObject
        {
            public void AppDomainSetup(string pathToUntrusted, string untrustedAssembly, string[] parameters, PermissionSet permissionSet)
            {
                //Setting the AppDomainSetup. It is very important to set the ApplicationBase to a folder
                //other than the one in which the sandboxer resides.
                AppDomainSetup adSetup = new AppDomainSetup();
                adSetup.ApplicationBase = Path.GetFullPath(pathToUntrusted);

                //Setting the permissions for the AppDomain. We give the permission to execute and to
                //read/discover the location where the untrusted code is loaded.
                // PermissionSet permSet = new PermissionSet(PermissionState.None);
                // permSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));

                //We want the sandboxer assembly's strong name, so that we can add it to the full trust list.
                StrongName fullTrustAssembly = typeof(Sandboxer).Assembly.Evidence.GetHostEvidence<StrongName>();

                //Now we have everything we need to create the AppDomain, so let's create it.
                AppDomain newDomain = AppDomain.CreateDomain("Sandbox", null, adSetup, permissionSet, fullTrustAssembly);

                //Use CreateInstanceFrom to load an instance of the Sandboxer class into the
                //new AppDomain.
                ObjectHandle handle = Activator.CreateInstanceFrom(
                    newDomain, typeof(Sandboxer).Assembly.ManifestModule.FullyQualifiedName,
                    typeof(Sandboxer).FullName
                    );
                //Unwrap the new domain instance into a reference in this domain and use it to execute the
                //untrusted code.
                Sandboxer newDomainInstance = (Sandboxer)handle.Unwrap();
                try
                {
                    newDomainInstance.ExecuteUntrustedCode(untrustedAssembly, parameters);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            public void ExecuteUntrustedCode(string assemblyName, string[] parameters)
            {
                //Load the MethodInfo for a method in the new Assembly. This might be a method you know, or
                //you can use Assembly.EntryPoint to get to the main function in an executable.
                MethodInfo target = Assembly.Load(assemblyName).EntryPoint;
                try
                {
                    //Now invoke the method.
                    // bool retVal = (bool)target.Invoke(null, parameters);
                    target.Invoke(null, new object[] { parameters });

                }
                catch (Exception ex)
                {
                    // When we print informations from a SecurityException extra information can be printed if we are
                    //calling it with a full-trust stack.
                    new PermissionSet(PermissionState.Unrestricted).Assert();
                    Console.WriteLine("SecurityException caught:\n{0}", ex.ToString());
                    CodeAccessPermission.RevertAssert();
                    Console.ReadLine();
                }
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Check if an item is selected in the first CheckBoxList
            if (checkedListBox2.CheckedItems.Count > 0)
            {
                // Get the selected item
                string selectedItem = checkedListBox2.CheckedItems[0].ToString();

                // Remove the selected item from the first CheckBoxList
                checkedListBox2.Items.Remove(selectedItem);

                // Add the selected item to the second CheckBoxList
                checkedListBox1.Items.Add(selectedItem);
            }
            else
            {
                // Display a message to the user indicating that they need to select an item before moving it
                MessageBox.Show("Please select an item before moving it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

            string searchString = textBox4.Text;


            SearchCheckedListBox(checkedListBox1, searchString);
            SearchCheckedListBox(checkedListBox2, searchString);
        }

        private void SearchCheckedListBox(CheckedListBox checkedListBox, string searchString)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                string text = checkedListBox.Items[i].ToString();

                if (text.ToLower().Contains(searchString.ToLower()))
                {
                    checkedListBox.SetItemChecked(i, true);
                }
                else
                {
                    checkedListBox.SetItemChecked(i, false);
                }
            }
        }

    }
}