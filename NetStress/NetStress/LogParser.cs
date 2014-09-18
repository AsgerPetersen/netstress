using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetStress
{
    public class LogParser
    {
        // Apache2 std log format
        // 90.115.87.110 - - [07/Sep/2014:08:33:08 +0000] "GET /mapproxy/wmts/DSM/xxx/10/1251/797.jpeg HTTP/1.1" 200 31855 "http://yyy.dk/dhm14/map.html" "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2062.103 Safari/537.36"
        private Regex regex = new Regex(@"(\d+\.\d+\.\d+\.\d+) (.*) (.*) \[(.*)\] ""(.*) (.*) (.*)"" (\d+) (.*) ""(.*)"" ""(.*)""");
        private int urlIndex = 6;
        private int timeIndex = 4;
        private string host = string.Empty;
        // 07/Sep/2014:07:47:00 +0000
        private string timeFormat = "dd/MMM/yyyy:HH:mm:ss zzz";

        private CultureInfo cultureInfo = new CultureInfo("en-US");

        public CultureInfo CultureInfo
        {
            get { return cultureInfo; }
            set { cultureInfo = value; }
        }


        public string TimeFormat
        {
            get { return timeFormat; }
            set { timeFormat = value; }
        }
        

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

        /// <summary>
        /// Gets or sets the index of the URL group in the provided LogRegex.
        /// </summary>
        /// <value>
        /// The index of the URL.
        /// </value>
        public int UrlGroupIndex
        {
            get { return urlIndex; }
            set { urlIndex = value; }
        }

        /// <summary>
        /// Gets or sets the index of the time group in the provided LogRegex.
        /// </summary>
        /// <value>
        /// The index of the time group.
        /// </value>
        public int TimeGroupIndex
        {
            get { return timeIndex; }
            set { timeIndex = value; }
        }

        /// <summary>
        /// Gets or sets the regex used for log parsing.
        /// </summary>
        /// <value>
        /// The log regex.
        /// </value>
        public Regex LogRegex
        {
            get { return regex; }
            set { regex = value; }
        }

        public LogParser()
        {
        }

        public IEnumerable<StressRequest> Parse(string file)
        {
            DateTime startTime = DateTime.MinValue;
            foreach (string line in System.IO.File.ReadLines(file))
            {
                Match m = regex.Match(line);
                if (m.Success)
                {
                    DateTime requestTime = ParseDateTime(m.Groups[timeIndex].Value);
                    if(startTime == DateTime.MinValue)
                        startTime = requestTime;
                    StressRequest r = new StressRequest();
                    r.RelativeTime = requestTime - startTime;
                    r.Url = this.Host + m.Groups[urlIndex].Value;
                    yield return r;
                }
            }
        }

        private DateTime ParseDateTime(string s)
        {
            
            return DateTime.ParseExact(s, timeFormat, cultureInfo);
        }
    }
}
