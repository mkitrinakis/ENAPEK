using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; 

namespace Tasks
{
    class Program
    {
        static void Main(string[] args)
        { // 1st char is latin, second char is greek 
            TESTHDIKACalls testHDIKA = new TESTHDIKACalls();
            testHDIKA.run();
            return; 

            NormalizeInput ni = new NormalizeInput();
            ni.run();
        }
    }
}
