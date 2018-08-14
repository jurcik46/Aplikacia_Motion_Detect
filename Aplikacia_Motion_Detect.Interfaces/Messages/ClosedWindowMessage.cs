using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacia_Motion_Detect.Interfaces.Messages
{
    class ClosedWindowMessage
    {
        private bool _closed;
        public bool Closed { get => _closed; set => _closed = value; }
    }
}
