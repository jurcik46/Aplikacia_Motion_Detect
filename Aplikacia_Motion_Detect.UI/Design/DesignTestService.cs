using Aplikacia_Motion_Detect.Interfaces.Interface;


namespace Aplikacia_Motion_Detect.UI.Design
{
    public class DesignTestService : ITestService

    {
        private string _test = "sdaad desing";

        public string Test { get => _test; set => _test = value; }


        public bool Tesat()
        {
            return true;
        }
    }
}
