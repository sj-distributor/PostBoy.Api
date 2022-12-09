using Newtonsoft.Json;

namespace PostBoy.Messages.DTO.WeChat;

public class UploadWorkWechatTemporaryFileDto : INeedAccessToken
{
    public string AccessToken { get; set; }
    
    public string FileName { get; set; }
    
    public byte[] FileContent { get; set; }
    
    public UploadWorkWechatTemporaryFileType FileType { get; set; }
}

public class UploadWorkWechatTemporaryFileResponseDto : WorkWeChatResponseBaseDto
{
    [JsonProperty("type")]
    public string Type { get; set; }
    
    [JsonProperty("media_id")]
    public string MediaId { get; set; }
    
    [JsonProperty("created_at")]
    public string CreateAt { get; set; }
}

public enum UploadWorkWechatTemporaryFileType
{
    Image,
    Voice,
    Video,
    File
}