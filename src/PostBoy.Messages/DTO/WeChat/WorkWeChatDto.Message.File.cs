using Newtonsoft.Json;
using PostBoy.Messages.Enums.WeChat;

namespace PostBoy.Messages.DTO.WeChat;

public class WorkWeChatSendFileMessageDto
{
    [JsonProperty("media_id")]
    public string MediaId { get; set; }
}