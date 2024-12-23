using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPN.MVVM.ViewModel
{
    internal class GlobalViewModel
    {
        public static GlobalViewModel Instance { get; } = new GlobalViewModel();

        public bool _isAwesome;

        public bool IsAwesome
        {
            get { return _isAwesome; }
            set { _isAwesome = value; }
        }
    }
}
