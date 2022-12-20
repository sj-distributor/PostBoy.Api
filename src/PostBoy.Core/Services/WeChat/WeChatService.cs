using AutoMapper;
using PostBoy.Core.Ioc;
using PostBoy.Core.Services.WeChat.Exceptions;
using PostBoy.Messages.DTO.WeChat;
using PostBoy.Messages.Commands.WeChat;
using PostBoy.Messages.Requests.WeChat;

namespace PostBoy.Core.Services.WeChat;

public interface IWeChatService : IScopedDependency
{
    Task<CreateWorkWeChatGroupResponse> CreateWorkWeChatGroupAsync(CreateWorkWeChatGroupCommand command, CancellationToken cancellationToken);

    Task<GetWorkWeChatCorpsResponse> GetWorkWeChatCorpsAsync(
        GetWorkWeChatCorpsRequest request, CancellationToken cancellationToken);

    Task<GetWorkWeChatCorpApplicationsResponse> GetWorkWeChatCorpApplicationsAsync(
        GetWorkWeChatCorpApplicationsRequest request, CancellationToken cancellationToken);
}

public class WeChatService : IWeChatService
{
    private readonly IMapper _mapper;
    private readonly IWeChatUtilService _weChatUtilService;
    private readonly IWeChatDataProvider _weChatDataProvider;
    
    public WeChatService(IMapper mapper, IWeChatUtilService weChatUtilService, IWeChatDataProvider weChatDataProvider)
    {
        _mapper = mapper;
        _weChatUtilService = weChatUtilService;
        _weChatDataProvider = weChatDataProvider;
    }

    public async Task<CreateWorkWeChatGroupResponse> CreateWorkWeChatGroupAsync(CreateWorkWeChatGroupCommand command, CancellationToken cancellationToken)
    {
        var (corp, app) = await _weChatDataProvider
            .GetWorkWeChatCorpAndApplicationByAppIdAsync(command.AppId, cancellationToken).ConfigureAwait(false);

        if (corp == null || app == null)
            throw new WorkWeChatAppNotificationCorpMissingException(command.AppId);
        
        var postData = _mapper.Map<CreateWorkWeChatGroupDto>(command);

        postData.AccessToken = await _weChatUtilService
            .GetWorkWeChatAccessTokenAsync(corp.CorpId, app.Secret, cancellationToken).ConfigureAwait(false);
        
        var response = await _weChatUtilService
            .CreateWorkWechatGroupAsync(postData, cancellationToken).ConfigureAwait(false);
        
        return new CreateWorkWeChatGroupResponse
        {
            Data = response
        };
    }

    public async Task<GetWorkWeChatCorpsResponse> GetWorkWeChatCorpsAsync(
        GetWorkWeChatCorpsRequest request, CancellationToken cancellationToken)
    {
        var corps = await _weChatDataProvider.GetWorkWeChatCorpsAsync(cancellationToken).ConfigureAwait(false);

        return new GetWorkWeChatCorpsResponse
        {
            Data = _mapper.Map<List<WorkWeChatCorpDto>>(corps)
        };
    }
    
    public async Task<GetWorkWeChatCorpApplicationsResponse> GetWorkWeChatCorpApplicationsAsync(
        GetWorkWeChatCorpApplicationsRequest request, CancellationToken cancellationToken)
    {
        var corpApps = await _weChatDataProvider
            .GetWorkWeChatCorpApplicationsAsync(request.CorpId, cancellationToken).ConfigureAwait(false);

        return new GetWorkWeChatCorpApplicationsResponse
        {
            Data = _mapper.Map<List<WorkWeChatCorpApplicationDto>>(corpApps)
        };
    }
}