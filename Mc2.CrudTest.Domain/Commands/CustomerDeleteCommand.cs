using Mc2.CrudTest.SharedKernel.Domain.Abstraction;
using MediatR;

namespace Mc2.CrudTest.Domain.Commands;

public record CustomerDeleteCommand(long Id) : IRequest<ServiceCommandResult>
{ }