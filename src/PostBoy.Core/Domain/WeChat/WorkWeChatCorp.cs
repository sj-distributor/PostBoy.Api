using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostBoy.Core.Domain.WeChat;

[Table("work_wechat_corp")]
public class WorkWeChatCorp : IEntity
{
    [Column("id", TypeName = "char(36)")]
    public Guid Id { get; set; }
    
    [Column("corp_id"), StringLength(128)]
    public string CorpId { get; set; }
    
    [Column("corp_name"), StringLength(56)]
    public string CorpName { get; set; }
}