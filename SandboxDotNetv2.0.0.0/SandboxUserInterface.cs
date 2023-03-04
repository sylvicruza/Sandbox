using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;

namespace SandboxDotNetv2._0._0._0
{

    public partial class SandboxUserInterface : Form
    {
        private string pathToUntrusted;
        private string untrustedClass;

        //..
        private readonly PermissionSet permissions = new PermissionSet(PermissionState.None);
        private readonly Sandboxer sandbox = new Sandboxer();
        //..


        public SandboxUserInterface()
        {
            InitializeComponent();
        }
        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pathToUntrusted = Path.GetDirectoryName(openFileDialog1.FileName);
                untrustedClass = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                pathTextBox.Text = pathToUntrusted;

            }
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            string path = pathTextBox.Text; //path to the file
            string argument = argumentTextBox.Text;

            string[] arguments = new string[] { argument };
            Array.ForEach(arguments, s => Console.WriteLine(s));

            sandbox.AppDomainSetup(path, untrustedClass, arguments, permissions);
            
        }

        private void AddPermissionButton_Click(object sender, EventArgs e)
        {
            // Grant additional permissions based on selected items
            PermissionManager();
            MovePermissionToRight();

        }

        private void PermissionManager()
        {
            // ...
            foreach (var itemChecked in checkedListBox1.CheckedItems)
            {
                var permissionType = itemChecked.ToString();
                var permission = PermissionFactory.CreatePermission(permissionType);
                permissions.AddPermission(permission);
            }
            // ...
        }
        private void MovePermissionToRight()
        {
            // Check if an item is selected in the first CheckBoxList
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                // Get the selected item
                string selectedItem = checkedListBox1.CheckedItems[0].ToString();
                // Remove the selected item from the first CheckBoxList
                checkedListBox1.Items.Remove(selectedItem);
                // Add the selected item to the second CheckBoxList
                checkedListBox2.Items.Add(selectedItem);
                Console.WriteLine(selectedItem);
            }
            else
            {
                // Display a message to the user indicating that they need to select an item before moving it
                MessageBox.Show("Please select an item before moving it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }   

        private void MovePermissionToLeft()
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

        private void RemovePermissionButton_Click(object sender, EventArgs e)
        {
            MovePermissionToLeft();
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

        //clear parameter
        private void button2_Click(object sender, EventArgs e)
        {
            pathTextBox.Text = "";
            argumentTextBox.Text = "";
        }
    }
}