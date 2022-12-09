using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostBoy.Core.Domain.Account;

[Table("role_user")]
public class RoleUser : IEntity
{
    [Key]
    [Column("id", TypeName = "varchar(36)")]
    public Guid Id { get; set; }
    
    [Column("created_date")]
    public DateTime CreatedDate { get; set; }
    
    [Column("modified_date")]
    public DateTime ModifiedDate { get; set; }

    [Column("role_id", TypeName = "varchar(36)")]
    public Guid RoleId { get; set; }
    
    [Column("user_id", TypeName = "varchar(36)")]
    public Guid UserId { get; set; }
}