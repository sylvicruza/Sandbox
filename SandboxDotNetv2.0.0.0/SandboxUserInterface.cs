using System;
using System.Collections.Generic;
using System.Drawing;
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
            if (string.IsNullOrEmpty(path)) { 
                MessageBox.Show(Messages.InvalidInput, Messages.Attention, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (radioButton1.Checked)
            {
                Console.WriteLine("Enable policy access");
                sandbox.SetupAndRun(path, untrustedClass, arguments, SandboxAccessPolicy.UpdatePermissionSet(permissions));
            }

            sandbox.SetupAndRun(path, untrustedClass, arguments, PermissionFactory.PermissionBuilder(permissions));

        }

        private void AddPermissionButton_Click(object sender, EventArgs e)
        {
            // Grant additional permissions based on selected items
            if (radioButton2.Checked)
            {
                PermissionManager();
                MovePermission(checkedListBox1, checkedListBox2);
            }
            else
            {  // Display a message to the user indicating that they need to select an item before moving it
                MessageBox.Show(Messages.EnablePermmission, Messages.OperationFailed, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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
        private void MovePermission(CheckedListBox moveRight, CheckedListBox moveLeft)
        {
            // Check if an item is selected in the first CheckBoxList
            if (moveRight.CheckedItems.Count > 0)
            {
                List<string> selectedItems = new List<string>();
                foreach (var itemChecked in moveRight.CheckedItems)
                {
                      selectedItems.Add(itemChecked.ToString());
                }
               
                selectedItems.ForEach(itemChecked => {
                    moveRight.Items.Remove(itemChecked);
                moveLeft.Items.Add(itemChecked);
                });
                
            }
            else
            {
                // Display a message to the user indicating that they need to select an item before moving it
                MessageBox.Show(Messages.SelectPermmission, Messages.OperationFailed, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      

        private void RemovePermissionButton_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                MovePermission(checkedListBox2, checkedListBox1);
            }
            else
            {  // Display a message to the user indicating that they need to select an item before moving it
                MessageBox.Show(Messages.EnablePermmission, Messages.OperationFailed, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }



        private void SearchCheckedListBox(CheckedListBox checkedListBox)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                if (checkBox1.Checked)
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

        private void SandboxUserInterface_Load(object sender, EventArgs e)
        {
            // Get the width and height of the screen
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            // Set the width and height of the form to two-thirds of the screen width and height, respectively
            this.Width = (int)(screenWidth * 0.85);
            this.Height = (int)(screenHeight * 1);

            // Center the form on the screen
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void changeBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.BackColor = colorDialog.Color;
            }


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SearchCheckedListBox(checkedListBox1);

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SearchCheckedListBox(checkedListBox2);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Messages.About, Messages.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

      

        private void checkedListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void button6_Click(object sender, EventArgs e)
        {
           
           
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                MovePermission(checkedListBox3, checkedListBox4);
                foreach (var itemChecked in checkedListBox3.CheckedItems)
                {
                    var accessType = itemChecked.ToString();
                    var permission = SandboxAccessPolicy.UpdatePermission(accessType);
                    permissions.AddPermission(permission);
                }
            }
            else
            {
                // Display a message to the user indicating that they need to select an item before moving it
                MessageBox.Show(Messages.EnableAccess, Messages.OperationFailed, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                MovePermission(checkedListBox4, checkedListBox3);
            }
            else
            {  // Display a message to the user indicating that they need to select an item before moving it
                MessageBox.Show(Messages.EnableAccess, Messages.OperationFailed, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}