using LPLManager.com;
using LPLManager.Controller;
using LPLManager.Dialog;
using LPLManager.FileManager;
using LPLManager.Object;
using LPLManager.Service;
using LPLManager.Uility;
using LPLManager.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LPLManager.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        string _selectedChoice;
        int _currentCbIndex;
        int _currentItemIndex;
        int _currentId;
        bool _isInEdit;
        bool _isRemoveDbEnabled;
        bool _isRemoveEnabled;
        bool _isAddEnabled;
        ImageSource _currentImage;
        Item _tempItem;
        PlaylistController _controller;
        List<Item> _sourceItems;
        List<string> _playlistName;

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

        public ImageSource CurrentImage
        {
            get
            {
                return _currentImage;
            }
            set
            {
                _currentImage = value;
                OnPropertyChanged();
            }
        }

        public bool IsRemoveDbEnabled
        {
            get
            {
                return _isRemoveDbEnabled;
            }
            set
            {
                _isRemoveDbEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsRemoveEnabled
        {
            get
            {
                return _isRemoveEnabled;
            }
            set
            {
                _isRemoveEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsAddEnabled
        {
            get
            {
                return _isAddEnabled;
            }
            set
            {
                _isAddEnabled = value;
                OnPropertyChanged();
            }
        }

        public int CurrentCbIndex
        {
            get
            {
                return _currentCbIndex;
            }
            set
            {
                _currentCbIndex = value;
                OnPropertyChanged();
            }
        }

        public int CurrentItemIndex
        {
            get
            {
                return _currentItemIndex;
            }
            set
            {
                _currentItemIndex = value;
                OnPropertyChanged();
            }
        }

        public bool IsInEdit
        {
            get
            {
                return _isInEdit;
            }
            set
            {
                _isInEdit = value;
                OnPropertyChanged();
            }
        }

        public string SelectedChoice
        {
            get
            {
                return _selectedChoice;
            }
            set
            {
                _selectedChoice = value;
                IsAddEnabled = IsRemoveDbEnabled = !string.IsNullOrEmpty(value);
                LoadItems(_selectedChoice);
                OnPropertyChanged();
            }
        }

        public bool TextBoxCanBeEnabled
        {
            get
            {
                return _currentId != -1;
            }
        }

        public List<string> PlaylistName
        {
            get
            {
                return _playlistName;
            }
            set
            {
                _playlistName = value;
                OnPropertyChanged();
            }
        }

        public List<Item> SourceItems
        {
            get
            {
                return _sourceItems;
            }
            set
            {
                _sourceItems = value;
                OnPropertyChanged();
            }
        }

        public Item TempItem
        {
            get
            {
                return _tempItem;
            }
            set
            {
                _tempItem = value;
                OnPropertyChanged();
            }
        }

        public Item CurrentItem
        {
            get
            {
                return !string.IsNullOrEmpty(_selectedChoice) ? Controller.Model.Playlists[_selectedChoice].items.Where(i => i.id == _currentId).SingleOrDefault() : null;
            }
            set
            {
                if (value != null)
                {
                    _currentId = value.id;
                    IsRemoveEnabled = true;
                    TempItem = value.Clone();
                }
                else
                {
                    _currentId = -1;
                    IsRemoveEnabled = false;
                    TempItem = new Item();
                }
                OnPropertyChanged(nameof(TextBoxCanBeEnabled));
                OnPropertyChanged();
            }
        }


        #endregion

        ICommand _txtChanged;

        ICommand _resetCommand;

        ICommand _editCommand;

        ICommand _removeCommand;

        ICommand _removePlaylistCommand;

        ICommand _addCommand;

        ICommand _addPlaylistCommand;

        public ICommand TxtChanged
        {
            get
            {
                if (_txtChanged == null)
                {
                    _txtChanged = new RelayCommand(param => ChangedText());
                }
                return _txtChanged;
            }
        }

        public ICommand ResetCommand
        {
            get
            {
                if (_resetCommand == null)
                    _resetCommand = new RelayCommand(param => Reset());

                return _resetCommand;
            }
        }

        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                    _editCommand = new RelayCommand(param => Edit());

                return _editCommand;
            }
        }

        public ICommand RemoveCommand
        {
            get
            {
                if (_removeCommand == null)
                    _removeCommand = new RelayCommand(param => Remove());

                return _removeCommand;
            }
        }

        public ICommand RemovePlaylistCommand
        {
            get
            {
                if (_removePlaylistCommand == null)
                    _removePlaylistCommand = new RelayCommand(param => RemovePlaylist());

                return _removePlaylistCommand;
            }
        }

        public ICommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                    _addCommand = new RelayCommand(param => Add());

                return _addCommand;
            }
        }

        public ICommand AddPlaylistCommand
        {
            get
            {
                if (_addPlaylistCommand == null)
                    _addPlaylistCommand = new RelayCommand(param => AddPlaylist());

                return _addPlaylistCommand;
            }
        }

        void LoadItems(string currentChoice)
        {
            if (!string.IsNullOrEmpty(currentChoice))
            {
                SourceItems = Controller.Model.Playlists[currentChoice].items;
            }
        }

        public MainWindowViewModel()
        {
            CurrentCbIndex = -1;
            _currentId = -1;
            Controller.LoadModel();
            List<string> list = new List<string>();
            Controller.Model.Playlists.Keys.ToList().ForEach(k => list.Add(k));
            PlaylistName = list;
        }

        private async void ChangedText()
        {
            if (CurrentItem != null && TempItem != null)
            {
                IsInEdit = !await new MainWindowService().CheckCurrentWithTemp(CurrentItem, TempItem);

                string pathImage = Directory.GetCurrentDirectory() + "/thumbnails/" + SelectedChoice + "/Named_Boxarts/" + CurrentItem.label + ".png";

                if (CurrentImage != null)
                    CurrentImage = null;
                if (Controller.isCustom == false)
                    if (File.Exists(pathImage))
                    {
                        CurrentImage = FileManagers.FileManager.LoadImage(pathImage);
                    }
                    else
                    {
                        int width = 128;
                        int height = width;
                        int stride = width / 8;
                        byte[] pixels = new byte[height * stride];
                        CurrentImage = BitmapSource.Create(
                                                 width, height,
                                                 96, 96,
                                                 PixelFormats.Indexed1,
                                                 new BitmapPalette(new List<Color>() { Colors.LightGray }),
                                                 pixels,
                                                 stride);
                    }
            }
        }

        private void Edit()
        {
            DialogeWarning dialog = new DialogeWarning();

            MessageBoxResult result = MessageBox.Show("Are you sure to edit?", "Edit", MessageBoxButton.YesNo, MessageBoxImage.Question);

            //dialog.DataContext = new WarningDialogViewModel() { Description = "TestView" };

            //dialog.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            //dialog.ShowDialog();

            if (result == MessageBoxResult.Yes)
            {
                //if (Controller.isCustom == false && Controller.isImageEdit)
                //{
                //    Controller.SaveImage(picItem.Source as BitmapImage, currentItem.label, txtLabel.Text);
                //}
                Item itemToEdit = Controller.Model.Playlists[SelectedChoice].items.SingleOrDefault(i => i.id == _currentId);
                int tempIndex = CurrentItemIndex;

                itemToEdit.core_name = TempItem.core_name;
                itemToEdit.core_path = TempItem.core_path;
                itemToEdit.crc32 = TempItem.crc32;
                itemToEdit.db_name = TempItem.db_name;
                itemToEdit.label = TempItem.label;
                itemToEdit.path = TempItem.path;

                //if (Controller.isCustom)
                //    FileJson<Root>.Write(Controller.customPath, Controller.Model.Playlists[Controller.CurrentDatabase]);
                //else
                FileJson<Root>.Write(@"playlists\" + SelectedChoice + ".lpl", Controller.Model.Playlists[SelectedChoice]);
                MessageBox.Show("Playlist updated");
                IsInEdit = false;
                //if (Controller.isCustom)
                //    Controller.LoadCustomDatabase(Controller.customPath);
                //else
                Controller.ReloadDatabase(SelectedChoice);
                SourceItems = Controller.Model.Playlists[SelectedChoice].items;
                CurrentItemIndex = tempIndex;
            }
        }

        void Reset()
        {
            _currentId = -1;
            CurrentCbIndex = -1;
            IsAddEnabled = false;
            IsRemoveEnabled = false;
            IsInEdit = false;
            SourceItems = new List<Item>();
            CurrentItem = null;
            SelectedChoice = string.Empty;

            Controller.LoadModel();
            List<string> list = new List<string>();
            Controller.Model.Playlists.Keys.ToList().ForEach(k => list.Add(k));
            PlaylistName = list;

        }

        void Remove()
        {

            Item currentItem = Controller.Model.Playlists[SelectedChoice].items.SingleOrDefault(i => i.id == _currentId);
            MessageBoxResult result = MessageBox.Show("Are you sure to remove it?", "Remove", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes && currentItem != null)
            {
                bool deleteItem = false;
                if (File.Exists(currentItem.path))
                    deleteItem = MessageBox.Show("Do you wanna remove item too?", "Remove item", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;

                Controller.RemoveItem(currentItem, SelectedChoice, removeFile: deleteItem);
                //if (Controller.isCustom)
                //    FileJson<Root>.Write(Controller.customPath, Controller.Model.Playlists[Controller.CurrentDatabase]);
                //else
                FileJson<Root>.Write(@"playlists\" + SelectedChoice + ".lpl", Controller.Model.Playlists[SelectedChoice]);
                MessageBox.Show("Element removed");

                Controller.ReloadDatabase(SelectedChoice);
                SourceItems = Controller.Model.Playlists[SelectedChoice].items;
            }
        }

        void RemovePlaylist()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to remove the playlist?", "Remove Playlist", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                //string path = Controller.isCustom ? Controller.customPath : "playlists\\" + Controller.CurrentDatabase + ".lpl";
                string path = "playlists\\" + SelectedChoice + ".lpl";
                FileManagers.FileManager.DeleteFile(path);
                MessageBox.Show("Playlist Removed!");
                Reset();
            }
        }

        void Add()
        {
            AddItem addUI = new AddItem();
            addUI.Owner = Context.mainWindow;
            addUI.Controller = Controller;
            addUI.Controller.CurrentDatabase = SelectedChoice;
            addUI.ShowDialog();
            if (Controller.isAdded)
            {
                Controller.isAdded = false;
                _currentId = -1;
                SourceItems = Controller.Model.Playlists[SelectedChoice].items;
            }
        }

        void AddPlaylist()
        {
            AddPlaylist addUI = new AddPlaylist();
            addUI.Owner = Context.mainWindow;
            addUI.Controller = Controller;
            addUI.Controller.CurrentDatabase = SelectedChoice;
            addUI.ShowDialog();
            if (Controller.isAdded)
            {
                _currentId = -1;
                Controller.isAdded = false;
                Reset();
            }
        }
    }
}
