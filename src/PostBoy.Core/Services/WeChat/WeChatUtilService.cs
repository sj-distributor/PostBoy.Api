using PostBoy.Core.Ioc;
using PostBoy.Core.Services.Http.Clients;
using PostBoy.Messages.DTO.WeChat;

namespace PostBoy.Core.Services.WeChat;

public interface IWeChatUtilService : IScopedDependency
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

public class WeChatUtilService : IWeChatUtilService
{
    private readonly IWeChatClient _weChatClient;

    public WeChatUtilService(IWeChatClient weChatClient)
    {
        _weChatClient = weChatClient;
    }

    public async Task<string> GetWorkWeChatAccessTokenAsync(
        string corpId, string corpSecret, CancellationToken cancellationToken)
    {
        return await _weChatClient
            .GetWorkWeChatAccessTokenAsync(corpId, corpSecret, cancellationToken).ConfigureAwait(false);
    }

    public async Task<byte[]> GetWorkWeChatMediaAsync(
        string accessToken, string mediaId, CancellationToken cancellationToken)
    {
        return await _weChatClient
            .GetWorkWeChatMediaAsync(accessToken, mediaId, cancellationToken).ConfigureAwait(false);
    }

    public async Task<CreateWorkWeChatGroupResponseDto> CreateWorkWechatGroupAsync(
        CreateWorkWeChatGroupDto createWorkWechatGroup, CancellationToken cancellationToken)
    {
        return await _weChatClient
            .CreateWorkWechatGroupAsync(createWorkWechatGroup, cancellationToken).ConfigureAwait(false);
    }

    public async Task<UploadWorkWechatFileResponseDto> UploadWorkWechatFileAsync(
        UploadWorkWechatFileDto uploadFile, CancellationToken cancellationToken)
    {
        return await _weChatClient
            .UploadWorkWechatFileAsync(uploadFile, cancellationToken).ConfigureAwait(false);
    }

    public async Task<WorkWeChatSendMessageResponseDto> SendWorkWeChatMessageAsync(
        WorkWeChatSendMessageDto message, CancellationToken cancellationToken)
    {
        return await _weChatClient
            .SendWorkWeChatMessageAsync(message, cancellationToken).ConfigureAwait(false);
    }
}