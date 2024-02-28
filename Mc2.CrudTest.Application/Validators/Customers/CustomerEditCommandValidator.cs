using FluentValidation;
using IbanNet;
using IbanNet.FluentValidation;
using Mc2.CrudTest.Domain.Commands;

namespace Mc2.CrudTest.Application.Validators.Customers;

public class CustomerEditCommandValidator : AbstractValidator<CustomerEditCommand>
{
    public CustomerEditCommandValidator(IIbanValidator ibanValidator)
    {
        RuleFor(customer => customer.FirstName).NotEmpty().NotNull();
        RuleFor(customer => customer.LastName).NotEmpty().NotNull();
        RuleFor(customer => customer.PhoneNumber).MatchPhoneNumberRule().NotEmpty();
        RuleFor(customer => customer.DateOfBirth).NotEmpty().NotNull();
        RuleFor(customer => customer.Email).EmailAddress().NotEmpty().NotNull();
        RuleFor(customer => customer.BankAccount).MinimumLength(10).NotEmpty().Iban(ibanValidator);
    }
}
