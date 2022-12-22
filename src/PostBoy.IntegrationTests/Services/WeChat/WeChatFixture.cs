using Mediator.Net;
using PostBoy.IntegrationTests.TestBaseClasses;
using PostBoy.IntegrationTests.Utils.WeChat;
using Shouldly;
using Xunit;

namespace PostBoy.IntegrationTests.Services.WeChat;

public class WeChatFixture : WeChatFixtureBase
{
    private readonly WeChatUtil _weChatUtil;

    public WeChatFixture()
    {
        _weChatUtil = new WeChatUtil(CurrentScope);
    }

    [Fact]
    public async Task ShouldGetWorkWeChatCorps()
    {
        await _weChatUtil.AddWorkWeChatCorp(Guid.NewGuid(), "test_corp1", "test_corp_name1");
        await _weChatUtil.AddWorkWeChatCorp(Guid.NewGuid(), "test_corp2", "test_corp_name2");
        await _weChatUtil.AddWorkWeChatCorp(Guid.NewGuid(), "test_corp3", "test_corp_name3");

        var corps = await _weChatUtil.GetWorkWeChatCorps();
        
        corps.Count.ShouldBe(3);
    }

    [Fact]
    public async Task ShouldGetWorkWeChatCorpApplications()
    {
        var corpId1 = Guid.NewGuid();
        var corpId2 = Guid.NewGuid();
        var corpId3 = Guid.NewGuid();
        
        await _weChatUtil.AddWorkWeChatCorp(corpId1, "test_corp1", "test_corp_name1");
        await _weChatUtil.AddWorkWeChatCorp(corpId2, "test_corp2", "test_corp_name2");
        await _weChatUtil.AddWorkWeChatCorp(corpId3, "test_corp3", "test_corp_name3");

        await _weChatUtil.AddWorkWeChatCorpApplication(Guid.NewGuid(), corpId1, "test_app_id1","测试应用1", "test_secret1", 100000);
        await _weChatUtil.AddWorkWeChatCorpApplication(Guid.NewGuid(), corpId1, "test_app_id2","测试应用2", "test_secret2", 100001);
        
        var apps1 = await _weChatUtil.GetWorkWeChatCorpApplications(corpId1);
        var apps2 = await _weChatUtil.GetWorkWeChatCorpApplications(corpId2);
        
        apps1.Count.ShouldBe(2);
        apps2.Count.ShouldBe(0);
    }
}