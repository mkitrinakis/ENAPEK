using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENAPEK.Helpers;


using Newtonsoft.Json; 

namespace Tasks
{
    class TESTHDIKACalls
    {
        public void run()
        {
            Log.write("TESTHDIKACalls - starts");
            List<int> loops = new List<int>();
            for (int i = 0; i <= 0; i++) loops.Add(i);
            
            Parallel.ForEach(loops, (loop) =>
            {
                Log.write(loop.ToString());
                HDIKACalls.StructAMKADetailsResponse rs = HDIKACalls.getAMKADetails(true, "14087200714", "Kitrinakis"); 
                Log.write(loop.ToString() + "--->" + JsonConvert.SerializeObject( rs));
            });
                

            }
        }
    }

