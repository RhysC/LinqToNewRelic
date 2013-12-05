using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace LinqToNewRelic.Implementations
{
    abstract class ApiBase
    {
        private readonly string _apiKey;

        protected ApiBase(string apiKey)
        {
            _apiKey = apiKey;
        }

        protected static string GetNewRelicUrl(string accountId, string extendedPath)
        {
            return string.Format("https://api.newrelic.com/api/v1/accounts/{0}/{1}", accountId, extendedPath);
        }
        protected static string GetUrl(string baseUrl, NameValueCollection parameters)
        {
            var urlbuilder = new UriBuilder(baseUrl) { Port = -1, Query = parameters.ToQueryString(true) };
            return urlbuilder.ToString();
        }

        protected static NameValueCollection ToNamedValueCollection(MetricDataSearchParams searchParams)
        {
            //http://newrelic.github.io/newrelic_api/README_rdoc.html#label-Metric+Data
            var parameters = new NameValueCollection();

            parameters["begin"] = searchParams.StartDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            // Begin time, in XML UTC format For example: 2011-04-20T15:47:00Z
            parameters["end"] = searchParams.EndDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            //End time, in XML UTC format. For example: 2011-04-20T15:52:00Z
            foreach (var metric in searchParams.Metrics)
            {
                //Included one or many times, this lists the metrics you are interested in. You can only specify a specific metric once. If you specify multiple metrics, the request parameter should look like metrics[]=foo&metrics[]=bar (append '[]' to the end of the name of the parameter). You can specify metrics[] even if there is a single metric.
                parameters["metrics[]"] = metric;
            }
            parameters["field"] = searchParams.MetricField;
            //Each metric supports different fields, such as 'average_response_time'. If the field is not valid for a given metric, you will get a blank value, and if there are no valid fields for the metrics you request, you will get an error.
            parameters["summary"] = "0";
            //1 or 0, defaults to 0. This determines whether you get back a single value aggregated over the entire time period, or a time series. Summary results do not include the begin and end times in the result. Time s
            return parameters;
        }
        protected T GetDeserializedResult<T>(string url)
        {
            var webClient = new WebClient();
            webClient.Headers.Add("x-api-key", _apiKey);
            var json = webClient.DownloadString(url);
            return JsonConvert.DeserializeObject<T>(json);
        }

        protected T GetDeserializedXmlResult<T>(string url)
        {
            var webClient = new WebClient();
            webClient.Headers.Add("x-api-key", _apiKey);
            var xml = webClient.DownloadString(url);

            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(reader))
            {
                    return (T)serializer.Deserialize(xmlReader);
            }	
        }
    }
}