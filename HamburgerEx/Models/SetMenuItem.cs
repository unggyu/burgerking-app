using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamburgerEx
{
    class SetMenuItem : MenuItem
    {
        public Burger Burger { get; set; }
        public Side Side { get; set; }
        public Drink Drink { get; set; }
    }
}
