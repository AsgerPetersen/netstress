using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetStress
{
    public class StressResult
    {
        public bool Success { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public long TimeTaken { get; set; }

        public long ContentLength { get; set; }

        public StressRequest Request { get; set; }

        public List<string> ResponseHeaders { get; set; } 

        public StressResult(StressRequest request)
        {
            this.Request = request;
        }
    }
}
