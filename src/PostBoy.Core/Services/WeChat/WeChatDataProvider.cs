using AutoMapper;
using PostBoy.Core.Ioc;
using PostBoy.Core.Data;
using PostBoy.Core.Domain.WeChat;

namespace PostBoy.Core.Services.WeChat;

public interface IWeChatDataProvider : IScopedDependency
{
    Task<List<WorkWeChatCorp>> GetWorkWeChatCorpsAsync(CancellationToken cancellationToken);
    
    Task<List<WorkWeChatCorpApplication>> GetWorkWeChatCorpApplicationsAsync(Guid corpId, CancellationToken cancellationToken);
    
    Task<(WorkWeChatCorp, WorkWeChatCorpApplication)>
        GetWorkWeChatCorpAndApplicationByAppIdAsync(string appId, CancellationToken cancellationToken);
}

public class WeChatDataProvider : IWeChatDataProvider
{
    private readonly IMapper _mapper;
    private readonly IRepository _repository;

    public WeChatDataProvider(IMapper mapper, IRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<List<WorkWeChatCorp>> GetWorkWeChatCorpsAsync(CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync<WorkWeChatCorp>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<List<WorkWeChatCorpApplication>> GetWorkWeChatCorpApplicationsAsync(Guid corpId, CancellationToken cancellationToken)
    {
        return await _repository
            .ToListAsync<WorkWeChatCorpApplication>(x => x.WorkWeChatCorpId == corpId, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<(WorkWeChatCorp, WorkWeChatCorpApplication)> 
        GetWorkWeChatCorpAndApplicationByAppIdAsync(string appId, CancellationToken cancellationToken)
    {
        var app = await _repository
            .SingleOrDefaultAsync<WorkWeChatCorpApplication>(x => x.AppId == appId, cancellationToken).ConfigureAwait(false);

        if (app == null)
            return (null, null);
        
        var corp = await _repository
            .SingleOrDefaultAsync<WorkWeChatCorp>(x => x.Id == app.WorkWeChatCorpId, cancellationToken).ConfigureAwait(false);

        return (corp, app);
    }
}