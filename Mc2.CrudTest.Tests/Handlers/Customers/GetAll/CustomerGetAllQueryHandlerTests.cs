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

public class CustomerGetAllQueryHandlerTests
{
    private Fixture _fixture { get { return new Fixture(); } }
    public CustomerGetAllQueryHandlerTests()
    {

    }


    [Fact]
    public async Task Get_All_Handler_Should_Always_Return_Success()
    {

        var query = _fixture.Create<CustomerGetAllQuery>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(m => m.CustomerRepository.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<Customer> { ValidDataSamples.Customer });

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<IList<CustomerDto>>(It.IsAny<IList<Customer>>())).Returns(new List<CustomerDto> { ValidDataSamples.CustomerDto });

        var customerAddCommandHandler = new CustomerGetAllQueryHandler(unitOfWorkMock.Object, mapperMock.Object);
        var result = await customerAddCommandHandler.Handle(query, _fixture.Create<CancellationToken>());

        result.Result.Should().NotBeNull();
        result.HasResult.Should().BeTrue();
        result.HasError.Should().BeFalse();
        result.ErrorMessages.Should().BeNull();
    }


}
