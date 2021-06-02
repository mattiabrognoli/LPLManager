using LPLManager.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPLManager.Uility
{
    public static class Check
    {
        public static bool CurrentWithTemp(Item current, Item temp)
        {
            bool equal = false;
                equal = current.core_name == temp.core_name &
                        current.core_path == temp.core_path &
                        current.crc32 == temp.crc32 &
                        current.db_name == temp.db_name &
                        current.label == temp.label &
                        current.path == temp.path;

            return equal;
        }
    }
}
