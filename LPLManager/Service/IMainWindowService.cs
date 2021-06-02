using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPLManager.Service
{
    interface IMainWindowService <T>
    {
        Task<bool> CheckCurrentWithTemp(T current, T temp);
    }
}
