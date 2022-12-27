using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostBoy.Core.Domain.Authentication;

public class UserAccountApiKey : IEntity
{
    [Key]
    [Column("id", TypeName = "varchar(36)")]
    public Guid Id { get; set; }
    
    [Column("user_account_id")]
    public int UserAccountId { get; set; }
    
    [Column("api_key")]
    [StringLength(128)]
    public string ApiKey { get; set; }
}