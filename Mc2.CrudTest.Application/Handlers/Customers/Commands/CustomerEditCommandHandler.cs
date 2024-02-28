using FluentValidation;
using Mc2.CrudTest.Application.Options;
using Mc2.CrudTest.Domain.Commands;
using Mc2.CrudTest.Domain.Repositories;
using Mc2.CrudTest.SharedKernel.Domain.Abstraction;
using MediatR;
using Microsoft.Extensions.Options;

namespace Mc2.CrudTest.Application.Handlers.Customers.Commands;

public class CustomerEditCommandHandler : IRequestHandler<CustomerEditCommand, ServiceCommandResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CustomerEditCommand> _customerValidator;
    private readonly ApplicationErrors _applicationErrors;

    public CustomerEditCommandHandler(IUnitOfWork unitOfWork,
        IValidator<CustomerEditCommand> customerValidator,
        IOptions<ApplicationErrors> applicationErrors)
    {
        _unitOfWork = unitOfWork;
        _customerValidator = customerValidator;
        _applicationErrors = applicationErrors.Value;
    }

    public async Task<ServiceCommandResult> Handle(CustomerEditCommand command, CancellationToken cancellationToken)
    {
        var validationResult = _customerValidator.Validate(command);

        if (validationResult.IsValid)
        {
            var customer = await _unitOfWork.CustomerRepository.GetAsync(command.Id, cancellationToken);

            if (customer != null)
            {
                var anotherCustomerWithNewValueExist = await _unitOfWork.CustomerRepository.GetAsync
                (c => c.FirstName == command.FirstName && c.LastName == command.LastName &&
                c.DateOfBirth == command.DateOfBirth && c.Id != customer.Id, cancellationToken);

                if (anotherCustomerWithNewValueExist != null)
                {
                    return new ServiceCommandResult(CommandErrorType.Validation, _applicationErrors.DuplicatedCustomer);
                }

                var anotherCustomerWithNewEmailExist = await _unitOfWork.CustomerRepository.GetAsync
                (c => c.Email == command.Email && c.Id != customer.Id, cancellationToken);

                if (anotherCustomerWithNewEmailExist != null)
                {
                    return new ServiceCommandResult(CommandErrorType.Validation, _applicationErrors.ThisEmailUsedBefore);
                }

                customer.Edit(command.FirstName, command.LastName, command.DateOfBirth, command.PhoneNumber, command.Email, command.BankAccount);

                var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result > 0 ? new ServiceCommandResult() : new ServiceCommandResult(CommandErrorType.General, _applicationErrors.NoRowsWereAffected);
            }
            else
            {
                return new ServiceCommandResult(CommandErrorType.NotFound);
            }

        }
        else
        {
            return new ServiceCommandResult(CommandErrorType.Validation, validationResult.Errors.Select(p => p.ErrorMessage).ToArray());
        }

    }
}
