using LPLManager.com;
using LPLManager.Controller;
using LPLManager.Object;
using LPLManager.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LPLManager.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        string _selectedChoice;
        int _currentId;
        bool _isInEdit;
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
            set
            {
                _selectedChoice = value;
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
                    TempItem = new Item()
                    {
                        core_name = value.core_name,
                        core_path = value.core_path,
                        crc32 = value.crc32,
                        db_name = value.db_name,
                        label = value.label,
                        path = value.path
                    };
                }
                else
                {
                    _currentId = -1;
                }
                OnPropertyChanged(nameof(TextBoxCanBeEnabled));
                OnPropertyChanged();
            }
        }


        #endregion

        public ICommand _txtChanged;

        public ICommand TxtChanged
        {
            get
            {
                if (_txtChanged == null)
                {
                    _txtChanged = new RelayCommand(param => this.ChangedText());
                }
                return _txtChanged;
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
            Controller.LoadModel();
            _currentId = -1;
            List<string> list = new List<string>();
            Controller.Model.Playlists.Keys.ToList().ForEach(k => list.Add(k));
            PlaylistName = list;
            //TxtChanged = new RelayCommand(param => ChangedText());
        }

        private async void ChangedText()
        {
            if (CurrentItem != null && TempItem != null)
                IsInEdit = !await new MainWindowService().CheckCurrentWithTemp(CurrentItem, TempItem);    
        }

    }
}
