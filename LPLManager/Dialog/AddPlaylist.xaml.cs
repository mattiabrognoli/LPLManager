using LPLManager.Controller;
using System.Windows;
using System.Windows.Controls;

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
            if (!Controller.AddNewPlaylist(cmbPlaylists.Text))
                MessageBox.Show("Error!\nPlaylist already exist");
            else
            {
                MessageBox.Show("Playlist added!");
                Close();
            }
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
