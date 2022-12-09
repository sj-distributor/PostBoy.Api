namespace PostBoy.Messages.DTO.Account;

public class UserAccountDto
{
    public UserAccountDto()
    {
        Roles = new List<RoleDto>();
    }
    
    public Guid Id { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public DateTime ModifiedOn { get; set; }
    
    public Guid Uuid { get; set; }
    
    public string UserName { get; set; }
    
    public bool IsActive { get; set; }
    
    public List<RoleDto> Roles { get; set; }
}