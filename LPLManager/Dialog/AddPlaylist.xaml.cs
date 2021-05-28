using LPLManager.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LPLManager.Dialog
{
    /// <summary>
    /// Logica di interazione per AddPlaylist.xaml
    /// </summary>
    public partial class AddPlaylist : Window
    {
        public PlaylistController Controller { get; set; }

        public AddPlaylist()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbPlaylists.BeginInit();
            cmbPlaylists.Items.Clear();
            cmbPlaylists.ItemsSource = Controller.GetPlaylistNames();
            cmbPlaylists.EndInit();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Controller.AddNewPlaylist(cmbPlaylists.Text);
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cmbPlaylists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnOk.IsEnabled = !string.IsNullOrEmpty((sender as ComboBox).SelectedItem as string);
        }
    }
}
