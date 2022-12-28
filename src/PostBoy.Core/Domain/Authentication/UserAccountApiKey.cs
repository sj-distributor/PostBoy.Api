using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostBoy.Core.Domain.Authentication;

[Table("user_account_api_key")]
public class UserAccountApiKey : IEntity
{
    [Key]
    [Column("id", TypeName = "char(36)")]
    public Guid Id { get; set; }
    
    [Column("user_account_id", TypeName = "char(36)")]
    public Guid UserAccountId { get; set; }
    
    [Column("api_key")]
    [StringLength(128)]
    public string ApiKey { get; set; }
    
    [Column("description")]
    [StringLength(256)]
    public string Description { get; set; }
}