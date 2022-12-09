using PostBoy.Core.Ioc;
using PostBoy.Messages.DTO.WeChat;

namespace PostBoy.Core.Services.Http.Clients;

public interface IWeChatClient : IScopedDependency
{
    Task<string> GetWorkWeChatAccessTokenAsync(
        string corpId, string corpSecret, CancellationToken cancellationToken);

    Task<byte[]> GetWorkWeChatMediaAsync(
        string accessToken, string mediaId, CancellationToken cancellationToken);

    Task<CreateWorkWeChatGroupResponseDto> CreateWorkWechatGroupAsync(
        CreateWorkWeChatGroupDto createWorkWechatGroup, CancellationToken cancellationToken);

    Task<UploadWorkWechatTemporaryFileResponseDto> UploadWorkWechatTemporaryFileAsync(
        UploadWorkWechatTemporaryFileDto uploadFile, CancellationToken cancellationToken);
    
    Task<WorkWeChatSendMessageResponseDto> SendWorkWeChatTextMessageAsync(
        WorkWeChatSendTextMessageDto textMessage, CancellationToken cancellationToken);
    
    Task<WorkWeChatSendMessageResponseDto> SendWorkWeChatImageMessageAsync(
        WorkWeChatSendImageMessageDto imageMessage, CancellationToken cancellationToken);
    
    Task<WorkWeChatSendMessageResponseDto> SendWorkWeChatVoiceMessageAsync(
        WorkWeChatSendVoiceMessageDto voiceMessage, CancellationToken cancellationToken);
    
    Task<WorkWeChatSendMessageResponseDto> SendWorkWeChatVideoMessageAsync(
        WorkWeChatSendVideoMessageDto videoMessage, CancellationToken cancellationToken);
    
    Task<WorkWeChatSendMessageResponseDto> SendWorkWeChatFileMessageAsync(
        WorkWeChatSendFileMessageDto fileMessage, CancellationToken cancellationToken);
    
    Task<WorkWeChatSendMessageResponseDto> SendWorkWeChatMpNewsMessageAsync(
        WorkWeChatSendMpNewsMessageDto mpNewsMessage, CancellationToken cancellationToken);
    
}

public class WeChatClient : IWeChatClient
{
    private readonly IPostBoyHttpClientFactory _httpClientFactory;

    public WeChatClient(IPostBoyHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> GetWorkWeChatAccessTokenAsync(
        string corpId, string corpSecret, CancellationToken cancellationToken)
    {
        var tokenUrl = $"https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={corpId}&corpsecret={corpSecret}";

        var tokenResult = await _httpClientFactory
            .GetAsync<WorkWeChatAccessTokenDto>(tokenUrl, cancellationToken).ConfigureAwait(false);

        return tokenResult?.AccessToken;
    }

    public async Task<byte[]> GetWorkWeChatMediaAsync(
        string accessToken, string mediaId, CancellationToken cancellationToken)
    {
        var mediaUrl = $"https://qyapi.weixin.qq.com/cgi-bin/media/get?access_token={accessToken}&media_id={mediaId}";
        
        return await _httpClientFactory
            .GetAsync<byte[]>(mediaUrl, cancellationToken).ConfigureAwait(false);
    }

    public async Task<CreateWorkWeChatGroupResponseDto> CreateWorkWechatGroupAsync(
        CreateWorkWeChatGroupDto createWorkWechatGroup, CancellationToken cancellationToken)
    {
        var sendUrl = $"https://qyapi.weixin.qq.com/cgi-bin/appchat/create?access_token={createWorkWechatGroup.AccessToken}";

        return await _httpClientFactory
            .PostAsJsonAsync<CreateWorkWeChatGroupResponseDto>(sendUrl, createWorkWechatGroup,
                cancellationToken).ConfigureAwait(false);
    }
    
    public async Task<WorkWeChatSendMessageResponseDto> SendWorkWeChatTextMessageAsync(
        WorkWeChatSendTextMessageDto textMessage, CancellationToken cancellationToken)
    {
        return await SendWorkWeChatMessageAsync(textMessage, cancellationToken).ConfigureAwait(false);
    }

    public async Task<WorkWeChatSendMessageResponseDto> SendWorkWeChatImageMessageAsync(
        WorkWeChatSendImageMessageDto imageMessage, CancellationToken cancellationToken)
    {
        return await SendWorkWeChatMessageAsync(imageMessage, cancellationToken).ConfigureAwait(false);
    }

    public async Task<WorkWeChatSendMessageResponseDto> SendWorkWeChatVoiceMessageAsync(
        WorkWeChatSendVoiceMessageDto voiceMessage, CancellationToken cancellationToken)
    {
        return await SendWorkWeChatMessageAsync(voiceMessage, cancellationToken).ConfigureAwait(false);
    }

    public async Task<WorkWeChatSendMessageResponseDto> SendWorkWeChatVideoMessageAsync(
        WorkWeChatSendVideoMessageDto videoMessage, CancellationToken cancellationToken)
    {
        return await SendWorkWeChatMessageAsync(videoMessage, cancellationToken).ConfigureAwait(false);
    }

    public async Task<WorkWeChatSendMessageResponseDto> SendWorkWeChatFileMessageAsync(
        WorkWeChatSendFileMessageDto fileMessage, CancellationToken cancellationToken)
    {
        return await SendWorkWeChatMessageAsync(fileMessage, cancellationToken).ConfigureAwait(false);
    }

    public async Task<WorkWeChatSendMessageResponseDto> SendWorkWeChatMpNewsMessageAsync(
        WorkWeChatSendMpNewsMessageDto mpNewsMessage, CancellationToken cancellationToken)
    {
        return await SendWorkWeChatMessageAsync(mpNewsMessage, cancellationToken).ConfigureAwait(false);
    }

    private async Task<WorkWeChatSendMessageResponseDto> SendWorkWeChatMessageAsync(
        WorkWeChatSendMessageBaseDto message, CancellationToken cancellationToken)
    {
        var sendUrl = $"https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={message.AccessToken}";

        return await _httpClientFactory
            .PostAsJsonAsync<WorkWeChatSendMessageResponseDto>(sendUrl, message,
                cancellationToken).ConfigureAwait(false);
    }

    public async Task<UploadWorkWechatTemporaryFileResponseDto> UploadWorkWechatTemporaryFileAsync(
        UploadWorkWechatTemporaryFileDto uploadFile, CancellationToken cancellationToken)
    {
        var boundary = DateTime.Now.Ticks.ToString("X");
        
        var byteContent = new ByteArrayContent(uploadFile.FileContent);
        var multipartContent = new MultipartFormDataContent(boundary);
        
        multipartContent.Headers.Remove("Content-Type");
        multipartContent.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data; boundary=" + boundary);

        byteContent.Headers.Remove("Content-Type");
        byteContent.Headers.Remove("Content-Disposition");
        byteContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
        byteContent.Headers.TryAddWithoutValidation("Content-Disposition", $"form-data; name=\"media\";filename=\"{uploadFile.FileName}\";filelength={uploadFile.FileContent.Length}");

        multipartContent.Add(byteContent);
        
        var sendUrl = $"https://qyapi.weixin.qq.com/cgi-bin/media/upload?access_token={uploadFile.AccessToken}&type={uploadFile.FileType.ToString().ToLower()}";

        return await _httpClientFactory.PostAsync<UploadWorkWechatTemporaryFileResponseDto>(sendUrl, multipartContent, cancellationToken).ConfigureAwait(false);
    }
}