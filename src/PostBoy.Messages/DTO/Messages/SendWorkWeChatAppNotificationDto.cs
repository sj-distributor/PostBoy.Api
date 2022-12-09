namespace PostBoy.Messages.DTO.Messages;

public class SendWorkWeChatAppNotificationDto
{
    public SendWorkWeChatAppNotificationDto()
    {
        ToUsers = new List<string>();
        ToParties = new List<string>();
        ToTags = new List<string>();
    }
    
    public string ChatId { get; set; }

    public List<string> ToUsers { get; set; }
    
    public List<string> ToParties { get; set; }
    
    public List<string> ToTags { get; set; }
    
    public SendWorkWeChatTextNotificationDto Text { get; set; }
    
    public SendWorkWeChatImageNotificationDto Image { get; set; }
    
    public SendWorkWeChatVoiceNotificationDto Voice { get; set; }
    
    public SendWorkWeChatVideoNotificationDto Video { get; set; }
    
    public SendWorkWeChatFileNotificationDto File { get; set; }
    
    public SendWorkWeChatMpNewsNotificationDto MpNews { get; set; }
}

public class SendWorkWeChatTextNotificationDto
{
    public string Content { get; set; }
}

public class SendWorkWeChatImageNotificationDto : SendWorkWeChatMediaNotificationDto
{
}

public class SendWorkWeChatVoiceNotificationDto : SendWorkWeChatMediaNotificationDto
{
}

public class SendWorkWeChatVideoNotificationDto : SendWorkWeChatMediaNotificationDto
{
    public string Title { get; set; }
    
    public string Description { get; set; }
}

public class SendWorkWeChatFileNotificationDto : SendWorkWeChatMediaNotificationDto
{
}

public class SendWorkWeChatMpNewsNotificationDto
{
    public List<SendWorkWeChatArticleNotificationDto> Articles { get; set; }
}

public class SendWorkWeChatArticleNotificationDto : SendWorkWeChatMediaNotificationDto
{
    public string Title { get; set; }
    
    public string Author { get; set; }
    
    public string Digest { get; set; }

    public string Content { get; set; }
    
    public string ContentSourceUrl { get; set; }
}

public class SendWorkWeChatMediaNotificationDto
{
    public byte[] Media { get; set; }
}