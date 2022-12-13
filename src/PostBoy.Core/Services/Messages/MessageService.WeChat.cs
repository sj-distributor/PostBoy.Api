using PostBoy.Core.Domain.WeChat;
using PostBoy.Core.Extensions;
using PostBoy.Core.Services.WeChat.Exceptions;
using PostBoy.Messages.Commands.Messages;
using PostBoy.Messages.DTO.Messages;
using PostBoy.Messages.DTO.WeChat;
using PostBoy.Messages.Enums.WeChat;
using PostBoy.Messages.Events.Messages;
using Serilog;

namespace PostBoy.Core.Services.Messages;

public partial class MessageService
{
    public async Task<WorkWeChatAppNotificationSentEvent> SendMessageAsync(SendWorkWeChatAppNotificationCommand command, CancellationToken cancellationToken)
    {
        var sentMessage = await SendWorkWeChatAppNotificationAsync(command.WorkWeChatAppNotification, cancellationToken).ConfigureAwait(false);

        return new WorkWeChatAppNotificationSentEvent
        {
            SentMessage = sentMessage
        };
    }
    
    private async Task<WorkWeChatSendMessageDto> SendWorkWeChatAppNotificationAsync(
        SendWorkWeChatAppNotificationDto notificationData, CancellationToken cancellationToken)
    {
        if (notificationData == null)
            return null;
        
        var (corp, app) = await _weChatDataProvider
            .GetWorkWeChatCorpAndApplicationByAppIdAsync(notificationData.AppId, cancellationToken).ConfigureAwait(false);

        if (corp == null || app == null)
            throw new WorkWeChatAppNotificationCorpMissingException(notificationData.AppId);
        
        var message = await GenerateWorkWeChatSendMessageAsync(notificationData, corp, app, cancellationToken).ConfigureAwait(false);

        var response = await _weChatUtilService.SendWorkWeChatMessageAsync(message, cancellationToken).ConfigureAwait(false);
        
        Log.Information("Send work wechat message: {@Message}, response: {@Response}", message, response);

        return message;
    }

    private async Task<WorkWeChatSendMessageDto> GenerateWorkWeChatSendMessageAsync(
        SendWorkWeChatAppNotificationDto notificationData, WorkWeChatCorp corp, WorkWeChatCorpApplication app, CancellationToken cancellationToken)
    {
        var sendMessage = new WorkWeChatSendMessageDto
        {
            AgentId = app.AgentId,
            ChatId = notificationData.ChatId,
            ToTag = GenerateMultiIds(notificationData.ToTags),
            ToUser = GenerateMultiIds(notificationData.ToUsers),
            ToParty = GenerateMultiIds(notificationData.ToParties),
            AccessToken = await _weChatUtilService.GetWorkWeChatAccessTokenAsync(corp.CorpId, app.Secret, cancellationToken).ConfigureAwait(false)
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