using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HRNX.Connector.DayForce.Utils
{
   public class ServiceUtils
    {
        public HttpWebResponse HttpRequest(IDictionary<string, string> connection, string requestUrl, string methodName, string requestContent)
        {
            HttpWebResponse response = null;
            try
            {
                //create http request Url
                HttpWebRequest httprequest = WebRequest.Create(requestUrl) as HttpWebRequest;
                String str = connection[ConstantUtils.Username];
                String str1 = connection[ConstantUtils.Password];
                str = String.Concat(str, str1);
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);

                //  string data = System.Convert.ToBase64String(bytes);
                string data = "REZXU1Rlc3Q6REZXU1Rlc3Q=";

                if (!string.IsNullOrEmpty(str))
                {
                    httprequest.Headers.Add("Authorization", string.Format("Basic {0}", data));
                }
                httprequest.Method = methodName;
                httprequest.ContentType = "application/json";
                if (requestUrl == "https://usconfigr56.dayforcehcm.com/Api/ddn/V1/Employees")
                {
                    httprequest.Headers.Add("isValidateOnly", "true");
                }
                //  httprequest.Headers.Add("Connection", "keep-alive");
                //  httprequest.Headers.Add("accept-encoding", "gzip, deflate");
                //  httprequest.Headers.Add("Host", "usconfigr56.dayforcehcm.com");
                //  httprequest.Headers.Add("Postman-Token", "a682b497-c7e2-4460-8e22-f39f9222e915");
                //  httprequest.Headers.Add("Cache-Control", "no-cache");
                //  httprequest.Headers.Add("Accept", "*/*");
                //  httprequest.Headers.Add("User-Agent", "PostmanRuntime/7.11.0");
                if (!string.IsNullOrEmpty(requestContent))
                {
                    Byte[] bt = System.Text.Encoding.UTF8.GetBytes(requestContent);
                    Stream st = httprequest.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                }
                response = httprequest.GetResponse() as HttpWebResponse;
                return response;
            }
            catch (WebException e)
            {
                using (WebResponse resp = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)resp;
                    using (Stream data = resp.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        if ((httpResponse.StatusCode == HttpStatusCode.NotFound) && (methodName == HttpMethods.GET.ToString()))
                        {
                            Stream receiveStream = httpResponse.GetResponseStream();
                            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

                            var newData = (JObject)JsonConvert.DeserializeObject(text);
                            string textMessage = newData["message"].Value<string>();
                            if (textMessage == "Resource not found")
                            {
                                return null;
                            }
                        }
                        if ((httpResponse.StatusCode == HttpStatusCode.BadRequest) && (methodName == HttpMethods.POST.ToString()))
                        {
                            throw new WebException("Request is malformed. Correct and resubmit.");
                        }
                        if ((httpResponse.StatusCode == HttpStatusCode.BadRequest) && (methodName == HttpMethods.GET.ToString()))
                        {
                            throw new WebException("Request is malformed. Correct and resubmit.");
                        }
                        if ((httpResponse.StatusCode == HttpStatusCode.InternalServerError) && (methodName == HttpMethods.POST.ToString()))
                        {
                            throw new WebException("An Unexpected Server Error Occured");
                        }
                        if ((httpResponse.StatusCode == HttpStatusCode.InternalServerError) && (methodName == HttpMethods.GET.ToString()))
                        {
                            throw new WebException("An Unexpected Server Error Occured");
                        }

                        throw new WebException("Unable to post/get the request to DayForce Api:" + text, e.InnerException);
                    }
                }
            }
            catch (Exception e)
            {
                throw new WebException("Unable to post/get the request to DayForce api :" + e.Message, e.InnerException);
            }
        }
    }
}
