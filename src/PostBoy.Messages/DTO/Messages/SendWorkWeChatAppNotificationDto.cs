using Newtonsoft.Json;
using PostBoy.Messages.Enums.WeChat;

namespace PostBoy.Messages.DTO.Messages;

public class SendWorkWeChatAppNotificationDto
{
    public SendWorkWeChatAppNotificationDto()
    {
        ToTags = new List<string>();
        ToUsers = new List<string>();
        ToParties = new List<string>();
    }
    
    public string AppId { get; set; }
    
    public string ChatId { get; set; }

    public List<string> ToTags { get; set; }

    public List<string> ToUsers { get; set; }
    
    public List<string> ToParties { get; set; }
    
    /// <summary>
    /// 文本消息
    /// </summary>
    public SendWorkWeChatTextNotificationDto Text { get; set; }
    
    /// <summary>
    /// 文件消息
    /// </summary>
    public SendWorkWeChatFileNotificationDto File { get; set; }
    
    /// <summary>
    /// 图文消息
    /// </summary>
    public SendWorkWeChatMpNewsNotificationDto MpNews { get; set; }
}

public class SendWorkWeChatTextNotificationDto
{
    /// <summary>
    /// 消息内容
    /// </summary>
    public string Content { get; set; }
}

public class SendWorkWeChatFileNotificationDto
{
    [JsonIgnore]
    public Guid Id { get; } = Guid.NewGuid();
    
    /// <summary>
    /// 文件名称
    /// </summary>
    public string FileName { get; set; }
    
    /// <summary>
    /// 文件BASE64
    /// </summary>
    public string FileContent { get; set; }
    
    /// <summary>
    /// 文件类型
    /// </summary>
    public WorkWeChatFileType FileType { get; set; }
}

public class SendWorkWeChatMpNewsNotificationDto
{
    public SendWorkWeChatMpNewsNotificationDto()
    {
        Articles = new List<SendWorkWeChatArticleNotificationDto>();
    }
    
    /// <summary>
    /// 图文消息，一个图文消息支持1到8条图文
    /// </summary>
    public List<SendWorkWeChatArticleNotificationDto> Articles { get; set; }
}

public class SendWorkWeChatArticleNotificationDto
{
    [JsonIgnore]
    public Guid Id { get; } = Guid.NewGuid();
    
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// 图文消息的作者
    /// </summary>
    public string Author { get; set; }
    
    /// <summary>
    /// 图文消息的描述
    /// </summary>
    public string Digest { get; set; }

    /// <summary>
    /// 图文消息的内容
    /// </summary>
    public string Content { get; set; }
    
    /// <summary>
    /// 图文消息缩略图的BASE64
    /// </summary>
    public string FileContent { get; set; }
    
    /// <summary>
    /// 图文消息点击“阅读原文”之后的页面链接
    /// </summary>
    public string ContentSourceUrl { get; set; }
}