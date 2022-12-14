using PostBoy.Core.Domain.WeChat;
using PostBoy.Core.Extensions;
using PostBoy.Core.Services.WeChat.Exceptions;
using PostBoy.Messages.Commands.Messages;
using PostBoy.Messages.DTO.Messages;
using PostBoy.Messages.DTO.WeChat;
using PostBoy.Messages.Enums.WeChat;
using PostBoy.Messages.Events.Messages;

namespace PostBoy.Core.Services.Messages;

public partial class MessageService
{
    public async Task<WorkWeChatAppNotificationSentEvent> SendMessageAsync(SendWorkWeChatAppNotificationCommand command, CancellationToken cancellationToken)
    {
        var (sentMessage, sentResponse, uploadFilesDic) = 
            await SendWorkWeChatAppNotificationAsync(command.WorkWeChatAppNotification, cancellationToken).ConfigureAwait(false);

        return new WorkWeChatAppNotificationSentEvent
        {
            SentMessage = sentMessage,
            SentResponse = sentResponse,
            UploadFilesDic = uploadFilesDic
        };
    }
    
    private async Task<(WorkWeChatSendMessageDto, WorkWeChatSendMessageResponseDto, Dictionary<Guid, UploadWorkWechatFileResponseDto>)> SendWorkWeChatAppNotificationAsync(
        SendWorkWeChatAppNotificationDto notificationData, CancellationToken cancellationToken)
    {
        if (notificationData == null)
            return (null, null, null);
        
        var (corp, app) = await _weChatDataProvider
            .GetWorkWeChatCorpAndApplicationByAppIdAsync(notificationData.AppId, cancellationToken).ConfigureAwait(false);

        if (corp == null || app == null)
            throw new WorkWeChatAppNotificationCorpMissingException(notificationData.AppId);
        
        var accessToken = await _weChatUtilService
            .GetWorkWeChatAccessTokenAsync(corp.CorpId, app.Secret, cancellationToken).ConfigureAwait(false);
        
        var uploadFiles = 
            await UploadWorkWeChatFilesIfRequireAsync(accessToken, notificationData, cancellationToken).ConfigureAwait(false);

        var sendMessage = GenerateWorkWeChatSendMessageAsync(accessToken, notificationData, app, uploadFiles);

        var sendResponse = await _weChatUtilService.SendWorkWeChatMessageAsync(sendMessage, cancellationToken).ConfigureAwait(false);
        
        return (sendMessage, sendResponse, uploadFiles);
    }

    private async Task<Dictionary<Guid, UploadWorkWechatFileResponseDto>> UploadWorkWeChatFilesIfRequireAsync(
        string accessToken, SendWorkWeChatAppNotificationDto notificationData, CancellationToken cancellationToken)
    {
        var uploadFilesDic = new Dictionary<Guid, UploadWorkWechatFileDto>();

        if (notificationData.File != null)
        {
            uploadFilesDic.Add(notificationData.File.Id, new UploadWorkWechatFileDto
            {
                AccessToken = accessToken,
                FileName = notificationData.File.FileName,
                FileType = notificationData.File.FileType,
                FileContent = Convert.FromBase64String(notificationData.File.FileContent)
            });
        }
        else if (notificationData.MpNews != null)
        {
            foreach (var article in notificationData.MpNews.Articles)
            {
                uploadFilesDic.Add(article.Id, new UploadWorkWechatFileDto
                {
                    AccessToken = accessToken,
                    FileType = WorkWeChatFileType.Image,
                    FileName = $"{nameof(PostBoy)}{StringExtension.GenerateRandomNumbers(6)}",
                    FileContent = Convert.FromBase64String(article.FileContent)
                });
            }
        }
        
        var uploadTasks = new List<Task>();

        var uploadTasksDic = uploadFilesDic.ToDictionary(uploadFile => uploadFile.Key,
            uploadFile => _weChatUtilService.UploadWorkWechatFileAsync(uploadFile.Value, cancellationToken));
        
        uploadTasks.AddRange(uploadTasksDic.Values);
        
        await Task.WhenAll(uploadTasks).ConfigureAwait(false);

        return uploadTasksDic.ToDictionary(uploadTask => uploadTask.Key, uploadTask => uploadTask.Value.Result);
    }
    
    private WorkWeChatSendMessageDto GenerateWorkWeChatSendMessageAsync(
        string accessToken, SendWorkWeChatAppNotificationDto notificationData, 
        WorkWeChatCorpApplication corpApp, IReadOnlyDictionary<Guid, UploadWorkWechatFileResponseDto> uploadFiles)
    {
        var sendMessage = new WorkWeChatSendMessageDto
        {
            AccessToken = accessToken,
            AgentId = corpApp.AgentId,
            ChatId = notificationData.ChatId,
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
        else if (notificationData.File != null)
        {
            var file = uploadFiles.GetValueOrDefault(notificationData.File.Id);
            
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
        else if (notificationData.MpNews != null)
        {
            sendMessage.MpNews = new WorkWeChatSendMpNewsMessageDto();
            
            foreach (var article in notificationData.MpNews.Articles)
            {
                var file = uploadFiles.GetValueOrDefault(article.Id);
                
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