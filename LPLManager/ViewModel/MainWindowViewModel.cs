using LPLManager.Controller;
using LPLManager.Object;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LPLManager.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region [PRIVATE VARIABLES]
        private PlaylistController _controller;
        int _selectedIndex;
        List<string> _databaseNames;
        List<Item> _itemsList;
        int _selectedIndexItem;
        bool _textBoxCanBeEnabled;
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

        #region [PROPERTIES COMBOBOX PLAYLIST]

        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
                OnPropertyChanged(nameof(SelectedDatabase));
                LoadItemsDatabase(SelectedDatabase);
            }
        }

        public string SelectedDatabase => _selectedIndex > -1 && _selectedIndex < DatabaseNames.Count ? DatabaseNames[SelectedIndex] : "";

        public List<string> DatabaseNames
        {
            get
            {
                return _databaseNames;
            }
            set
            {
                _databaseNames = value;
                OnPropertyChanged(nameof(DatabaseNames));
            }
        }

        #endregion

        #region [PROPERTIES LIST ITEMS]

        public int SelectedIndexItem
        {
            get
            {
                return _selectedIndexItem;
            }
            set
            {
                _selectedIndexItem = value;
                Controller.CurrentItem = value != -1 ? SelectedItem.Clone() as Item : new Item();
                TextBoxCanBeEnabled = value != -1 ? true : false;
                OnPropertyChanged(nameof(SelectedIndexItem));
                OnPropertyChanged(nameof(SelectedItem));
                OnPropertyChanged(nameof(TempItem));
            }
        }

        public bool TextBoxCanBeEnabled
        {
            get
            {
                return _textBoxCanBeEnabled;
            }
            set
            {
                _textBoxCanBeEnabled = value;
                OnPropertyChanged(nameof(TextBoxCanBeEnabled));
            }
        }

        public bool EditCanBeEnable
        {
            get
            {
                return _selectedIndexItem != -1 ? !SelectedItem.Compare(Controller.CurrentItem) : false;
            }
        }

        public Item SelectedItem => SelectedIndexItem > -1 && SelectedIndexItem < ItemsList.Count ? ItemsList[SelectedIndex] : new Item();

        public List<Item> ItemsList
        {
            get
            {
                return _itemsList;
            }
            set
            {
                _itemsList = value;
                OnPropertyChanged(nameof(ItemsList));
            }
        }

        #endregion

        public Item TempItem
        {
            get
            {
                return Controller.CurrentItem;
            }
            set
            {
                Controller.CurrentItem = value;
                OnPropertyChanged(nameof(TempItem));
                OnPropertyChanged(nameof(EditCanBeEnable));
            }
        }


        public MainWindowViewModel()
        {
            _databaseNames = new List<string>();
            _itemsList = new List<Item>();
            Controller.LoadModel();
            Controller.Model.Playlists.Keys.ToList().ForEach(k => _databaseNames.Add(k));
            SelectedIndex = -1;
            _selectedIndexItem = -1;
            _textBoxCanBeEnabled = false;

        }

        public void LoadItemsDatabase(string selectedDatabase)
        {
            ItemsList = !string.IsNullOrEmpty(selectedDatabase) && Controller.Model.Playlists.Keys.Contains(selectedDatabase)
                        ? Controller.Model.Playlists[selectedDatabase].items
                        : new List<Item>();
                OnPropertyChanged(nameof(ItemsList));
        }

    }
}
