using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostBoy.Core.Domain.Account;

[Table("user_account")]
public class UserAccount : IEntity
{
    public UserAccount()
    {
        CreatedDate = DateTime.Now;
        ModifiedDate = DateTime.Now;
    }
    
    [Key]
    [Column("id", TypeName = "varchar(36)")]
    public Guid Id { get; set; }

    [Column("created_date")]
    public DateTime CreatedDate { get; set; }
    
    [Column("modified_date")]
    public DateTime ModifiedDate { get; set; }
    
    [Column("username")]
    [StringLength(512)]
    public string UserName { get; set; }
    
    [Column("password")]
    [StringLength(64)]
    public string Password { get; set; }
    
    [Column("active", TypeName = "tinyint(1)")]
    public bool IsActive { get; set; }
}