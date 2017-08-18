# RestClientExample

## 接口协议规范

### 调用格式
Rest/Post (业务数据以JSON格式传输)，目前选用的通信层用url格式即可，如：

```
http://url/?Cmd=接口名称&Data={"data":""}&Token=令牌
```

Token令牌第三方调用，无需另外做加密/生成处理，都为固定值，每个Token对应每个唯一商户。

### 返回值说明

```
public class Result
{
    //返回状态码，State > 0 表示接口调用正确，State = 0 时表示服务器发生未捕获的异常，State < 0 时表示服务器发生已知的错误，一般都为业务级别的错误
    [Newtonsoft.Json.JsonProperty("S")]
    public int State { get; set; }

    //错误信息，当State <= 0 时才会有值
    [Newtonsoft.Json.JsonProperty("M")]
    public string Message { get; set; }

    //返回的业务数据JSON
    [Newtonsoft.Json.JsonProperty("D")]
    public object Data { get; set; }

    //接口是否有返回业务数据Data，用于区分有/无返回值的接口信息
    [Newtonsoft.Json.JsonProperty("H")]
    public bool HasData { get; set; }
    
}
```

任何情况都应先判断 State ，如果 0 < State 表示正确，具体数字由业务确定，如果相反则表示错误，请做好相应错误处理 正确情况下可以判断 HasData 来确定是否有返回对象，请按需处理

