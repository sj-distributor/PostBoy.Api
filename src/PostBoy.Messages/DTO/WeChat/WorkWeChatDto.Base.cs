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