using LPLManager.Model;
using LPLManager.Object;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public string customPath { get; set; }

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
            customPath = path;
        }


        public bool CompareCurrententItem(Item modified)
        {
            bool equal = false;
            //if (modified != null)
            //    equal = modified.core_name == CurrentItem.core_name &
            //            modified.core_path == CurrentItem.core_path &
            //            modified.crc32 == CurrentItem.crc32 &
            //            modified.db_name == CurrentItem.db_name &
            //            modified.label == CurrentItem.label &
            //            modified.path == CurrentItem.path;

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

        public void AddItem(Item itemToAdd, string database, bool isCustom = false)
        {
            _model.Playlists[database].items = _model.Playlists[database].items.Append(itemToAdd).OrderBy(i => i.label).ToList();
            string pathItem = isCustom ? customPath : "playlists\\" + database + ".lpl";
            FileJson<Root>.Write(pathItem, _model.Playlists[database]);
            isAdded = true;
        }

        public bool AddNewPlaylist(string namePlaylist)
        {
            if (File.Exists("playlists\\" + namePlaylist + ".lpl"))
                return false;
            Root playlist = new Root()
            {
                version = "1.4",
                default_core_path = "",
                default_core_name = "",
                label_display_mode = 0,
                right_thumbnail_mode = 0,
                sort_mode = 0,
                items = new List<Item>()
            };
            FileJson<Root>.Write("playlists\\" + namePlaylist + ".lpl", playlist);
            isAdded = true;
            return true;
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

        public List<string> GetPlaylistNames()
        {
            return new List<string>()
            {
                "Atari - 2600",
                "Atari - 5200",
                "Atari - 7800",
                "Atari - Jaguar",
                "Atari - Lynx",
                "Atari - ST",
                "Bandai - WonderSwan Color",
                "Bandai - WonderSwan",
                "Bomberman Game Clone",
                "Casio - Loopy",
                "Casio - PV-1000",
                "Cave Story",
                "Coleco - ColecoVision",
                "Commodore - 64",
                "Commodore - Plus-4",
                "Commodore - VIC-20",
                "DOOM",
                "DOS",
                "Dinothawr",
                "Emerson - Arcadia 2001",
                "Entex - Adventure Vision",
                "Epoch - Super Cassette Vision",
                "FB Alpha - Arcade Games",
                "Fairchild - Channel F",
                "Funtech - Super Acan",
                "GCE - Vectrex",
                "GamePark - GP32",
                "Handheld Electronic Game",
                "Hartung - Game Master",
                "Lutro",
                "MAME",
                "Magnavox - Odyssey2",
                "Mattel - Intellivision",
                "Microsoft - MSX",
                "Microsoft - MSX",
                "Microsoft - Xbox 360",
                "Microsoft - Xbox",
                "NEC - PC Engine - TurboGrafx 16",
                "NEC - PC Engine CD - TurboGrafx-CD",
                "NEC - PC Engine SuperGrafx",
                "NEC - PC-FX",
                "Nintendo - Family Computer Disk System",
                "Nintendo - Game Boy Advance",
                "Nintendo - Game Boy Color",
                "Nintendo - Game Boy",
                "Nintendo - GameCube",
                "Nintendo - Nintendo 3DS",
                "Nintendo - Nintendo 64",
                "Nintendo - Nintendo 64DD",
                "Nintendo - Nintendo DS",
                "Nintendo - Nintendo DSi",
                "Nintendo - Nintendo Entertainment System",
                "Nintendo - Pokemon Mini",
                "Nintendo - Satellaview",
                "Nintendo - Sufami Turbo",
                "Nintendo - Super Nintendo Entertainment System",
                "Nintendo - Virtual Boy",
                "Nintendo - Wii U",
                "Nintendo - Wii",
                "Philips - Videopac+",
                "Quake1",
                "RCA - Studio II",
                "SNK - Neo Geo CD",
                "SNK - Neo Geo Pocket Color",
                "SNK - Neo Geo Pocket",
                "SNK - Neo Geo",
                "ScummVM",
                "Sega - 32X",
                "Sega - Dreamcast",
                "Sega - Game Gear",
                "Sega - Master System - Mark III",
                "Sega - Mega Drive - Genesis",
                "Sega - Mega-CD - Sega CD",
                "Sega - PICO",
                "Sega - SG-1000",
                "Sega - Saturn",
                "Sinclair - ZX 81",
                "Sinclair - ZX Spectrum",
                "Sony - PlayStation 2",
                "Sony - PlayStation 3",
                "Sony - PlayStation 4",
                "Sony - PlayStation Portable Vita",
                "Sony - PlayStation Portable",
                "Sony - PlayStation",
                "The 3DO Company - 3DO",
                "Tiger - Game.com",
                "Tomb Raider",
                "VTech - CreatiVision",
                "VTech - V.Smile",
                "Watara - Supervision"
            };
        }
    }
}
