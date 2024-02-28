using Mc2.CrudTest.Application.Options;
using Mc2.CrudTest.Domain.Commands;
using Mc2.CrudTest.Domain.Repositories;
using Mc2.CrudTest.SharedKernel.Domain.Abstraction;
using MediatR;
using Microsoft.Extensions.Options;

namespace Mc2.CrudTest.Application.Handlers.Customers.Commands;

public class CustomerDeleteCommandHandler : IRequestHandler<CustomerDeleteCommand, ServiceCommandResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationErrors _applicationErrors;

    public CustomerDeleteCommandHandler(IUnitOfWork unitOfWork,
        IOptions<ApplicationErrors> applicationErrors)
    {
        _unitOfWork = unitOfWork;
        _applicationErrors = applicationErrors.Value;
    }

    public async Task<ServiceCommandResult> Handle(CustomerDeleteCommand command, CancellationToken cancellationToken)
    {

        var customer = await _unitOfWork.CustomerRepository.GetAsync(command.Id, cancellationToken);

        if (customer != null)
        {
            await _unitOfWork.CustomerRepository.DeleteAsync(customer.Id, cancellationToken);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result > 0 ? new ServiceCommandResult() : new ServiceCommandResult(CommandErrorType.General, _applicationErrors.NoRowsWereAffected);
        }
        else
        {
            return new ServiceCommandResult(CommandErrorType.NotFound);
        }

    }

}

