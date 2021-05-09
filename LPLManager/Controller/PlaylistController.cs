using LPLManager.FileManagers;
using LPLManager.Model;
using LPLManager.Object;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LPLManager.FileManager;
using System.Windows.Media.Imaging;

namespace LPLManager.Controller
{
    public class PlaylistController
    {
        private PlaylistCollection _model;

        public PlaylistCollection Model { get => _model; }

        public string CurrentDatabase { get; set; }

        public Item CurrentItem { get; set; }

        public bool isCustom { get; set; }

        public bool isAdded { get; set; }

        public bool isImageEdit { get; set; }

        public int CurrentIndex { get; set; }

        public void LoadModel()
        {
            _model = new PlaylistCollection();
            _model.Playlists = new Dictionary<string, Root>();
            List<string> pathFiles = FileManagers.FileManager.GetPathWithParticularExtension("lpl");
            pathFiles.ForEach(f =>
            {
                int idTemp = 0;
                string plName = Path.GetFileNameWithoutExtension(f);
                _model.Playlists.Add(plName, FileJson<Root>.Read(f));
                _model.Playlists[plName].items = _model.Playlists[plName].items.OrderBy(i => i.label).ToList();
                _model.Playlists[plName].items.ForEach(i => i.id = idTemp++);
            });
        }

        public void ReloadDatabase(string dbName)
        {
            int idTemp = 0;
            _model.Playlists[dbName] = FileJson<Root>.Read(@"playlists\" + dbName + ".lpl");
            _model.Playlists[dbName].items.ForEach(i => i.id = idTemp++);
        }

        public void LoadCustomDatabase(string path)
        {
            _model.Playlists = new Dictionary<string, Root>();
            int idTemp = 0;
            string plName = Path.GetFileNameWithoutExtension(path);
            _model.Playlists.Add(plName, FileJson<Root>.Read(path));
            _model.Playlists[plName].items.ForEach(i => i.id = idTemp++);
            CurrentDatabase = plName;
            isCustom = true;
        }

        public bool CompareCurrententItem(Item modified)
        {
            bool equal = false;
            if (modified != null)
                equal = modified.core_name == CurrentItem.core_name &
                        modified.core_path == CurrentItem.core_path &
                        modified.crc32 == CurrentItem.crc32 &
                        modified.db_name == CurrentItem.db_name &
                        modified.label == CurrentItem.label &
                        modified.path == CurrentItem.path;

            return equal;
        }

        public bool Compare(Item item1, Item item2)
        {
            return item1.core_name == item2.core_name &
                   item1.core_path == item2.core_path &
                   item1.crc32 == item2.crc32 &
                   item1.db_name == item2.db_name &
                   item1.label == item2.label &
                   item1.path == item2.path;
        }

        public void RemoveItem(Item itemToRemove, string database, bool removeFile = false)
        {
            if (removeFile)
                FileManagers.FileManager.DeleteFile(itemToRemove.path);
            _model.Playlists[database].items = _model.Playlists[database].items.Where(c => c != itemToRemove).ToList();
        }

        public void AddItem(Item itemToAdd, string database)
        {
            _model.Playlists[database].items = _model.Playlists[database].items.Append(itemToAdd).OrderBy(i => i.label).ToList();
            FileJson<Root>.Write(@"playlists\" + database + ".lpl", _model.Playlists[database]);
            isAdded = true;
        }

        public void SaveImage(BitmapImage pic, string oldName, string newName)
        {
            FileManagers.FileManager.DeleteFile(@"thumbnails\" + CurrentDatabase + @"\Named_Boxarts\" + oldName + ".png");
            FileManagers.FileManager.DeleteFile(@"thumbnails\" + CurrentDatabase + @"\Named_Boxarts\" + newName + ".png");
            FileManagers.FileManager.CreateFolderIfNotExist("thumbnails");
            FileManagers.FileManager.CreateFolderIfNotExist(@"thumbnails\" + CurrentDatabase + @"\Named_Boxarts");
            FileManagers.FileManager.SaveImage(pic, @"thumbnails\" + CurrentDatabase + @"\Named_Boxarts\" + newName + ".png");
            isImageEdit = false;
        }
    }
}
