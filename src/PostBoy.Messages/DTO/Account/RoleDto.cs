namespace PostBoy.Messages.DTO.Account;

public class RoleDto
{
    public Guid Id { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public DateTime ModifiedDate { get; set; }
    
    public string Name { get; set; }
}