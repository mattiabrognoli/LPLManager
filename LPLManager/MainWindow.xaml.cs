using LPLManager.Controller;
using LPLManager.Dialog;
using LPLManager.FileManager;
using LPLManager.Object;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LPLManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region [PRIVATE VARIABLES]
        private PlaylistController _controller;
        #endregion

        #region [PROPERTIES]
        public PlaylistController Controller
        {
            get
            {
                if (_controller == null)
                    _controller = new PlaylistController();
                return _controller;
            }
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LPLManagerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Loading();
        }
        private void Loading()
        {
            ResetControls();
            Controller.LoadModel();
            LoadPlaylist();
        }

        private void LoadPlaylist()
        {
            //cmbPlaylist.BeginInit();
            //cmbPlaylist.Items.Clear();
            //Controller.Model.Playlists.Keys.ToList().ForEach(k => cmbPlaylist.Items.Add(k));
            //cmbPlaylist.EndInit();
        }

        private void ResetControls()
        {
            listItems.ItemsSource = new List<Item>();
            //cmbPlaylist.Items.Clear();
            txtPath.Text = "";
            txtLabel.Text = "";
            txtCorePath.Text = "";
            txtCoreName.Text = "";
            txtCrc32.Text = "";
            txtDbName.Text = "";
            //ChangeEnabledTextBox(false);
            //btnEdit.IsEnabled = false;
            btnRemove.IsEnabled = false;
            btnAddItem.IsEnabled = false;
            Controller.isCustom = false;
        }

        private void cmbPlaylist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ClearItem();
            if (!string.IsNullOrEmpty(cmbPlaylist.SelectedItem as string))
            {
                btnAddItem.IsEnabled = btnRemoveLPL.IsEnabled = true;
                Controller.CurrentDatabase = cmbPlaylist.SelectedItem as string;
                
                //listItems.ItemsSource = Controller.Model.Playlists[cmbPlaylist.SelectedItem as string].items;

                if (sender is Button && (sender as Button).Name == "btnEdit")
                    listItems.SelectedIndex = Controller.CurrentIndex;
            }
            else
            {
                btnAddItem.IsEnabled = btnRemoveLPL.IsEnabled = false;
                listItems.ItemsSource = new List<Item>();
            }
        }

        private void btnRemoveLPL_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to remove the playlist?", "Remove Playlist", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                string path = Controller.isCustom ? Controller.customPath : "playlists\\" + Controller.CurrentDatabase + ".lpl";
                FileManagers.FileManager.DeleteFile(path);
                MessageBox.Show("Playlist Removed!");
               Loading();
            }
        }

        private void ChangeEnabledTextBox(bool choice)
        {
            txtPath.IsEnabled =
            txtLabel.IsEnabled =
            txtCorePath.IsEnabled =
            txtCoreName.IsEnabled =
            txtCrc32.IsEnabled =
            btnSetPath.IsEnabled =
            btnSetCorePath.IsEnabled = choice;
        }

        private void ClearItem()
        {
            txtPath.Text =
            txtLabel.Text =
            txtCorePath.Text =
            txtCoreName.Text =
            txtCrc32.Text =
            txtDbName.Text = string.Empty;

            txtPath.IsEnabled =
            txtLabel.IsEnabled =
            txtCorePath.IsEnabled =
            txtCoreName.IsEnabled =
            txtCrc32.IsEnabled =
            txtDbName.IsEnabled =
            btnRemove.IsEnabled =
            btnEdit.IsEnabled =
            btnSetPath.IsEnabled =
            btnSetCorePath.IsEnabled =
            btnAddItem.IsEnabled = false;

            picItem.Source = null;
        }

        private void loadingInfo(Item currentItem)
        {
            txtPath.Text = currentItem.path;
            txtLabel.Text = currentItem.label;
            txtCorePath.Text = currentItem.core_path;
            txtCoreName.Text = currentItem.core_name;
            txtCrc32.Text = currentItem.crc32;
            txtDbName.Text = currentItem.db_name;
        }

        private void loadingImage(string label, string database)
        {
            string pathImage = Directory.GetCurrentDirectory() + "/thumbnails/" + database + "/Named_Boxarts/" + label + ".png";

            picItem.BeginInit();

            if (picItem.Source != null)
                picItem.Source = null;
            if (Controller.isCustom == false)
                if (File.Exists(pathImage))
                {
                    picItem.Source = FileManagers.FileManager.LoadImage(pathImage);
                }
                else
                {
                    int width = 128;
                    int height = width;
                    int stride = width / 8;
                    byte[] pixels = new byte[height * stride];
                    picItem.Source = BitmapSource.Create(
                                             width, height,
                                             96, 96,
                                             PixelFormats.Indexed1,
                                             new BitmapPalette(new List<Color>() { Colors.LightGray }),
                                             pixels,
                                             stride);
                }
            picItem.EndInit();
        }

        private void listItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listItems.SelectedIndex != -1 && !string.IsNullOrEmpty((listItems.SelectedItem as Item).label))
            {
                Item current = Controller.Model.Playlists[Controller.CurrentDatabase].items.SingleOrDefault(i => i.id == (listItems.SelectedItem as Item).id);
                if (current != null)
                {
                    btnRemove.IsEnabled = true;
                    ChangeEnabledTextBox(true);
                    Controller.CurrentItem = current;
                    loadingInfo(current);
                    loadingImage(current.label, cmbPlaylist.SelectedItem as string);
                }
            }
        }

        private void txtChangeEvent(object sender, TextChangedEventArgs e)
        {
            //if ((sender as TextBox).Name == "txtPath")
            //    vldPath.Visibility = Visibility.Hidden;

            //if ((sender as TextBox).Name == "txtLabel")
            //    vldLabel.Visibility = Visibility.Hidden;

            //Item actualItem = new Item()
            //{
            //    core_name = txtCoreName.Text,
            //    core_path = txtCorePath.Text,
            //    crc32 = txtCrc32.Text,
            //    db_name = txtDbName.Text,
            //    label = txtLabel.Text,
            //    path = txtPath.Text
            //};

            //btnEdit.IsEnabled = !Controller.CompareCurrententItem(actualItem);
        }

        #region [SAVE EVENTS]

        private void saveInfo(Item currentItem)
        {
            currentItem.path = txtPath.Text;
            currentItem.label = txtLabel.Text;
            currentItem.core_path = txtCorePath.Text;
            currentItem.core_name = txtCoreName.Text;
            currentItem.crc32 = txtCrc32.Text;
            currentItem.db_name = txtDbName.Text;
        }



        #endregion

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if ((listItems.SelectedItem as Item).label != "")
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
                    Item currentItem = Controller.Model.Playlists[Controller.CurrentDatabase].items.SingleOrDefault(i => i.id == (listItems.SelectedItem as Item).id);
                    MessageBoxResult result = MessageBox.Show("Are you sure to edit?", "Edit", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes && currentItem != null)
                    {
                        if (Controller.isCustom == false && picItem.Source != null && Controller.isImageEdit)
                        {
                            Controller.SaveImage(picItem.Source as BitmapImage, currentItem.label, txtLabel.Text);
                        }
                        saveInfo(currentItem);
                        if (Controller.isCustom)
                            FileJson<Root>.Write(Controller.customPath, Controller.Model.Playlists[Controller.CurrentDatabase]);
                        else
                            FileJson<Root>.Write(@"playlists\" + (cmbPlaylist.SelectedItem as string) + ".lpl", Controller.Model.Playlists[Controller.CurrentDatabase]);
                        MessageBox.Show("Playlist updated");
                        btnEdit.IsEnabled = false;
                        Controller.CurrentIndex = listItems.SelectedIndex;
                        if (Controller.isCustom)
                            Controller.LoadCustomDatabase(Controller.customPath);
                        else
                            Controller.ReloadDatabase(cmbPlaylist.SelectedItem as string);
                        cmbPlaylist_SelectionChanged(sender, e as SelectionChangedEventArgs);
                    }
                }
            }
        }

        private void picItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialogImage = new OpenFileDialog();
            openFileDialogImage.Filter = "Image Files|*.png;*.jpg;*.jpeg";
            if (openFileDialogImage.ShowDialog() == true)
            {
                try
                {
                    if (picItem.Source != null)
                        picItem.Source = null;

                    picItem.Source = LPLManager.FileManagers.FileManager.LoadImage(openFileDialogImage.FileName);
                    Controller.isImageEdit = true;
                    btnEdit.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        #region [REMOVE EVENTS]

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if ((listItems.SelectedItem as Item).label != "")
            {
                Item currentItem = Controller.Model.Playlists[Controller.CurrentDatabase].items.SingleOrDefault(i => i.id == (listItems.SelectedItem as Item).id);
                MessageBoxResult result = MessageBox.Show("Are you sure to remove it?", "Remove", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes && currentItem != null)
                {
                    bool deleteItem = false;
                    if (File.Exists(currentItem.path))
                        deleteItem = MessageBox.Show("Do you wanna remove item too?", "Remove item", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;

                    Controller.RemoveItem(currentItem, Controller.CurrentDatabase, removeFile: deleteItem);
                    if (Controller.isCustom)
                        FileJson<Root>.Write(Controller.customPath, Controller.Model.Playlists[Controller.CurrentDatabase]);
                    else
                        FileJson<Root>.Write(@"playlists\" + (cmbPlaylist.SelectedItem as string) + ".lpl", Controller.Model.Playlists[Controller.CurrentDatabase]);
                    MessageBox.Show("Element removed");
                    if (Controller.isCustom)
                        Controller.LoadCustomDatabase(Controller.customPath);
                    else
                        Controller.ReloadDatabase(cmbPlaylist.SelectedItem as string);
                    cmbPlaylist_SelectionChanged(sender, e as SelectionChangedEventArgs);
                }
            }
        }

        #endregion

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

        private void picItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialogImage = new OpenFileDialog();
            openFileDialogImage.Filter = "Image Files|*.png;*.jpg;*.jpeg";
            if (openFileDialogImage.ShowDialog() == true)
            {
                try
                {
                    if (picItem.Source != null)
                        picItem.Source = null;

                    picItem.Source = LPLManager.FileManagers.FileManager.LoadImage(openFileDialogImage.FileName);
                    Controller.isImageEdit = true;
                    btnEdit.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            AddItem addUI = new AddItem();
            addUI.Owner = this;
            addUI.Controller = Controller;
            addUI.ShowDialog();
            if (Controller.isAdded)
            {
                Controller.isAdded = false;
                cmbPlaylist_SelectionChanged(sender, e as SelectionChangedEventArgs);
            }
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, "Created by Mattia Brognoli (mattia.brognoli9@gmail.com)");
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialogImage = new OpenFileDialog();
            openFileDialogImage.Filter = "LPL Files|*.lpl";

            if (openFileDialogImage.ShowDialog() == true)
            {
                try
                {
                    ResetControls();
                    Controller.LoadCustomDatabase(openFileDialogImage.FileName);
                    LoadPlaylist();
                    cmbPlaylist.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }

        }

        private void MenuItemReset_Click(object sender, RoutedEventArgs e)
        {
            Loading();
        }

        private void MenuItemNew_Click(object sender, RoutedEventArgs e)
        {
            AddPlaylist addUI = new AddPlaylist();
            addUI.Owner = this;
            addUI.Controller = Controller;
            addUI.ShowDialog();
            if (Controller.isAdded)
            {
                Controller.isAdded = false;
                Loading();
            }
        }

        private void MenuItemHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, "Put the exe file in the retroarch root and start it.\nThen you can add, edit and remove LPL retroarch playlists.");
        }

        private void test(object sender, TextChangedEventArgs e)
        {

        }
    }
}