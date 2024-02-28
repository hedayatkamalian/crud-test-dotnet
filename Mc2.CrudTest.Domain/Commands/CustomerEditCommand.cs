﻿using Mc2.CrudTest.SharedKernel.Domain.Abstraction;
using MediatR;

namespace Mc2.CrudTest.Domain.Commands;

public record CustomerEditCommand(long Id, string FirstName, string LastName, string PhoneNumber,
    string Email, DateOnly DateOfBirth, string BankAccount) : IRequest<ServiceCommandResult>
{

}