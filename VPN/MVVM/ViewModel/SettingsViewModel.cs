using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPN.MVVM.ViewModel
{
    internal class SettingsViewModel
    {
        public GlobalViewModel Global { get; } = GlobalViewModel.Instance;
        public SettingsViewModel()
        {

        }
    }
}
