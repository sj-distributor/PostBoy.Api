using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostBoy.Core.Domain.Account;

[Table("role_user")]
public class RoleUser : IEntity
{
    [Key]
    [Column("id", TypeName = "varchar(36)")]
    public Guid Id { get; set; }
    
    [Column("created_on")]
    public DateTime CreatedOn { get; set; }
    
    [Column("modified_on")]
    public DateTime ModifiedOn { get; set; }

    [Column("role_id", TypeName = "varchar(36)")]
    public Guid RoleId { get; set; }
    
    [Column("user_id", TypeName = "varchar(36)")]
    public Guid UserId { get; set; }
}