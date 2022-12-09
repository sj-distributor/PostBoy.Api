using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostBoy.Core.Domain.Account;

[Table("role")]
public class Role : IEntity
{
    [Key]
    [Column("id", TypeName = "varchar(36)")]
    public Guid Id { get; set; }
    
    [Column("created_date")]
    public DateTime CreatedDate { get; set; }
    
    [Column("modified_date")]
    public DateTime ModifiedDate { get; set; }

    [Column("name")]
    [StringLength(512)]
    public string Name { get; set; }
}