using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetStress
{
    class Program
    {
        static void Main(string[] args)
        {
            string logfile = args[0];
            string host = args[1];
            double timefactor = double.Parse(args[2]);

            // Set up logparser using all defaults
            LogParser log = new LogParser();
            log.Host = host;
            IEnumerable<StressRequest> reqs = log.Parse(logfile);

            // Test
            StressTester.RunTest(reqs, timefactor, true);


        }
    }
}
