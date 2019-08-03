using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartRockets
{
    class Function
    {
        public Action<MouseEventArgs> action { get; set; }
        public Action<Graphics> render { get; set; }
    }
}
