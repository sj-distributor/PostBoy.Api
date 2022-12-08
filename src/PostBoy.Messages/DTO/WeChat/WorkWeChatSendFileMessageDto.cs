using Newtonsoft.Json;

namespace PostBoy.Messages.DTO.WeChat;

public class WorkWeChatSendFileMessageDto : WorkWeChatSendMessageBaseDto
{
    public WorkWeChatSendFileMessageDto()
    {
        MsgType = "file";
    }
    
    [JsonProperty("file")]
    public WorkWeChatSendFileMessageDetailDto File { get; set; }
}

public class WorkWeChatSendFileMessageDetailDto
{
    [JsonProperty("media_id")]
    public string MediaId { get; set; }
}