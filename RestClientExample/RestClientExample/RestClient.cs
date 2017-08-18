using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Http;

public class Result
{
    [Newtonsoft.Json.JsonProperty("S")]
    public int State { get; set; }

    [Newtonsoft.Json.JsonProperty("M")]
    public string Message { get; set; }

    [Newtonsoft.Json.JsonProperty("D")]
    public object Data { get; set; }

    [Newtonsoft.Json.JsonProperty("H")]
    public bool HasData { get; set; }

    [Newtonsoft.Json.JsonProperty("R")]
    public string ResultCode { get; set; }

    public string ToJson()
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(this);
    }
}

public class RestClient
{
    public static string MEMBER_URL = "http://api.data.51xiaojinku.com/Member";

    private string Token { get; set; }

    private Http HttpClient { get; set; }

    public RestClient(string token)
    {
        this.Token = token;
        this.HttpClient = new Http();
    }

    private List<KeyValuePair<string, string>> List()
    {
        return new List<KeyValuePair<string, string>>();
    }

    public Result RunCMD(string serviceUrl, string cmd, string data, HttpWebRequestMethod method = HttpWebRequestMethod.POST)
    {
        try
        {
            var list = List();
            list.Add(new KeyValuePair<string, string>("cmd", cmd));
            list.Add(new KeyValuePair<string, string>("token", this.Token));
            list.Add(new KeyValuePair<string, string>("data", data));

            HttpRequestResult requestResult = null;
            switch (method)
            {
                case HttpWebRequestMethod.GET:
                    serviceUrl = serviceUrl + "?" + Http.GetData(list);
                    requestResult = HttpClient.Get(serviceUrl, null);
                    break;
                case HttpWebRequestMethod.POST:
                    requestResult = HttpClient.Post(serviceUrl, null, Http.GetPostData(list));
                    break;
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Result>(requestResult.Html);
        }
        catch (Exception)
        {
            return new Result { State = -500, Message = "服务器发生错误" };
        }

    }

    public T RunCMD<T>(string serviceUrl, string cmd, string data, HttpWebRequestMethod method = HttpWebRequestMethod.POST)
    {
        try
        {
            var list = List();
            list.Add(new KeyValuePair<string, string>("cmd", cmd));
            list.Add(new KeyValuePair<string, string>("token", this.Token));
            list.Add(new KeyValuePair<string, string>("data", data));

            HttpRequestResult requestResult = null;
            switch (method)
            {
                case HttpWebRequestMethod.GET:
                    serviceUrl = serviceUrl + "?" + Http.GetData(list);
                    requestResult = HttpClient.Get(serviceUrl, null);
                    break;
                case HttpWebRequestMethod.POST:
                    requestResult = HttpClient.Post(serviceUrl, null, Http.GetPostData(list));
                    break;
            }
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Result>(requestResult.Html);
            if (result.State > 0 && result.HasData)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result.Data.ToString());
            }
            else
            {
                return default(T);
            }
        }
        catch (Exception ex)
        {
            return default(T);
        }


    }

}
