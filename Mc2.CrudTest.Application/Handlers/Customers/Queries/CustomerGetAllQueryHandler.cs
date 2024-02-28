using AutoMapper;
using Mc2.CrudTest.Application.Dtos;
using Mc2.CrudTest.Application.Queries.Customers;
using Mc2.CrudTest.Domain.Repositories;
using Mc2.CrudTest.SharedKernel.Domain.Abstraction;
using MediatR;

namespace Mc2.CrudTest.Application.Handlers.Customers.Queries;

public class CustomerGetAllQueryHandler : IRequestHandler<CustomerGetAllQuery, ServiceQueryResult<IList<CustomerDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CustomerGetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceQueryResult<IList<CustomerDto>>> Handle(CustomerGetAllQuery request, CancellationToken cancellationToken)
    {
        var customers = await _unitOfWork.CustomerRepository.GetAllAsync(cancellationToken);


        return new ServiceQueryResult<IList<CustomerDto>>(_mapper.Map<IList<CustomerDto>>(customers));

    }
}
