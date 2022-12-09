using Newtonsoft.Json;

namespace PostBoy.Messages.DTO.WeChat;

public class CreateWorkWeChatGroupDto : INeedAccessToken
{
    public string AccessToken { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("owner")]
    public string Owner { get; set; }
    
    [JsonProperty("userlist")]
    public List<string> UserList { get; set; }
    
    [JsonProperty("chatid")]
    public string ChatId { get; set; }
}

public class CreateWorkWeChatGroupResponseDto : WorkWeChatResponseBaseDto
{
    [JsonProperty("chatid")]
    public string ChatId { get; set; }
}