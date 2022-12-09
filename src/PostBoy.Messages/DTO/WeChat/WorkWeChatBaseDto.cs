using Newtonsoft.Json;

namespace PostBoy.Messages.DTO.WeChat;

public interface INeedAccessToken
{
    public string AccessToken { get; set; }
}

public class WorkWeChatResponseBaseDto
{
    [JsonProperty("errcode")]
    public int ErrCode { get; set; }
    
    [JsonProperty("errmsg")]
    public string ErrorMsg { get; set; }
}

public class WorkWeChatSendMessageBaseDto : INeedAccessToken
{
    public string AccessToken { get; set; }
    
    [JsonProperty("agentid")]
    public long AgentId { get; set; }
    
    [JsonProperty("touser")]
    public string ToUser { get; set; }
    
    [JsonProperty("toparty")]
    public string ToParty { get; set; }
    
    [JsonProperty("totag")]
    public string ToTag { get; set; }

    [JsonProperty("msgtype")]
    protected string MsgType { get; set; }
    
    [JsonProperty("safe")]
    public int Safe { get; set; }
}

public class WorkWeChatSendMessageResponseDto : WorkWeChatResponseBaseDto
{
    [JsonProperty("invalidtag")]
    public string InvalidTag { get; set; }
    
    [JsonProperty("invaliduser")]
    public string InvalidUser { get; set; }
    
    [JsonProperty("invalidparty")]
    public string InvalidParty { get; set; }
    
    [JsonProperty("unlicenseduser")]
    public string UnlicensedUser { get; set; }
    
    [JsonProperty("msgid")]
    public string MessageId { get; set; }
    
    [JsonProperty("response_code")]
    public string ResponseCode { get; set; }
}