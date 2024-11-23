namespace workshopManager.Application.Dtos;

public record class CustomerDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
