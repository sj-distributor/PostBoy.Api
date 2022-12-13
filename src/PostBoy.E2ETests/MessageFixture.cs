using Microsoft.Extensions.DependencyInjection;
using PostBoy.Core.Data;
using PostBoy.Core.Domain.Account;
using PostBoy.Core.Domain.WeChat;
using PostBoy.Messages.Commands.Messages;
using PostBoy.Messages.DTO.Messages;

namespace PostBoy.E2ETests;

[Collection("Sequential")]
public class MessageFixture : IClassFixture<ApiTestFixture>
{
    private readonly ApiTestFixture _factory;

    private const string AppId = "PostBoy_AppId";
    private readonly List<string> _testUsers = new() { "mars" }; 

    public MessageFixture(ApiTestFixture factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ShouldSendWorkWeChatTextNotification()
    {
        await CreateWorkWeChatCorpAndApplication();

        var client = _factory.CreateClient();

        await client.PostAsJsonAsync("api/message/send", new SendMessageCommand
        {
            WorkWeChatAppNotification = new SendWorkWeChatAppNotificationDto
            {
                AppId = AppId,
                ToUsers = _testUsers,
                Text = new SendWorkWeChatTextNotificationDto
                {
                    Content = "TEST"
                }
            }
        });
    }

    private async Task CreateWorkWeChatCorpAndApplication()
    {
        var repo = _factory.Services.GetRequiredService<IRepository>();
        var unitOfWork = _factory.Services.GetRequiredService<IUnitOfWork>();

        var corpId = Guid.NewGuid();
        
        await repo.InsertAsync(new WorkWeChatCorp
        {
            Id = corpId,
            CorpId = "ww7ba014f121ddabef",
            CorpName = "人类问题研究中心"
        });
        await repo.InsertAsync(new WorkWeChatCorpApplication
        {
            WorkWeChatCorpId = corpId,
            AppId = AppId,
            Name = "PostBoy",
            AgentId = 1000013,
            Secret = "5nNH6F5ImMNLOm3p-Q4BdERsL8JA4EsaNBGurivoGOo"
        });
        
        await unitOfWork.SaveChangesAsync();
    }
}