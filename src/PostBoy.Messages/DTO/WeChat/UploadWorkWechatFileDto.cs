using Newtonsoft.Json;
using PostBoy.Messages.Enums.WeChat;

namespace PostBoy.Messages.DTO.WeChat;

public class UploadWorkWechatFileDto : INeedAccessToken
{
    public string AccessToken { get; set; }
    
    public string FileName { get; set; }
    
    public byte[] FileContent { get; set; }
    
    public WorkWeChatFileType FileType { get; set; }
}

public class UploadWorkWechatFileResponseDto : WorkWeChatResponseBaseDto
{
    [JsonProperty("type")]
    public string Type { get; set; }
    
    [JsonProperty("media_id")]
    public string MediaId { get; set; }
    
    [JsonProperty("created_at")]
    public string CreateAt { get; set; }
}