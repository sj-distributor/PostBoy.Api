using Newtonsoft.Json;

namespace PostBoy.Messages.DTO.WeChat;

public class WorkWeChatAccessTokenDto : WorkWeChatResponseBaseDto
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
    
    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
}