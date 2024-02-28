using Mc2.CrudTest.Application.Dtos;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Presentation.Server.Requests.Customers;

namespace Mc2.CrudTest.Tests;

public static class ValidDataSamples
{
    public const string PhoneNumber = "9150102123";
    public const string Email = "hedayat.kamalian@gmail.com";
    public const string Iban = "NL91ABNA0417164300";
    public static readonly DateOnly DateOfBirth = new DateOnly(1985, 01, 13);
    public static Customer Customer = Customer.New(123456, "fname", "lname", DateOfBirth, PhoneNumber, Email, Iban);
    public static CustomerDto CustomerDto = new CustomerDto
    {
        Id = 456321,
        FirstName = "Fname",
        LastName = "Lname",
        Email = Email,
        BankAccount = Iban,
        DateOfBirth = DateOfBirth,
        PhoneNumber = PhoneNumber,

    };

    public static CustomerEditRequest CustomerEditRequest = new CustomerEditRequest
    {
        FirstName = "Fname",
        LastName = "Lname",
        Email = Email,
        BankAccount = Iban,
        DateOfBirth = DateOfBirth,
        PhoneNumber = PhoneNumber,

    };

}
