using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPLManager.Object
{
    public class Item : ICloneable
    {
        public int id;
        public string path { get; set; }
        public string label { get; set; }
        public string core_path { get; set; }
        public string core_name { get; set; }
        public string crc32 { get; set; }
        public string db_name { get; set; }

        public object Clone()
        {
            return new Item()
            {
                id = id,
                path = path,
                label = label,
                core_path = core_path,
                core_name = core_name,
                crc32 = crc32,
                db_name = db_name
            };
        }

        public bool Compare(Item itemToCompare)
        {
            return  id == itemToCompare.id &&
                    path == itemToCompare.path &&
                    label == itemToCompare.label &&
                    core_path == itemToCompare.core_path &&
                    core_name == itemToCompare.core_name &&
                    crc32 == itemToCompare.crc32 &&
                    db_name == itemToCompare.db_name;
        }
    }

    public class Root
    {
        public string version { get; set; }
        public string default_core_path { get; set; }
        public string default_core_name { get; set; }
        public int label_display_mode { get; set; }
        public int right_thumbnail_mode { get; set; }
        public int left_thumbnail_mode { get; set; }
        public int sort_mode { get; set; }
        public List<Item> items { get; set; }
    }

}
