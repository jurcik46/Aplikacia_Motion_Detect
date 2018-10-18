using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacia_Motion_Detect.Interfaces.Messages
{
    public class NotifiMessage
    {
        private string _msg;

        public string Msg
        {
            get { return _msg; }
            set { _msg = value; }
        }
    }
}
