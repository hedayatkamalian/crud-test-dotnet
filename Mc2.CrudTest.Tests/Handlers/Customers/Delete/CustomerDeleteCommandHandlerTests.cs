using AutoFixture;
using FluentAssertions;
using Mc2.CrudTest.Application.Handlers.Customers.Commands;
using Mc2.CrudTest.Domain.Commands;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Domain.Repositories;
using Mc2.CrudTest.SharedKernel.Domain.Abstraction;
using Moq;
using Xunit;

namespace Mc2.CrudTest.Tests.Handlers.Customers.Edit;

public class CustomerDeleteCommandHandlerTests
{
    private Fixture _fixture { get { return new Fixture(); } }
    public CustomerDeleteCommandHandlerTests()
    {

    }


    [Fact]
    public async Task Delete_Handler_Should_Return_No_Error_When_Operation_Is_Successfull()
    {

        var customerDeleteCommand = new CustomerDeleteCommand(_fixture.Create<long>());

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(ValidDataSamples.Customer);
        unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);


        var customerDeleteCommandHandler = new CustomerDeleteCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
        var result = await customerDeleteCommandHandler.Handle(customerDeleteCommand, _fixture.Create<CancellationToken>());

        result.WasSuccessfull.Should().BeTrue();
        result.ErrorType.Should().BeNull();
    }

    [Fact]
    public async Task Delete_Handler_Should_Return_Null_When_Customer_Does_Not_Exist()
    {

        var customerDeleteCommand = new CustomerDeleteCommand(_fixture.Create<long>());

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Customer);
        unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);


        var customerDeleteCommandHandler = new CustomerDeleteCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
        var result = await customerDeleteCommandHandler.Handle(customerDeleteCommand, _fixture.Create<CancellationToken>());

        result.WasSuccessfull.Should().BeFalse();
        result.ErrorType.Should().Be(CommandErrorType.NotFound);
    }


}
