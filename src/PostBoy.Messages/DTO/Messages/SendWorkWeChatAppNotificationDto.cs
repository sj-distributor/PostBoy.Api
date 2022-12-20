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
    
    /// <summary>
    /// 企业微信应用Id，由PostBoy提供
    /// </summary>
    public string AppId { get; set; }
    
    /// <summary>
    /// 企业微信群聊Id
    /// </summary>
    public string ChatId { get; set; }

    /// <summary>
    /// 指定接收消息的标签，标签ID列表，最多支持100个。
    /// 当ToUsers为"@all"时忽略本参数
    /// </summary>
    public List<string> ToTags { get; set; }

    /// <summary>
    /// 指定接收消息的成员，成员ID列表（最多支持1000个）。
    /// 特殊情况：指定为"@all"，则向该企业应用的全部成员发送
    /// </summary>
    public List<string> ToUsers { get; set; }
    
    /// <summary>
    /// 指定接收消息的部门，部门ID列表，最多支持100个。
    /// 当ToUsers为"@all"时忽略本参数
    /// </summary>
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