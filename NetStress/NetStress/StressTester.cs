using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetStress
{
    public class StressTester
    {
        public static IEnumerable<StressResult> RunTest(IEnumerable<StressRequest> requests, double timeFactor, bool ordered = true)
        {
            // Inspired by http://stackoverflow.com/questions/16194054/is-async-httpclient-from-net-4-5-a-bad-choice-for-intensive-load-applications
            ServicePointManager.DefaultConnectionLimit = 100;
            var urls = requests;
            if (!ordered)
            {
                urls = urls.ToList().OrderBy(r => r.RelativeTime);
            }

            DateTime start = DateTime.Now;
            List<Task<StressResult>> tasks = new List<Task<StressResult>>();
            foreach (StressRequest r in urls)
            {
                // Multiply time factor
                TimeSpan reqTime = r.RelativeTime.Multiply(timeFactor);
                Console.WriteLine("Next req at: " + reqTime);
                while (DateTime.Now - start < reqTime)
                {
                    System.Threading.Thread.Sleep(1);
                }
                Task<StressResult> t = Task.Factory.StartNew(() => PerformRequest(r));
                tasks.Add(t);
            }
            Task.WaitAll(tasks.ToArray());
            return tasks.Select(t => t.Result);
        }

        private static StressResult PerformRequest(StressRequest req)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            Stopwatch watch = new Stopwatch();
            StressResult result = new StressResult(req);
            try
            {
                watch.Start();
                request = (HttpWebRequest)WebRequest.Create(req.Url);
                request.Method = "GET";
                request.KeepAlive = true;
                response = (HttpWebResponse)request.GetResponse();
                watch.Stop();
                
                // DEBUG
                Console.WriteLine(watch.ElapsedMilliseconds);
                
                result.TimeTaken = watch.ElapsedMilliseconds;
                result.HttpStatusCode = response.StatusCode;
                result.ContentLength = response.ContentLength;
                result.Success = true;
                return result;
            }
            catch (Exception e)
            {
                // TODO: Consider including exception in result
                result.Success = false;
                return result; ;
            }
            finally
            {
                if (response != null) response.Close();
            }
        }
    }
}
