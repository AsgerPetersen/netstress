using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetStress
{
    public class CsvLogParser
    {
        private int idIndex = 0;
        private int urlIndex = 2;
        private int timeIndex = 1;
        private string host = string.Empty;
        // 07/Sep/2014:07:47:00 +0000
        private string timeFormat = "yyyy-MM-dd HH:mm:ss.FFF";

        private CultureInfo cultureInfo = new CultureInfo("en-US");

        /// <summary>
        /// Gets or sets the host which will be prepended to the url parsed from the logfile.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        public string Host
        {
            get { return host; }
            set { host = value; }
        }

        public CsvLogParser()
        {
        }

        public IEnumerable<StressRequest> Parse(string file)
        {
            DateTime startTime = DateTime.MinValue;
            foreach (string line in System.IO.File.ReadLines(file))
            {
                string[] columns = line.Split(";".ToCharArray());
                DateTime requestTime = ParseDateTime(columns[timeIndex]);
                if (startTime == DateTime.MinValue)
                    startTime = requestTime;
                StressRequest r = new StressRequest();
                r.RelativeTime = requestTime - startTime;
                r.Url = this.Host + columns[urlIndex];
                r.Id = columns[this.idIndex];
                yield return r;
                }
            }

        private DateTime ParseDateTime(string s)
        {
            return DateTime.ParseExact(s, timeFormat, cultureInfo);
        }
    }
}
