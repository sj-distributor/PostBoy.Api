using AutoMapper;
using PostBoy.Core.Data;
using PostBoy.Core.Ioc;

namespace PostBoy.Core.Services.WeChat;

public interface IWeChatDataProvider : IScopedDependency
{
    
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
    
    
}