using LPLManager.com;
using LPLManager.Controller;
using LPLManager.FileManager;
using LPLManager.Object;
using LPLManager.Service;
using LPLManager.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
                IsRemoveDbEnabled = !string.IsNullOrEmpty(value);
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
                    TempItem = value.Clone();
                }
                else
                {
                    _currentId = -1;
                    TempItem = new Item();
                }
                OnPropertyChanged(nameof(TextBoxCanBeEnabled));
                OnPropertyChanged();
            }
        }


        #endregion

        private ICommand _txtChanged;

        private ICommand _resetCommand;

        private ICommand _editCommand;

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
                {
                    _resetCommand = new RelayCommand(param => Reset());
                }
                return _resetCommand;
            }
        }

        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                {
                    _editCommand = new RelayCommand(param => Edit());
                }
                return _editCommand;
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
                IsInEdit = !await new MainWindowService().CheckCurrentWithTemp(CurrentItem, TempItem);    
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

        private void Reset()
        {
            _currentId = -1;
            CurrentCbIndex = -1;
            IsInEdit = false;
            SourceItems = new List<Item>();
            CurrentItem = new Item();
            SelectedChoice = string.Empty;

            Controller.LoadModel();
            List<string> list = new List<string>();
            Controller.Model.Playlists.Keys.ToList().ForEach(k => list.Add(k));
            PlaylistName = list;

        }
    }
}
