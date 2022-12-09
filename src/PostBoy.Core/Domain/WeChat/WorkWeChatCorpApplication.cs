using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostBoy.Core.Domain.WeChat;

[Table("work_wechat_corp_application")]
public class WorkWeChatCorpApplication : IEntity
{
    [Column("id", TypeName = "char(36)")]
    public Guid Id { get; set; }
    
    [Column("app_id"), StringLength(128)]
    public string AppId { get; set; }
    
    [Column("work_wechat_corp_id", TypeName = "char(36)")]
    public Guid WorkWeChatCorpId { get; set; }
    
    [Column("name"), StringLength(56)]
    public string Name { get; set; }
    
    [Column("secret"), StringLength(1024)]
    public string Secret { get; set; }
    
    [Column("agent_id")]
    public int AgentId { get; set; }
}