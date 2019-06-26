using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENAREK.Helpers; 

namespace Tasks
{
    class TestRandomGenerator
    {
        public void run()
        {
            Console.WriteLine("Give AFM");
            string afm = Console.ReadLine();
            if (RandomGenerator.checkID(afm))
            {
                Console.WriteLine(afm + ": OK");
            }
            else {  Console.WriteLine(afm + ": NOT OK"); }

        }
    }
}
