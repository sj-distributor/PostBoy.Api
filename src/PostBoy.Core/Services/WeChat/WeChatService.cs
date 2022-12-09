using AutoMapper;
using PostBoy.Core.Ioc;
using PostBoy.Core.Extensions;
using PostBoy.Messages.DTO.Messages;
using PostBoy.Messages.DTO.WeChat;
using PostBoy.Messages.Enums.WeChat;

namespace PostBoy.Core.Services.WeChat;

public interface IWeChatService : IScopedDependency
{
    Task SendWorkWeChatAppNotificationAsync(
        SendWorkWeChatAppNotificationDto notificationData, CancellationToken cancellationToken);
}

public class WeChatService : IWeChatService
{
    private readonly IMapper _mapper;
    private readonly IWeChatUtilService _weChatUtilService;

    public WeChatService(IMapper mapper, IWeChatUtilService weChatUtilService)
    {
        _mapper = mapper;
        _weChatUtilService = weChatUtilService;
    }

    public async Task SendWorkWeChatAppNotificationAsync(
        SendWorkWeChatAppNotificationDto notificationData, CancellationToken cancellationToken)
    {
        var message = await GenerateWorkWeChatSendMessageAsync(notificationData, cancellationToken).ConfigureAwait(false);
    }

    private async Task<WorkWeChatSendMessageDto> GenerateWorkWeChatSendMessageAsync(
        SendWorkWeChatAppNotificationDto notificationData, CancellationToken cancellationToken)
    {
        var sendMessage = new WorkWeChatSendMessageDto
        {
            ChatId = notificationData.ChatId,
            AgentId = 1,
            ToTag = GenerateMultiIds(notificationData.ToTags),
            ToUser = GenerateMultiIds(notificationData.ToUsers),
            ToParty = GenerateMultiIds(notificationData.ToParties)
        };

        if (notificationData.Text != null)
        {
            sendMessage.Text = new WorkWeChatSendTextMessageDto
            {
                Content = notificationData.Text.Content
            };
        }
        if (notificationData.File != null)
        {
            var file = await _weChatUtilService
                .UploadWorkWechatFileAsync(new UploadWorkWechatFileDto
                {
                    FileName = notificationData.File.FileName,
                    FileType = notificationData.File.FileType,
                    FileContent = Convert.FromBase64String(notificationData.File.FileContent)
                }, cancellationToken).ConfigureAwait(false);

            switch (notificationData.File.FileType)
            {
                case WorkWeChatFileType.Image:
                    sendMessage.Image = new WorkWeChatSendFileMessageDto
                    {
                        MediaId = file?.MediaId
                    };
                    break;
                case WorkWeChatFileType.Voice:
                    sendMessage.Voice = new WorkWeChatSendFileMessageDto
                    {
                        MediaId = file?.MediaId
                    };
                    break;
                case WorkWeChatFileType.Video:
                    sendMessage.Video = new WorkWeChatSendFileMessageDto
                    {
                        MediaId = file?.MediaId
                    };
                    break;
                case WorkWeChatFileType.File:
                    sendMessage.File = new WorkWeChatSendFileMessageDto
                    {
                        MediaId = file?.MediaId
                    };
                    break;
            }
        }
        if (notificationData.MpNews != null)
        {
            sendMessage.MpNews = new WorkWeChatSendMpNewsMessageDto();
            
            foreach (var article in notificationData.MpNews.Articles)
            {
                var file = await _weChatUtilService
                    .UploadWorkWechatFileAsync(new UploadWorkWechatFileDto
                    {
                        FileType = WorkWeChatFileType.Image,
                        FileName = $"{nameof(PostBoy)}{StringExtension.GenerateRandomNumbers(6)}",
                        FileContent = Convert.FromBase64String(article.FileContent)
                    }, cancellationToken).ConfigureAwait(false);
                
                sendMessage.MpNews.Articles.Add(new WorkWeChatSendMpNewsMessageDetailDto
                {
                    Title = article.Title,
                    Author = article.Author,
                    Digest = article.Digest,
                    Content = article.Content,
                    ThumbMediaId = file?.MediaId,
                    ContentSourceUrl = article.ContentSourceUrl
                });
            }
        }
        
        return sendMessage;
    }
    
    private static string GenerateMultiIds(IEnumerable<string> ids)
    {
        return string.Join("|", ids);
    }
}