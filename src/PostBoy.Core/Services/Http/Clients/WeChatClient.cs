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

    Task<UploadWorkWechatFileResponseDto> UploadWorkWechatFileAsync(
        UploadWorkWechatFileDto uploadFile, CancellationToken cancellationToken);

    Task<WorkWeChatSendMessageResponseDto> SendWorkWeChatMessageAsync(
        WorkWeChatSendMessageDto message, CancellationToken cancellationToken);
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
    
    public async Task<WorkWeChatSendMessageResponseDto> SendWorkWeChatMessageAsync(
        WorkWeChatSendMessageDto message, CancellationToken cancellationToken)
    {
        var sendUrl = $"https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={message.AccessToken}";
        var sendToChatUrl = $"https://qyapi.weixin.qq.com/cgi-bin/appchat/send?access_token={message.AccessToken}";

        return await _httpClientFactory
            .PostAsJsonAsync<WorkWeChatSendMessageResponseDto>(
                string.IsNullOrEmpty(message.ChatId) ? sendUrl : sendToChatUrl, message, cancellationToken).ConfigureAwait(false);
    }

    public async Task<UploadWorkWechatFileResponseDto> UploadWorkWechatFileAsync(
        UploadWorkWechatFileDto uploadFile, CancellationToken cancellationToken)
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

        return await _httpClientFactory.PostAsync<UploadWorkWechatFileResponseDto>(sendUrl, multipartContent, cancellationToken).ConfigureAwait(false);
    }
}