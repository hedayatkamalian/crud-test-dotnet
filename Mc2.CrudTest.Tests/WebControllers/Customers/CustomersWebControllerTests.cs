﻿using AutoFixture;
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
}
