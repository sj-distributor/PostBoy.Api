using Newtonsoft.Json;

namespace PostBoy.Messages.DTO.WeChat;

public class WorkWeChatSendTextMessageDto
{
    [JsonProperty("content")]
    public string Content { get; set; }
}