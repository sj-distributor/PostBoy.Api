using Mediator.Net.Contracts;
using PostBoy.Messages.DTO.WeChat;
using PostBoy.Messages.Responses;

namespace PostBoy.Messages.Commands.WeChat;

public class CreateWorkWeChatGroupCommand : ICommand
{
    /// <summary>
    /// 应用唯一标识，由PostBoy提供
    /// </summary>
    public string AppId { get; set; }
    
    /// <summary>
    /// 群聊名
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// 指定群主的id。如果不指定，系统会随机从UserList中选一人作为群主
    /// </summary>
    public string Owner { get; set; }
    
    /// <summary>
    /// 群聊的唯一标志，如果不填，系统会随机生成群id
    /// </summary>
    public string ChatId { get; set; }

    /// <summary>
    /// 群成员id列表，至少2人
    /// </summary>
    public List<string> UserList { get; set; }
}

public class CreateWorkWeChatGroupResponse : PostBoyResponse<CreateWorkWeChatGroupResponseDto>
{
}