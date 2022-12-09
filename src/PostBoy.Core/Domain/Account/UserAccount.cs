using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostBoy.Core.Domain.Account;

[Table("user_account")]
public class UserAccount : IEntity
{
    public UserAccount()
    {
        Uuid = Guid.NewGuid();
        CreatedOn = DateTime.Now;
        ModifiedOn = DateTime.Now;
    }
    
    [Key]
    [Column("id", TypeName = "varchar(36)")]
    public Guid Id { get; set; }

    [Column("created_on")]
    public DateTime CreatedOn { get; set; }
    
    [Column("modified_on")]
    public DateTime ModifiedOn { get; set; }
    
    [Column("uuid", TypeName = "varchar(36)")]
    public Guid Uuid { get; set; }
    
    [Column("username")]
    [StringLength(512)]
    public string UserName { get; set; }
    
    [Column("password")]
    [StringLength(64)]
    public string Password { get; set; }
    
    [Column("active", TypeName = "tinyint(1)")]
    public bool IsActive { get; set; }
}