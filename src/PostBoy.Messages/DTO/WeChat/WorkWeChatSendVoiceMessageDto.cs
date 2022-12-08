using Newtonsoft.Json;

namespace PostBoy.Messages.DTO.WeChat;

public class WorkWeChatSendVoiceMessageDto : WorkWeChatSendMessageBaseDto
{
    public WorkWeChatSendVoiceMessageDto()
    {
        MsgType = "voice";
    }
    
    [JsonProperty("voice")]
    public WorkWeChatSendVoiceMessageDetailDto Voice { get; set; }
}

public class WorkWeChatSendVoiceMessageDetailDto
{
    [JsonProperty("media_id")]
    public string MediaId { get; set; }
}