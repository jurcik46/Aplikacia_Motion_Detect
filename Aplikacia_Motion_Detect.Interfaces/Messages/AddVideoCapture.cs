using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTKVideoCapLib;

namespace Aplikacia_Motion_Detect.Interfaces.Messages
{
    public class AddVideoCapture
    {
        private bool _createNew;

        public bool CreateNew
        {
            get { return _createNew; }
            set { _createNew = value; }
        }
    }
}
