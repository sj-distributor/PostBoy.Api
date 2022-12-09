using Newtonsoft.Json;

namespace PostBoy.Messages.DTO.WeChat;

public class WorkWeChatSendTextMessageDto : WorkWeChatSendMessageBaseDto
{
    public WorkWeChatSendTextMessageDto()
    {
        MsgType = "text";
    }
    
    [JsonProperty("text")]
    public WorkWeChatSendTextMessageDetailDto Text { get; set; }
}

public class WorkWeChatSendTextMessageDetailDto
{
    [JsonProperty("content")]
    public string Content { get; set; }
}