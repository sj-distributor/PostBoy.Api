namespace PostBoy.Messages.DTO.Account;

public class RoleDto
{
    public Guid Id { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public DateTime ModifiedOn { get; set; }
    
    public string Name { get; set; }
}