namespace Mc2.CrudTest.Application.Dtos;

public class CustomerDto
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string BankAccount { get; set; }
    public string Email { get; set; }
}
