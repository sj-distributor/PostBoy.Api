namespace PostBoy.Messages.DTO.Authentication;

public class UserAccountApiKeyDto 
{
    public Guid Id { get; set; }

    public int UserAccountId { get; set; }
    
    public string ApiKey { get; set; }
}