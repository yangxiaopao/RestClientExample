using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class HttpRequestResult
{
    public string Html;
    public string RequestUri;
    public int contentLength;
}
public class Http
{
    public string userAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/28.0.1500.72 Safari/537.36";
    public Http()
    {

    }
    public enum HttpWebRequestMethod
    {
        POST,
        GET
    }

    public HttpRequestResult Get(string url, string referer)
    {
        return HttpOperating(url, HttpWebRequestMethod.GET, referer, "").Result;
    }

    public HttpRequestResult Post(string url, string referer, string postData)
    {
        return HttpOperating(url, HttpWebRequestMethod.POST, referer, postData).Result;
    }


    private async Task<HttpRequestResult> HttpOperating(string url, HttpWebRequestMethod method, string referer, string postData)
    {
        int retry = 2;
        do
        {
            using (var data = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded"))
            {
                using (var handler = new HttpClientHandler())
                {
                    try
                    {
                        handler.AllowAutoRedirect = true;
                        handler.MaxAutomaticRedirections = 20;
                        handler.AutomaticDecompression = DecompressionMethods.GZip;
                        using (var httpClient = new HttpClient(handler))
                        {
                            httpClient.DefaultRequestHeaders.Add("User-Agent", this.userAgent);
                            httpClient.Timeout = TimeSpan.FromSeconds(10);
                            HttpResponseMessage message = null;
                            switch (method)
                            {
                                case HttpWebRequestMethod.POST:
                                    {
                                        if (null != postData)
                                        {
                                            message = await httpClient.PostAsync(url, data).ConfigureAwait(false);
                                        }
                                    }
                                    break;
                                case HttpWebRequestMethod.GET:
                                    {
                                        message = await httpClient.GetAsync(url).ConfigureAwait(false);
                                    }
                                    break;
                            }
                            if (message.StatusCode.Equals(HttpStatusCode.OK))
                            {
                                var html = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
                                return new HttpRequestResult
                                {
                                    Html = html,
                                    RequestUri = message.RequestMessage.RequestUri.AbsoluteUri,
                                    contentLength = Convert.ToInt32(message.Content.Headers.ContentLength)
                                };
                            }
                            else
                            {
                                return null;
                            }
                        }


                    }
                    catch (Exception e)
                    {

                    }
                }

            }

        } while (retry-- > 0);

        return null;
    }
    /// <summary>
    /// 将list转成查询字符串
    /// </summary>
    /// <param name="postData"></param>
    /// <returns></returns>
    public static string GetPostData(List<KeyValuePair<string, string>> postData)
    {
        return GetData(postData, true).ToString();
    }


    public static string GetData(List<KeyValuePair<string, string>> postData, bool UrlEncode = false)
    {
        var data = new StringBuilder(postData.Count);
        foreach (var item in postData)
        {
            var value = item.Value;
            if (UrlEncode && null != item.Value)
            {
                value = System.Net.WebUtility.UrlEncode(value);
            }
            data.AppendFormat("{0}={1}&", item.Key, value);
        }
        return data.ToString().TrimEnd('&');
    }


}