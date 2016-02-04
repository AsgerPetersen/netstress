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
            if (args.Length < 3)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("     StressTest logfile host timefactor");
                Console.WriteLine("Logfile must be ;-seperated");
                Console.WriteLine("First columns is request id");
                Console.WriteLine("Second columns is timestamp. Format yyyy-MM-dd HH:mm:ss.FFF");
                Console.WriteLine("Third column is appended to the 'host' to form the request url");
                return;
            }


            string logfile = args[0];
            string host = args[1];
            double timefactor = double.Parse(args[2]);

            //host = "http://137.117.169.140/mapcgi?map=/data/ortofoto/ortofoto/orto_foraar/2013/linuxtest2.map&";
            //logfile = @"Z:\Projekter\datafordeler\data\outfile_workhours.csv";
            //timefactor = 0.5;

            // Set up logparser using all defaults
            //LogParser log = new LogParser();
            //log.Host = host;
            //IEnumerable<StressRequest> reqs = log.Parse(logfile);

            CsvLogParser log = new CsvLogParser();
            log.Host = host;
            IEnumerable<StressRequest> reqs = log.Parse(logfile);

            // Test
            StressTester.RunTest(reqs, timefactor, true);


        }
    }
}
