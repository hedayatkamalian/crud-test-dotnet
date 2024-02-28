﻿using Mc2.CrudTest.Application.Dtos;
using Mc2.CrudTest.SharedKernel.Domain.Abstraction;
using MediatR;

namespace Mc2.CrudTest.Application.Queries.Customers;

public record CustomerGetAllQuery() : IRequest<ServiceQueryResult<IList<CustomerDto>>>
{
}