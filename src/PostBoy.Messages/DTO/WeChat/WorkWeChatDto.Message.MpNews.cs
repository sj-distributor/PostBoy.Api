using Newtonsoft.Json;

namespace PostBoy.Messages.DTO.WeChat;

public class WorkWeChatSendMpNewsMessageDto
{
    public WorkWeChatSendMpNewsMessageDto()
    {
        Articles = new List<WorkWeChatSendMpNewsMessageDetailDto>();
    }
    
    [JsonProperty("articles")]
    public List<WorkWeChatSendMpNewsMessageDetailDto> Articles { get; set; }
}

public class WorkWeChatSendMpNewsMessageDetailDto
{
    [JsonProperty("title")]
    public string Title { get; set; }
    
    [JsonProperty("author")]
    public string Author { get; set; }
    
    [JsonProperty("digest")]
    public string Digest { get; set; }
    
    [JsonProperty("content")]
    public string Content { get; set; }
    
    [JsonProperty("content_source_url")]
    public string ContentSourceUrl { get; set; }
    
    [JsonProperty("thumb_media_id")]
    public string ThumbMediaId { get; set; }
}