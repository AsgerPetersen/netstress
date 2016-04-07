using System;
using System.Collections.Generic;
using System.Globalization;
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
                Console.WriteLine("     StressTest logfile host timefactor [headers]");
                Console.WriteLine("Logfile must be ;-seperated");
                Console.WriteLine("     First columns is request id");
                Console.WriteLine("     Second columns is timestamp. Format yyyy-MM-dd HH:mm:ss.FFF");
                Console.WriteLine("     Third column is appended to the 'host' to form the request url");
                Console.WriteLine("headers is of the form {header1,header2,...,headern}. For each request");
                Console.WriteLine("     these response headers are copied to the output");
                return;
            }


            string logfile = args[0];
            string host = args[1].Trim("\"".ToCharArray());
            double timefactor = double.Parse(args[2], CultureInfo.InvariantCulture);
            List<string> headers = new List<string>();
            if (args.Length > 3)
            {
                headers = args[3].TrimStart("{".ToCharArray()).TrimEnd("}".ToCharArray()).Split(',').ToList();
            }

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
            StressTester tester = new StressTester();
            tester.KeepHeaders = headers;
            tester.RunTest(reqs, timefactor, ordered: true);
        }
    }
}
