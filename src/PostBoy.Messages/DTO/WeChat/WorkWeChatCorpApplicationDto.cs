namespace PostBoy.Messages.DTO.WeChat;

public class WorkWeChatCorpApplicationDto
{
    public Guid Id { get; set; }
    
    public string AppId { get; set; }
    
    public Guid WorkWeChatCorpId { get; set; }
    
    public string Name { get; set; }
    
    public string Secret { get; set; }
    
    public int AgentId { get; set; }
}