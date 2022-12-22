using Autofac;
using Mediator.Net;
using PostBoy.Core.Data;
using PostBoy.Core.Domain.WeChat;
using PostBoy.Messages.DTO.WeChat;
using PostBoy.Messages.Requests.WeChat;

namespace PostBoy.IntegrationTests.Utils.WeChat;

public class WeChatUtil : TestUtil
{
    public WeChatUtil(ILifetimeScope scope) : base(scope)
    {
    }
    
    public async Task<WorkWeChatCorp> AddWorkWeChatCorp(Guid id, string corpId, string corpName)
    {
        return await RunWithUnitOfWork<IRepository, WorkWeChatCorp>(async repository =>
        {
            var corp = new WorkWeChatCorp
            {
                Id = id,
                CorpId = corpId,
                CorpName = corpName
            };
            await repository.InsertAsync(corp);
            return corp;
        });
    }
    
    public async Task<WorkWeChatCorpApplication> AddWorkWeChatCorpApplication(Guid id, Guid corpId, string appId, string name, string secret, int agentId)
    {
        return await RunWithUnitOfWork<IRepository, WorkWeChatCorpApplication>(async repository =>
        {
            var app = new WorkWeChatCorpApplication
            {
                Id = id,
                AppId = appId,
                WorkWeChatCorpId = corpId,
                Name = name,
                Secret = secret,
                AgentId = agentId
            };
            await repository.InsertAsync(app);
            return app;
        });
    }

    public async Task<List<WorkWeChatCorpDto>> GetWorkWeChatCorps()
    {
        return await Run<IMediator, List<WorkWeChatCorpDto>>(async mediator =>
        {
            var response =
                await mediator.RequestAsync<GetWorkWeChatCorpsRequest, GetWorkWeChatCorpsResponse>(
                    new GetWorkWeChatCorpsRequest());

            return response.Data;
        });
    }
    
    public async Task<List<WorkWeChatCorpApplicationDto>> GetWorkWeChatCorpApplications(Guid corpId)
    {
        return await Run<IMediator, List<WorkWeChatCorpApplicationDto>>(async mediator =>
        {
            var response =
                await mediator.RequestAsync<GetWorkWeChatCorpApplicationsRequest, GetWorkWeChatCorpApplicationsResponse>(
                    new GetWorkWeChatCorpApplicationsRequest
                    {
                        CorpId = corpId
                    });

            return response.Data;
        });
    }
}