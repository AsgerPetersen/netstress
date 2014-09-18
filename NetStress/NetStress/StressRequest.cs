using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetStress
{
    public class StressRequest
    {
        /// <summary>
        /// Gets or sets the relative time.
        /// </summary>
        /// <value>
        /// The relative time.
        /// </value>
        public TimeSpan RelativeTime { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }
    }
}
