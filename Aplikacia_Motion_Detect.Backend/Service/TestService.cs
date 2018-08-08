using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplikacia_Motion_Detect.Interfaces.Interface;
namespace Aplikacia_Motion_Detect.Backend.Service
{
    public class TestService : ITestService
    {
        private string _test = "sdaad";
        public string Test { get => _test; set => _test = value; }


        public bool Tesat()
        {
            throw new NotImplementedException();
        }
    }
}
