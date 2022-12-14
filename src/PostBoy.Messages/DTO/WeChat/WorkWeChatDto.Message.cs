using Newtonsoft.Json;

namespace PostBoy.Messages.DTO.WeChat;

public class WorkWeChatSendMessageDto : INeedAccessToken
{
    [JsonIgnore]
    public string AccessToken { get; set; }
    
    [JsonProperty("chatid")]
    public string ChatId { get; set; }
    
    [JsonProperty("agentid")]
    public long AgentId { get; set; }
    
    [JsonProperty("touser")]
    public string ToUser { get; set; }
    
    [JsonProperty("toparty")]
    public string ToParty { get; set; }
    
    [JsonProperty("totag")]
    public string ToTag { get; set; }

    [JsonProperty("msgtype")]
    protected string MsgType
    {
        get
        {
            if (Text != null)
                return "text";
            if (Image != null)
                return "image";
            if (Voice != null)
                return "voice";
            if (Video != null)
                return "video";
            if (File != null)
                return "file";
            if (MpNews != null)
                return "mpnews";
            return "";
        }
    }
    
    [JsonProperty("safe")]
    public int Safe { get; set; }
    
    [JsonProperty("text")]
    public WorkWeChatSendTextMessageDto Text { get; set; }
    
    [JsonProperty("image")]
    public WorkWeChatSendFileMessageDto Image { get; set; }
    
    [JsonProperty("voice")]
    public WorkWeChatSendFileMessageDto Voice { get; set; }
    
    [JsonProperty("video")]
    public WorkWeChatSendFileMessageDto Video { get; set; }
    
    [JsonProperty("file")]
    public WorkWeChatSendFileMessageDto File { get; set; }
    
    [JsonProperty("mpnews")]
    public WorkWeChatSendMpNewsMessageDto MpNews { get; set; }
}