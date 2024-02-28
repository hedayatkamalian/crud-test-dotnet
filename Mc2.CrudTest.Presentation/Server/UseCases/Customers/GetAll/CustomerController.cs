using Mc2.CrudTest.Application.Dtos;
using Mc2.CrudTest.Application.Queries.Customers;
using Mc2.CrudTest.SharedKernel.WebApi;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mc2.CrudTest.Presentation.Server.UseCases.Customers.GetAll;

[Route("[controller]")]
[ApiController]
public class CustomersController : CustomController
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IList<CustomerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new CustomerGetAllQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return FromServiceResult(result);
    }
}
