using Mc2.CrudTest.Domain.Commands;
using Mc2.CrudTest.SharedKernel.WebApi;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mc2.CrudTest.Presentation.Server.UseCases.Customers.Delete;

[Route("[controller]")]
[ApiController]
public class CustomersController : CustomController
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
    {
        var command = new CustomerDeleteCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        return FromServiceResult(result);
    }
}

