using Newtonsoft.Json;

namespace PostBoy.Messages.DTO.WeChat;

public class WorkWeChatSendMpNewsMessageDto : WorkWeChatSendMessageBaseDto
{
    public WorkWeChatSendMpNewsMessageDto()
    {
        MsgType = "mpnews";
    }
    
    [JsonProperty("mpnews")]
    public WorkWeChatSendMpNewsMessageDetailDto MpNews { get; set; }
}

public class WorkWeChatSendMpNewsMessageDetailDto
{
    [JsonProperty("articles")]
    public List<WorkWeChatSendMpNewsMessageArticleDetailDto> Articles { get; set; }
}

public class WorkWeChatSendMpNewsMessageArticleDetailDto
{
    [JsonProperty("title")]
    public string Title { get; set; }
    
    [JsonProperty("content")]
    public string Content { get; set; }
    
    [JsonProperty("content_source_url")]
    public string ContentSourceUrl { get; set; }
    
    [JsonProperty("thumb_media_id")]
    public string ThumbMediaId { get; set; }
    
    [JsonProperty("author")]
    public string Author { get; set; }
    
    [JsonProperty("digest")]
    public string Digest { get; set; }
}