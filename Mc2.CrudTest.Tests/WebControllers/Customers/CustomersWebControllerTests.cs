using AutoFixture;
using FluentAssertions;
using Mc2.CrudTest.Application.Dtos;
using Mc2.CrudTest.Application.Queries.Customers;
using Mc2.CrudTest.Domain.Commands;
using Mc2.CrudTest.Presentation.Server.Requests.Customers;
using Mc2.CrudTest.SharedKernel.Domain.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Mc2.CrudTest.Tests.WebControllers.Customers;

public class CustomersWebControllerTests
{
    private Fixture _fixture { get { return new Fixture(); } }
    public CustomersWebControllerTests()
    {

    }

    [Fact]
    public async Task Add_Should_Return_Unsupportable_Entity_When_Command_Result_Has_Validation_Error()
    {
        var mediatorMock = new Mock<IMediator>();
        var serviceResult = new ServiceCommandResult(CommandErrorType.Validation);
        mediatorMock.Setup(m => m.Send(It.IsAny<CustomerAddCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

        var customerController = new Presentation.Server.UseCases.Customers.Add.CustomersController(mediatorMock.Object);
        customerController.Url = GeneralFixtures.CreateMockUrlHelper().Object;

        var request = new CustomerAddRequest
        {
            FirstName = _fixture.Create<string>(),
            LastName = _fixture.Create<string>(),
            DateOfBirth = ValidDataSamples.DateOfBirth,
            BankAccount = _fixture.Create<string>(),
            PhoneNumber = _fixture.Create<string>(),
            Email = _fixture.Create<string>()
        };

        var result = await customerController.Add(request, _fixture.Create<CancellationToken>());


        result.Should().NotBeNull();
        result.Should().BeOfType<UnprocessableEntityObjectResult>();
    }

    [Fact]
    public async Task Add_Should_Return_NoContent_When_Command_Result_Is_Successfull()
    {
        var serviceResult = new ServiceCommandResult(_fixture.Create<string>());
        serviceResult.Uri = _fixture.Create<string>();

        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<CustomerAddCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

        var customerController = new Presentation.Server.UseCases.Customers.Add.CustomersController(mediatorMock.Object);
        customerController.Url = GeneralFixtures.CreateMockUrlHelper().Object;

        var request = new CustomerAddRequest
        {
            FirstName = _fixture.Create<string>(),
            LastName = _fixture.Create<string>(),
            DateOfBirth = ValidDataSamples.DateOfBirth,
            BankAccount = _fixture.Create<string>(),
            PhoneNumber = _fixture.Create<string>(),
            Email = _fixture.Create<string>()
        };

        var result = await customerController.Add(request, _fixture.Create<CancellationToken>());

        result.Should().NotBeNull();
        result.Should().BeOfType<CreatedResult>();
    }

    [Fact]
    public async Task Get_Should_Return_OK_With_CustomerDto_Result_When_QueryResult_Has_No_Error()
    {
        var mediatorMock = new Mock<IMediator>();
        var serviceResult = new ServiceQueryResult<CustomerDto>(ValidDataSamples.CustomerDto);
        mediatorMock.Setup(m => m.Send(It.IsAny<CustomerGetQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

        var customerController = new Presentation.Server.UseCases.Customers.Get.CustomersController(mediatorMock.Object);
        var result = await customerController.Get(_fixture.Create<long>(), _fixture.Create<CancellationToken>());

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeOfType<CustomerDto>();
    }

    [Fact]
    public async Task Get_Should_Return_NotFound_When_QueryResult_Has_NotFound_Error()
    {
        var mediatorMock = new Mock<IMediator>();
        var serviceResult = new ServiceQueryResult<CustomerDto>(null as CustomerDto);
        mediatorMock.Setup(m => m.Send(It.IsAny<CustomerGetQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

        var customerController = new Presentation.Server.UseCases.Customers.Get.CustomersController(mediatorMock.Object);
        var result = await customerController.Get(_fixture.Create<long>(), _fixture.Create<CancellationToken>());

        result.Should().NotBeNull();
        result.Should().BeOfType<NotFoundResult>();

    }


    [Fact]
    public async Task Edit_Should_Return_Unsupportable_Entity_When_Command_Result_Has_Validation_Error()
    {
        var mediatorMock = new Mock<IMediator>();
        var serviceResult = new ServiceCommandResult(CommandErrorType.Validation);
        mediatorMock.Setup(m => m.Send(It.IsAny<CustomerEditCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

        var customerController = new Presentation.Server.UseCases.Customers.Edit.CustomersController(mediatorMock.Object);
        var result = await customerController.Edit(_fixture.Create<long>(), ValidDataSamples.CustomerEditRequest, _fixture.Create<CancellationToken>());

        result.Should().NotBeNull();
        result.Should().BeOfType<UnprocessableEntityObjectResult>();
    }

    [Fact]
    public async Task Edit_Should_Return_NoContent_When_Command_Result_Is_Successfull()
    {
        var mediatorMock = new Mock<IMediator>();
        var serviceResult = new ServiceCommandResult();
        mediatorMock.Setup(m => m.Send(It.IsAny<CustomerEditCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

        var customerController = new Presentation.Server.UseCases.Customers.Edit.CustomersController(mediatorMock.Object);
        var result = await customerController.Edit(_fixture.Create<long>(), ValidDataSamples.CustomerEditRequest, _fixture.Create<CancellationToken>());

        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Edit_Should_Return_Not_Found_When_Command_Result_Is_NotFound()
    {
        var mediatorMock = new Mock<IMediator>();
        var serviceResult = new ServiceCommandResult(CommandErrorType.NotFound);
        mediatorMock.Setup(m => m.Send(It.IsAny<CustomerEditCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

        var customerController = new Presentation.Server.UseCases.Customers.Edit.CustomersController(mediatorMock.Object);
        var result = await customerController.Edit(_fixture.Create<long>(), ValidDataSamples.CustomerEditRequest, _fixture.Create<CancellationToken>());

        result.Should().NotBeNull();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetAll_Should_Return_OK_AnyTime()
    {
        var mediatorMock = new Mock<IMediator>();
        var serviceResult = new ServiceQueryResult<IList<CustomerDto>>(new List<CustomerDto> { ValidDataSamples.CustomerDto });
        mediatorMock.Setup(m => m.Send(It.IsAny<CustomerGetAllQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

        var customerController = new Presentation.Server.UseCases.Customers.GetAll.CustomersController(mediatorMock.Object);
        var result = await customerController.GetAll(_fixture.Create<CancellationToken>());

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeAssignableTo<IList<CustomerDto>>();
    }

    [Fact]
    public async Task Delete_Should_Return_Not_Found_When_Command_Result_Is_NotFound()
    {
        var mediatorMock = new Mock<IMediator>();
        var serviceResult = new ServiceCommandResult(CommandErrorType.NotFound);
        mediatorMock.Setup(m => m.Send(It.IsAny<CustomerDeleteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

        var customerController = new Presentation.Server.UseCases.Customers.Delete.CustomersController(mediatorMock.Object);
        var result = await customerController.Delete(_fixture.Create<long>(), _fixture.Create<CancellationToken>());

        result.Should().NotBeNull();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_Should_Return_NoContent_When_Command_Result_Is_Successful()
    {
        var mediatorMock = new Mock<IMediator>();
        var serviceResult = new ServiceCommandResult();
        mediatorMock.Setup(m => m.Send(It.IsAny<CustomerDeleteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

        var customerController = new Presentation.Server.UseCases.Customers.Delete.CustomersController(mediatorMock.Object);
        var result = await customerController.Delete(_fixture.Create<long>(), _fixture.Create<CancellationToken>());

        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
    }
}
