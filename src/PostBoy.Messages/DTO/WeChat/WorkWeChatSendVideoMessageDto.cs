using Newtonsoft.Json;

namespace PostBoy.Messages.DTO.WeChat;

public class WorkWeChatSendVideoMessageDto : WorkWeChatSendMessageBaseDto
{
    public WorkWeChatSendVideoMessageDto()
    {
        MsgType = "video";
    }
    
    [JsonProperty("voice")]
    public WorkWeChatSendVideoMessageDetailDto Video { get; set; }
}

public class WorkWeChatSendVideoMessageDetailDto
{
    [JsonProperty("media_id")]
    public string MediaId { get; set; }
    
    [JsonProperty("title")]
    public string Title { get; set; }
    
    [JsonProperty("description")]
    public string Description { get; set; }
}