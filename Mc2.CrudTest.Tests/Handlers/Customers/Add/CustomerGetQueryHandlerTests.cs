using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Mc2.CrudTest.Application.Dtos;
using Mc2.CrudTest.Application.Handlers.Customers.Queries;
using Mc2.CrudTest.Application.Queries.Customers;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Domain.Repositories;
using Moq;
using Xunit;

namespace Mc2.CrudTest.Tests.Handlers.Customers.Add;

public class CustomerGetQueryHandlerTests
{
    private Fixture _fixture { get { return new Fixture(); } }
    public CustomerGetQueryHandlerTests()
    {

    }


    [Fact]
    public async Task Get_Handler_Should_Return_No_Error_When_Operation_Is_Successfull()
    {

        var query = _fixture.Create<CustomerGetQuery>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(ValidDataSamples.Customer);

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(ValidDataSamples.CustomerDto);

        var customerAddCommandHandler = new CustomerGetQueryHandler(unitOfWorkMock.Object, mapperMock.Object);
        var result = await customerAddCommandHandler.Handle(query, _fixture.Create<CancellationToken>());

        result.HasResult.Should().BeTrue();
        result.HasError.Should().BeFalse();
        result.Result.Should().NotBeNull();
        result.ErrorMessages.Should().BeNull();
    }

    [Fact]
    public async Task Get_Handler_Should_Return_No_Result_When_Id_Not_Exist()
    {

        var query = _fixture.Create<CustomerGetQuery>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Customer);

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(ValidDataSamples.CustomerDto);

        var customerAddCommandHandler = new CustomerGetQueryHandler(unitOfWorkMock.Object, mapperMock.Object);
        var result = await customerAddCommandHandler.Handle(query, _fixture.Create<CancellationToken>());

        result.HasResult.Should().BeFalse();
        result.HasError.Should().BeFalse();
        result.Result.Should().BeNull();
        result.ErrorMessages.Should().BeNull();
    }


}
