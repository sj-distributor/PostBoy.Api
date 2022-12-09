using Newtonsoft.Json;

namespace PostBoy.Messages.DTO.WeChat;

public class WorkWeChatSendImageMessageDto : WorkWeChatSendMessageBaseDto
{
    public WorkWeChatSendImageMessageDto()
    {
        MsgType = "image";
    }
    
    [JsonProperty("image")]
    public WorkWeChatSendImageMessageDetailDto Image { get; set; }
}

public class WorkWeChatSendImageMessageDetailDto
{
    [JsonProperty("media_id")]
    public string MediaId { get; set; }
}