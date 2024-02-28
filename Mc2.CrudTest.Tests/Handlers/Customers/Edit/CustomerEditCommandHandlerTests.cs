﻿using AutoFixture;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Mc2.CrudTest.Application.Handlers.Customers.Commands;
using Mc2.CrudTest.Domain.Commands;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Domain.Repositories;
using Mc2.CrudTest.SharedKernel.Domain.Abstraction;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Mc2.CrudTest.Tests.Handlers.Customers.Edit;

public class CustomerEditCommandHandlerTests
{
    private Fixture _fixture { get { return new Fixture(); } }
    public CustomerEditCommandHandlerTests()
    {

    }


    [Fact]
    public async Task Edit_Handler_Should_Return_No_Error_When_Operation_Is_Successfull()
    {

        var customerAddCommand = new CustomerEditCommand(_fixture.Create<long>(), _fixture.Create<string>(), _fixture.Create<string>(), ValidDataSamples.PhoneNumber, ValidDataSamples.Email,
            ValidDataSamples.DateOfBirth, ValidDataSamples.Iban);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(ValidDataSamples.Customer);
        unitOfWorkMock.SetupSequence(m => m.CustomerRepository.GetAsync(It.IsAny<Expression<Func<Customer, bool>>>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(null as Customer)
           .ReturnsAsync(null as Customer);

        unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var validator = new Mock<IValidator<CustomerEditCommand>>();
        var validationResult = new ValidationResult();
        validator.Setup(v => v.Validate(It.IsAny<CustomerEditCommand>())).Returns(validationResult);

        var customerAddCommandHandler = new CustomerEditCommandHandler(unitOfWorkMock.Object, validator.Object, CommonMocks.ApplicationErros);
        var result = await customerAddCommandHandler.Handle(customerAddCommand, _fixture.Create<CancellationToken>());

        result.WasSuccessfull.Should().BeTrue();
        result.ErrorType.Should().BeNull();
    }

    [Fact]
    public async Task Edit_Handler_Should_Return_Validation_Error_When_Command_Is_Not_Valid()
    {

        var customerAddCommand = new CustomerEditCommand(_fixture.Create<long>(), _fixture.Create<string>(), _fixture.Create<string>(), ValidDataSamples.PhoneNumber, ValidDataSamples.Email,
           ValidDataSamples.DateOfBirth, ValidDataSamples.Iban);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(ValidDataSamples.Customer);
        unitOfWorkMock.SetupSequence(m => m.CustomerRepository.GetAsync(It.IsAny<Expression<Func<Customer, bool>>>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(null as Customer)
           .ReturnsAsync(null as Customer);

        unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var validator = new Mock<IValidator<CustomerEditCommand>>();
        var validationResult = new ValidationResult(new ValidationFailure[] { new ValidationFailure { ErrorMessage = "an error" } });
        validator.Setup(v => v.Validate(It.IsAny<CustomerEditCommand>())).Returns(validationResult);

        var customerAddCommandHandler = new CustomerEditCommandHandler(unitOfWorkMock.Object, validator.Object, CommonMocks.ApplicationErros);
        var result = await customerAddCommandHandler.Handle(customerAddCommand, _fixture.Create<CancellationToken>());

        result.WasSuccessfull.Should().BeFalse();
        result.ErrorType.Should().NotBeNull();
        result.ErrorType.Should().Be(CommandErrorType.Validation);
    }

    [Fact]
    public async Task Edit_Handler_Should_Return_Validation_Error_When_Email_Is_Duplicate()
    {

        var customerAddCommand = new CustomerEditCommand(_fixture.Create<long>(), _fixture.Create<string>(), _fixture.Create<string>(), ValidDataSamples.PhoneNumber, ValidDataSamples.Email,
           ValidDataSamples.DateOfBirth, ValidDataSamples.Iban);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(ValidDataSamples.Customer);
        unitOfWorkMock.SetupSequence(m => m.CustomerRepository.GetAsync(It.IsAny<Expression<Func<Customer, bool>>>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(null as Customer)
           .ReturnsAsync(ValidDataSamples.Customer);

        unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var validator = new Mock<IValidator<CustomerEditCommand>>();
        var validationResult = new ValidationResult();
        validator.Setup(v => v.Validate(It.IsAny<CustomerEditCommand>())).Returns(validationResult);

        var customerAddCommandHandler = new CustomerEditCommandHandler(unitOfWorkMock.Object, validator.Object, CommonMocks.ApplicationErros);
        var result = await customerAddCommandHandler.Handle(customerAddCommand, _fixture.Create<CancellationToken>());

        result.WasSuccessfull.Should().BeFalse();
        result.ErrorType.Should().NotBeNull();
        result.ErrorType.Should().Be(CommandErrorType.Validation);
    }

    [Fact]
    public async Task Edit_Handler_Should_Return_Validation_Error_When_Customer_Is_Duplicate()
    {

        var customerAddCommand = new CustomerEditCommand(_fixture.Create<long>(), _fixture.Create<string>(), _fixture.Create<string>(), ValidDataSamples.PhoneNumber, ValidDataSamples.Email,
           ValidDataSamples.DateOfBirth, ValidDataSamples.Iban);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(ValidDataSamples.Customer);
        unitOfWorkMock.SetupSequence(m => m.CustomerRepository.GetAsync(It.IsAny<Expression<Func<Customer, bool>>>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(ValidDataSamples.Customer)
           .ReturnsAsync(null as Customer);

        unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var validator = new Mock<IValidator<CustomerEditCommand>>();
        var validationResult = new ValidationResult();
        validator.Setup(v => v.Validate(It.IsAny<CustomerEditCommand>())).Returns(validationResult);

        var customerAddCommandHandler = new CustomerEditCommandHandler(unitOfWorkMock.Object, validator.Object, CommonMocks.ApplicationErros);
        var result = await customerAddCommandHandler.Handle(customerAddCommand, _fixture.Create<CancellationToken>());

        result.WasSuccessfull.Should().BeFalse();
        result.ErrorType.Should().NotBeNull();
        result.ErrorType.Should().Be(CommandErrorType.Validation);
    }
}