using Mc2.CrudTest.Domain.Commands;
using Mc2.CrudTest.Presentation.Server.Requests.Customers;
using Mc2.CrudTest.SharedKernel.WebApi;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mc2.CrudTest.Presentation.Server.UseCases.Customers.Edit
{
    [Route("[controller]")]
    [ApiController]
    public class CustomersController : CustomController
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] CustomerEditRequest request, CancellationToken cancellationToken)
        {
            var command = new CustomerEditCommand(id, request.FirstName, request.LastName, request.PhoneNumber
                , request.Email, request.DateOfBirth, request.BankAccount);

            var result = await _mediator.Send(command, cancellationToken);
            return FromServiceResult(result);
        }
    }
}
