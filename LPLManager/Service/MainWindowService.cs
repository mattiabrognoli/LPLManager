using LPLManager.Object;
using LPLManager.Uility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPLManager.Service
{
    public class MainWindowService : IMainWindowService<Item>
    {
        public Task<bool> CheckCurrentWithTemp(Item current, Item temp)
        {
            return Task.Run(() => Check.CurrentWithTemp(current, temp));
        }
    }
}
