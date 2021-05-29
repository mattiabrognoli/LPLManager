using LPLManager.Controller;
using LPLManager.Object;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace LPLManager
{
    /// <summary>
    /// Logica di interazione per AddItem.xaml
    /// </summary>
    public partial class AddItem : Window
    {
        #region [PROPERTIES]
        public PlaylistController Controller { get; set; }
        #endregion

        public AddItem()
        {
            InitializeComponent();
        }

        private void btnSetPath_Click(object sender, RoutedEventArgs e)
        {
            OpenDialogPath(txtPath, txtLabel);
        }

        private void btnSetCorePath_Click(object sender, RoutedEventArgs e)
        {
            OpenDialogPath(txtCorePath);
        }

        private void OpenDialogPath(TextBox control, TextBox secondControl = null)
        {
            OpenFileDialog openFileDialogPath = new OpenFileDialog();
            openFileDialogPath.Filter = "All files|*";
            if (openFileDialogPath.ShowDialog() == true)
            {
                control.Text = openFileDialogPath.FileName;
                if (secondControl != null)
                    secondControl.Text = Path.GetFileNameWithoutExtension(control.Text);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            bool check = true;

            if (string.IsNullOrEmpty(txtPath.Text))
            {
                vldPath.Visibility = Visibility.Visible;
                check = false;
            }

            if (string.IsNullOrEmpty(txtLabel.Text))
            {
                vldLabel.Visibility = Visibility.Visible;
                check = false;
            }

            if (check)
            {
                Controller.AddItem(new Item()
                {
                    path = txtPath.Text,
                    label = txtLabel.Text,
                    core_path = txtCorePath.Text,
                    core_name = txtCoreName.Text,
                    crc32 = txtCrc32.Text,
                    db_name = txtDbName.Text

                }, Controller.CurrentDatabase);
                MessageBox.Show("Item added to playlist");
                Controller.ReloadDatabase(Controller.CurrentDatabase);
                Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtPath.Text = "";
            txtLabel.Text = "";
            txtCorePath.Text = "DETECT";
            txtCoreName.Text = "DETECT";
            txtCrc32.Text = "";
            txtDbName.Text = Controller.CurrentDatabase + ".lpl";
        }

        private void txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((sender as TextBox).Name == "txtPath")
                vldPath.Visibility = Visibility.Hidden;

            if ((sender as TextBox).Name == "txtLabel")
                vldLabel.Visibility = Visibility.Hidden;
        }
    }
}
